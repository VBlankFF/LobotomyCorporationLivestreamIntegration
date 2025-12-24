using Harmony;
using SharedMemory;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace LiveStreamIntegration
{
    static class Constants
    {
        public const int BUFFER_SIZE = 50;
        public const int NUM_VOTING_OPTIONS = 3;
        public const float MAX_VOTE_TIME = 60f;
    }
    public class Harmony_Patch
    {
        public static CircularBuffer buffer;
        public static VoteUI votingUI;
        public static Dictionary<string, int> recordedUserVotes;
        public static Dictionary<int, int> recordedOptionVotes;
        public static List<Effect> effects;
        public static bool isVotingActive = false;
        public static Effect[] currentVotableEffects;
        public static bool isVotingTimeInRealTime = false;
        public Harmony_Patch() 
        {
            // Make the CircularBuffer that the mod uses to communicate with Streamer.Bot, or load it if it already exists
            try
            {
                buffer = new CircularBuffer("LobotomyCorporationLivestreamIntegration", 2000, 50);
            }
            catch
            {
                buffer = new CircularBuffer("LobotomyCorporationLivestreamIntegration");
            }
            recordedOptionVotes = new Dictionary<int, int>();
            recordedUserVotes = new Dictionary<string, int>();
            effects = Effect.LoadEffects();
            currentVotableEffects = new Effect[Constants.NUM_VOTING_OPTIONS];
            HarmonyInstance HInstance = HarmonyInstance.Create("LobotomyCorporationLivestreamIntegration");
            HarmonyMethod getAllFromBuffer = new HarmonyMethod(typeof(Harmony_Patch).GetMethod("GetAllFromBuffer"));
            HInstance.Patch(typeof(GlobalGameManager).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance), null, getAllFromBuffer, null);
            // Create the voting UI on day start
            HarmonyMethod voteUI = new HarmonyMethod(typeof(Harmony_Patch).GetMethod("StartVoteUI"));
            HInstance.Patch(typeof(DeployUI).GetMethod("OnManagementStart", BindingFlags.Public | BindingFlags.Instance), null, voteUI, null);
            // Destroy the voting UI at certain points
            HarmonyMethod destroyUI = new HarmonyMethod(typeof(Harmony_Patch).GetMethod("DestroyVoteUI"));
            HInstance.Patch(typeof(DayEndScene).GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance), null, destroyUI, null);
            HInstance.Patch(typeof(GameManager).GetMethod("RestartGame", BindingFlags.Public | BindingFlags.Instance), null, destroyUI, null);
            HInstance.Patch(typeof(DeployUI).GetMethod("Init", BindingFlags.Public | BindingFlags.Instance), null, destroyUI, null);
            // Create the settings UI
            HarmonyMethod settingsUI = new HarmonyMethod(typeof(HarmonyPatch).GetMethod("MakeUI"));
            HInstance.Patch(typeof(AlterTitleController).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance), null, settingsUI, null);

        }
        // Begins the voting process by enabling voting, resetting the votes and time, and selecting new options.
        public static void InitVoting()
        {
            isVotingActive = true;
            currentVotableEffects = ChooseNewVotableEffects();
            ResetOptionVotes(recordedOptionVotes);
            ResetUserVotes(recordedUserVotes);
            UpdateVoteUINames();
            votingUI.voteTime = Constants.MAX_VOTE_TIME;
            votingUI.SetDisplayVoteTime((int)Constants.MAX_VOTE_TIME);
        }
        // Deletes the Vote UI and stops voting (if the Vote UI exists)
        public static void DestroyVoteUI()
        {
            if (votingUI is null)
            {
                return;
            }
            GameObject.Destroy(votingUI);
            votingUI = null;
            isVotingActive = false;
        }
        // Resets the votes for every option to 0
        public static void ResetOptionVotes(Dictionary<int, int> optionVotes)
        {
            for (int i = 0; i < Constants.NUM_VOTING_OPTIONS; i++)
            {
                optionVotes[i] = 0;
                votingUI.voteOptions[i].SetNumVotes(0);
            }
        }
        // Removes the vote associated with each user, so they can vote without their previous selection having a vote removed.
        public static void ResetUserVotes(Dictionary<string, int> userVotes)
        {
            userVotes.Clear();
        }
        // Gets every vote from the circular buffer. The vote(s) from a user are associated with them, and each vote is counted and switched if necessary
        public static void GetAllFromBuffer()
        {
            byte[] bufferEntry = new byte[Constants.BUFFER_SIZE];
            bool changed = false;
            while(buffer.Read(bufferEntry, 0, 0) > 0)
            {
                // Get the Id of the user that voted from the buffer
                char vote = BitConverter.ToChar(bufferEntry, 0);
                char[] userId = new char[(Constants.BUFFER_SIZE / 2) - 1];
                for (int i = 2; i < Constants.BUFFER_SIZE; i += 2)
                {
                    userId[(i / 2) - 1] = BitConverter.ToChar(bufferEntry, i);
                }   
                LobotomyBaseMod.ModDebug.Log("vote = " + vote + " uid = " + new string(userId));
                // Get the option they voted for, if possible
                int voteInt;
                if (!Int32.TryParse(vote.ToString(), out voteInt))
                {
                    continue;
                }
                if (voteInt > Constants.NUM_VOTING_OPTIONS)
                {
                    continue;
                }
                // If the user has already voted in this voting period, remove their previous vote
                int existingVote;
                if (!recordedUserVotes.TryGetValue(new string(userId), out existingVote) || existingVote != voteInt)
                {
                    if (recordedUserVotes.TryGetValue(new string(userId), out existingVote))
                    {
                        recordedOptionVotes[existingVote - 1]--;
                    }
                    // Apply the user's (new) vote
                    recordedUserVotes[new string(userId)] = voteInt;
                    recordedOptionVotes[voteInt - 1]++;
                    changed = true;
                }
            }
            // Update the Vote UI if anything changed
            if (changed)
            {
                if (votingUI is null)
                {
                    return;
                }
                for (int i = 0; i < Constants.NUM_VOTING_OPTIONS; i++)
                {
                    votingUI.voteOptions[i].SetNumVotes(recordedOptionVotes[i]);
                }
            }
        }
        // Activated the effect with the highest vote count (lower numbers win ties) and restarts voting
        public static void OnVoteTimerEnd()
        {
            RunWinningEffect();
            InitVoting();
        }
        // Creates the Vote UI and initializes voting
        public static void StartVoteUI()
        {
            if (votingUI is null)
            {
                isVotingActive = true;
                votingUI = new VoteUI();
                votingUI.Init();
                InitVoting();
            }
        }
        // Updates the Vote UI to show the names of the current options
        public static void UpdateVoteUINames()
        {
            if (votingUI is null)
            {
                return;
            }
            for (int i = 0; i < Constants.NUM_VOTING_OPTIONS; i++)
            {
                votingUI.voteOptions[i].SetName(currentVotableEffects[i].GetName());
            }
        }
        // Returns a list of random enabled Effects to use for voting
        public static Effect[] ChooseNewVotableEffects()
        {
            List<Effect> effectPool = new List<Effect>();
            Effect[] retval = new Effect[Constants.NUM_VOTING_OPTIONS];
            foreach (var i in effects)
            {
                // if the Effect is enabled and its votable condition is true, add it to the pool
                if (i.IsVotable())
                {
                    effectPool.Add(i);
                }
            }
            // Get (the desired amount of) random options from the voting pool. These are the new options to vote on.
            for (int i = 0; i < Constants.NUM_VOTING_OPTIONS; i++)
            {
                int randomEffect = UnityEngine.Random.Range(0, effectPool.Count);
                retval[i] = effectPool[randomEffect];
                effectPool.RemoveAt(randomEffect);
            }
            return retval;
        }
        // Selects the Effect with the most votes (lower numbers win ties) and invokes it
        public static void RunWinningEffect()
        {
            Effect highestEffect = currentVotableEffects[0];
            int votesForHighest = recordedOptionVotes[0];
            // Get the effect with the most votes
            for (int i = 1; i < Constants.NUM_VOTING_OPTIONS; i++)
            {
                if (recordedOptionVotes[i] > votesForHighest)
                {
                    highestEffect = currentVotableEffects[i];
                    votesForHighest = recordedOptionVotes[i];
                }
            }
            highestEffect.GetEffectMethod().Invoke(null, null);
        }
        public static void MakeUI()
        {
            GameObject MakeUIObj = new GameObject("Make Settings UI");
            MakeUIObj.AddComponent<SettingsUI.MakeUI>();
        }
    }
}
