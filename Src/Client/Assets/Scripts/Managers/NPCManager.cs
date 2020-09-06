using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class NPCManager : Singleton<NPCManager>
    {
        public delegate bool NpcActionHandler(NpcDefine npc);
        Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();

        public void RegisterNpcEvent(NpcFunction function, NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
                eventMap[function] += action;
        }

        public NpcDefine GetNpcDefine(int npcID)
        {
            NpcDefine npc = null;
            DataManager.Instance.Npcs.TryGetValue(npcID, out npc);
            return npc;
        }

        //NPC交互入口
        public bool Interactive(int npcID)
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcID))
            {
                var npc = DataManager.Instance.Npcs[npcID];
                return Interactive(npc);
            }
            return false;
        }

        //依类型分配
        public bool Interactive(NpcDefine npc)
        {
            if (DoTaskInteractive(npc))
            {
                return true;
            }
            else if (npc.Type == NpcType.Functional)
            {
                return DoFunctionInteractive(npc);
            }
            return false;
        }

        private bool DoFunctionInteractive(NpcDefine npc)
        {
            if (npc.Type != NpcType.Functional)
            {
                return false;
            }
            if (!eventMap.ContainsKey(npc.Function))
            {
                return false;
            }
            return eventMap[npc.Function].Invoke(npc);
        }

        private bool DoTaskInteractive(NpcDefine npc)
        {
            var status = QuestManager.Instance.GetQuestStatusByNpc(npc.ID);
            if (status == NpcQuestStatus.None)
                return false;

            return QuestManager.Instance.OpenNpcQuest(npc.ID);
        }
    }
}
