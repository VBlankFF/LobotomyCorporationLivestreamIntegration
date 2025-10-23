using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace LiveStreamIntegration
{
    public static class EffectDefinitions
    {
        public static List<Effect> effects;
        public static List<Effect> GetEffects()
        {
            if (effects is null)
            {
                InitEffects();
            }
            return effects;
        }
        public static void InitEffects()
        {
            effects = new List<Effect>();
            Effect nobullets = new Effect(typeof(EffectDefinitions).GetMethod("NoBullets", BindingFlags.Static | BindingFlags.Public), "Set bullets to 0");
            effects.Add(nobullets);
            Effect pauseGame = new Effect(typeof(EffectDefinitions).GetMethod("Pause", BindingFlags.Static | BindingFlags.Public), "Pause game");
            effects.Add(pauseGame);
            Effect trainSound = new Effect(typeof(EffectDefinitions).GetMethod("PlayTrainSound", BindingFlags.Static | BindingFlags.Public), "Train horn");
            effects.Add(trainSound);
            /*Effect sendTrain = new Effect(typeof(EffectDefinitions).GetMethod("SendTrain", BindingFlags.Static | BindingFlags.Public), "Send train");
            effects.Add(sendTrain);*/
            Effect wideAgents = new Effect(typeof(EffectDefinitions).GetMethod("WideAgents", BindingFlags.Static | BindingFlags.Public), "Wide agents for 90s");
            effects.Add(wideAgents);
            Effect noPause = new Effect(typeof(EffectDefinitions).GetMethod("BlockPausing", BindingFlags.Static | BindingFlags.Public), "No pausing for 60s");
            effects.Add(noPause);
            Effect wanderAgent = new Effect(typeof(EffectDefinitions).GetMethod("RandomWander", BindingFlags.Static | BindingFlags.Public), "Wandering agents");
            effects.Add(wanderAgent);
            Effect uncancelableWork = new Effect(typeof(EffectDefinitions).GetMethod("NoWorkCancel", BindingFlags.Static | BindingFlags.Public), "Can't cancel work 3m");
            effects.Add(uncancelableWork);
            Effect overloads = new Effect(typeof(EffectDefinitions).GetMethod("RandomOverloads", BindingFlags.Static | BindingFlags.Public), "5 random overloads");
            effects.Add(overloads);
            /*Effect causeFear = new Effect(typeof(EffectDefinitions).GetMethod("CauseFearToAll", BindingFlags.Static | BindingFlags.Public), "Scare all agents");
            effects.Add(causeFear);*/
            return;
        }
        public static void NoBullets()
        {
            GlobalBullet.GlobalBulletManager.instance.currentBullet = 0;
        }
        public static void Pause()
        {
            PlaySpeedSettingUI.instance.OnPause(PAUSECALL.NONE);
        }
        public static void PlayTrainSound()
        {
            SoundEffectPlayer.PlayOnce("creature/HellTrain/Train_Start", CameraMover.instance.transform.position);
        }
        public static void SendTrain()
        {

        }
        public static void WideAgents()
        {
            foreach (AgentModel agent in AgentManager.instance.GetAgentList())
            {
                if (agent.IsDead() || !agent.activated) continue;
                WideBuf becomeWide = new WideBuf();
                agent.AddUnitBuf(becomeWide);
            }
        }
        public static void BlockPausing()
        {

        }
        public static void RandomWander()
        {
            foreach (AgentModel agent in AgentManager.instance.GetAgentList())
            {
                if (agent.IsDead() || !agent.activated || agent.GetState() != AgentAIState.IDLE) continue;
                agent.SetWaitingPassage(MapGraph.instance.GetRoamingNodeByRandom().GetAttachedPassage());
            }
        }
        public static void NoWorkCancel()
        {
            SefiraBossManager.Instance.SetWorkCancelableState(false);
            EffectAfterTime.instance.AddEffect(typeof(EffectDefinitions).GetMethod("NoWorkCancelEnd"), 180f);
        }
        public static void NoWorkCancelEnd()
        {
            SefiraBossManager.Instance.SetWorkCancelableState(true);
        }
        public static void RandomOverloads()
        {
            CreatureOverloadManager.instance.ActivateOverload(5, OverloadType.DEFAULT, 60f);
        }
    }
    public class WideBuf : UnitBuf
    {
        public WideBuf()
        {
            this.remainTime = 90f;
        }
        public override void Init(UnitModel model)
        {
            base.Init(model);
            Vector3 unitScale = (this.model as AgentModel).GetWorkerUnit().animRoot.transform.localScale;
            (this.model as AgentModel).GetWorkerUnit().animRoot.transform.localScale = new Vector3(unitScale.x * 5, unitScale.y, unitScale.z);
        }
        public override void OnDestroy()
        {
            if (this.model is null)
            {
                return;
            }
            Vector3 unitScale = (this.model as AgentModel).GetWorkerUnit().animRoot.transform.localScale;
            (this.model as AgentModel).GetWorkerUnit().animRoot.transform.localScale = new Vector3(unitScale.x / 5, unitScale.y, unitScale.z);
            base.OnDestroy();
        }
    }
}
