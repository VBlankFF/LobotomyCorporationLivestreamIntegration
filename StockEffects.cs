using Harmony;
using LobotomyBaseModLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using static RandomEventBase;

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
            //effects.Add(noPause);
            Effect wanderAgent = new Effect(typeof(EffectDefinitions).GetMethod("RandomWander", BindingFlags.Static | BindingFlags.Public), "Wandering agents");
            effects.Add(wanderAgent);
            // The name can't be much longer than this without it either taking 2 lines or overlapping the vote number (neither looks good)
            Effect uncancelableWork = new Effect(typeof(EffectDefinitions).GetMethod("NoWorkCancel", BindingFlags.Static | BindingFlags.Public), "Can't cancel work for 3m");
            effects.Add(uncancelableWork);
            Effect overloads = new Effect(typeof(EffectDefinitions).GetMethod("RandomOverloads", BindingFlags.Static | BindingFlags.Public), "5 random overloads");
            effects.Add(overloads);
            /*Effect causeFear = new Effect(typeof(EffectDefinitions).GetMethod("CauseFearToAll", BindingFlags.Static | BindingFlags.Public), "Scare all agents");
            effects.Add(causeFear);*/
            Effect fakeDeath = new Effect(typeof(EffectDefinitions).GetMethod("FakeDeath", BindingFlags.Static | BindingFlags.Public), "Fake death");
            effects.Add(fakeDeath);
            Effect contractWorkers = new Effect(typeof(ContractWorkers).GetMethod("SummonContractWorkersEffect", BindingFlags.Static | BindingFlags.Public), "Hire contractors", votableCondition: typeof(ContractWorkers).GetMethod("CanDoEffect", BindingFlags.Static | BindingFlags.Public));
            effects.Add(contractWorkers);
            Effect shieldAgents = new Effect(typeof(EffectDefinitions).GetMethod("ShieldWorkers", BindingFlags.Static | BindingFlags.Public), "Shield workers");
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
        public static void FakeDeath()
        {
            IList<AgentModel> agentList = AgentManager.instance.GetAgentList();
            if (agentList is null || agentList.Count == 0) return;
            AgentModel fakeDeadAgent = agentList[UnityEngine.Random.Range(0, agentList.Count)];
            SefiraMessage sefiraMessage = new SefiraMessage();
            string d = sefiraMessage.desc;
            Sefira agentSefira = fakeDeadAgent.GetCurrentSefira();
            if (agentSefira is null) return;
            int sefiraIndex = agentSefira.index;
            if (SefiraConversationController.Instance.CheckMuted(sefiraIndex))
            {
                return;
            }
            bool isRobot = MissionManager.instance.ExistsFinishedBossMission(fakeDeadAgent.GetCurrentSefira().sefiraEnum);
            if (sefiraIndex == 5 ||  sefiraIndex == 6)
            {
                int tiphType = UnityEngine.Random.Range(0, 2);
                sefiraIndex = tiphType != 0 ? 6 : 5;
                d = Conversation.instance.GetSefiraMessage(sefiraIndex, 1, tiphType, isRobot).desc.Replace("#0", "<color=#66bfcd>" + fakeDeadAgent.name + "</color>");
            }
            else
            {
                d = Conversation.instance.GetSefiraMessage(sefiraIndex, 1, isRobot).desc.Replace("#0", "<color=#66bfcd>" + fakeDeadAgent.name + "</color>");
            }
           
            if (SefiraBossManager.Instance.IsAnyBossSessionActivated() || PlayerModel.instance.GetDay() >= 45 || MissionManager.instance.ExistsBossMission(agentSefira.sefiraEnum))
                return;
            string name = agentSefira.name;
            SefiraConversationController.Instance.UpdateConversation(CharacterResourceDataModel.instance.GetSefiraPortrait(agentSefira.sefiraEnum, false), CharacterResourceDataModel.instance.GetColor(name), d);
            AngelaConversation.instance.MakeDefaultFormatMessage(AngelaMessageState.AGENT_DEAD_HEALTH, (object)fakeDeadAgent);
        }
        public static void ShieldWorkers()
        {
            foreach (WorkerModel worker in WorkerManager.instance.GetWorkerList())
            {
                if (worker is null || worker.IsDead()) continue;
                if (worker is AgentModel && !(worker as AgentModel).activated) continue;
                worker.AddUnitBuf(new global::BarrierBuf(global::RwbpType.A, 100f, 30f));
            }
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
    public class NoneSefira : Sefira
    {
        public NoneSefira(string name, int index, string indexString, SefiraEnum sefiraEnum) : base(name, index, indexString, sefiraEnum)
        {
            this.index = Int32.MaxValue - 5;

        }
    }
    public class ContractWorkers : global::IObserver 
    {
        private static ContractWorkers _instance;
        public static ContractWorkers instance { get
            {
                if ( _instance == null )
                {
                    _instance = new ContractWorkers();
                }
                return _instance;
            }

            set => _instance = value; }
        public const int NUM_WORKERS = 3;
        public static AgentModel[] workers = new AgentModel[NUM_WORKERS];
        static ContractWorkers()
        {
            
        }
        public static bool CanDoEffect()
        {
            if (!(workers[0] is null))
            {
                return false;
            }
            if (PlayerModel.emergencyController.currentLevel == EmergencyLevel.NORMAL)
            {
                return false;
            }
            return true;
        }
        public static void SummonContractWorkersEffect()
        {
            for (int i = 0; i < workers.Length; i++)
            {
                workers[i] = new AgentModel(i + 10000)
                {
                    currentSefira = "1"
                };
                workers[i]._agentName = AgentNameList.instance.GetRandomNameByInfo();
                workers[i].name = workers[i]._agentName.GetName();
                
            }
            SetStats();
            SetEquips();
            List<AgentModel> agentList = (List<AgentModel>)typeof(AgentManager).GetField("agentList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(AgentManager.instance);
            foreach (var a in workers)
            {
                a.hp = (float)a.maxHp;
                a.mental = (float)a.maxMental;
                WorkerSpriteManager.instance.GetRandomBasicData(a.spriteData, true);
                WorkerSpriteManager.instance.GetArmorData(0, ref a.spriteData);
                AgentLayer.currentLayer.AddAgent(a);
                agentList.Add(a);
                a.activated = true;
                a.GetMovableNode().SetActive(true);
                a.OnStageStart();
            }
            typeof(AgentManager).GetField("agentList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(AgentManager.instance, agentList);
            AngelaConversationUI.instance.AddAngelaMessage("I've hired a few temporary workers, since you clearly need them. They've just entered Control Team's main room.");
            // Make the message last 10 seconds, probably
            typeof(AngelaConversationUI).GetField("displayElapsed", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(AngelaConversationUI.instance, -9f);
        }
        public static void EndAngelaMessage()
        {
        }
        public static void SetStats()
        {
            if (workers[0] is null)
            {
                return;
            }
            int r = 60;
            int w = 60;
            int b = 60;
            int p = 60;
            foreach (var a in AgentManager.instance.GetAgentList())
            {
                if (a.primaryStat.hp > r) r = a.primaryStat.hp;
                if (a.primaryStat.mental > w) w = a.primaryStat.mental;
                if (a.primaryStat.work > b) b = a.primaryStat.work;
                if (a.primaryStat.battle > p) p = a.primaryStat.battle;
            }
            foreach (var a in workers)
            {
                a.primaryStat.hp = r - UnityEngine.Random.Range(0, 10);
                a.primaryStat.mental = w - UnityEngine.Random.Range(0, 10);
                a.primaryStat.work = b - UnityEngine.Random.Range(0, 10);
                a.primaryStat.battle = p - UnityEngine.Random.Range(0, 10);
            }
            
        }
        public static void SetEquips()
        {
            List<ArmorModel>[] armorByRisk = new List<ArmorModel>[5];
            List<WeaponModel>[] weaponsByRisk = new List<WeaponModel>[5];
            for (int i = 0; i < armorByRisk.Length; i++)
            {
                armorByRisk[i] = new List<ArmorModel>();
                weaponsByRisk[i] = new List<WeaponModel>();
            }
            // Get equipment the player has, seperated by grade
            foreach (var equipment in InventoryModel.Instance.GetAllEquipmentList())
            {
                // I don't trust modded ego to not explode if created this way
                if (equipment.metaInfo.modid != string.Empty) continue;
                if (!Int32.TryParse(equipment.metaInfo.grade, out int grade)) continue;
                if (grade > 5) continue;
                if (equipment is WeaponModel)
                {
                    weaponsByRisk[grade - 1].Add(equipment as WeaponModel);
                }
                else if (equipment is ArmorModel)
                {
                    armorByRisk[grade - 1].Add(equipment as ArmorModel);
                }
            }
            // Find the highest grade of equipment the player owns
            int highestGradeWeapon = 4;
            while (weaponsByRisk[highestGradeWeapon].Count == 0 && highestGradeWeapon >= 0) highestGradeWeapon--;
            if (highestGradeWeapon < 0)
            {
                Debug.Log("LivestreamIntegration: No weapons?");
                return;
            }
            int highestGradeArmor = 4;
            while (armorByRisk[highestGradeArmor].Count == 0 && highestGradeArmor >= 0) highestGradeArmor--;
            if (highestGradeArmor < 0)
            {
                Debug.Log("LivestreamIntegration: No armor?");
                return;
            }
            foreach (var a in workers)
            {
                WeaponModel weaponToCopy = weaponsByRisk[highestGradeWeapon][UnityEngine.Random.Range(0, weaponsByRisk[highestGradeWeapon].Count)];
                ArmorModel armorToCopy = armorByRisk[highestGradeArmor][UnityEngine.Random.Range(0, armorByRisk[highestGradeArmor].Count)];
                WeaponModel newWeapon = WeaponModel.MakeWeapon(weaponToCopy.metaInfo);
                ArmorModel newArmor = ArmorModel.MakeArmor(armorToCopy.metaInfo);
                a.SetWeapon(newWeapon);
                a.SetArmor(newArmor);
            }
        }

        void IObserver.OnNotice(string notice, params object[] param)
        {
            if (notice == NoticeName.OnStageEnd)
            {
                for (int i = 0; i < workers.Length; i++)
                {
                    AgentManager.instance.RemoveAgent(workers[i].instanceId);
                    workers[i] = null;
                }
            }
        }
    }
}
