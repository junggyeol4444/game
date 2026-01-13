using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardStrategyRPG.Data
{
    [Serializable]
    public class QuestData
    {
        public string id;
        public string name;
        public string description;
        public QuestType type;
        public QuestStatus status;
        public List<QuestObjective> objectives;
        public QuestReward rewards;
        public string[] prerequisites;
        public int requiredLevel;
    }

    [Serializable]
    public class QuestObjective
    {
        public string id;
        public string description;
        public ObjectiveType type;
        public string targetId;
        public int requiredAmount;
        public int currentAmount;
        public bool isCompleted;
    }

    [Serializable]
    public class QuestReward
    {
        public int gold;
        public int gems;
        public int experience;
        public List<string> cardIds;
        public List<string> itemIds;
    }

    public enum QuestType
    {
        Main,
        Side,
        Daily,
        Weekly,
        Event
    }

    public enum QuestStatus
    {
        Locked,
        Available,
        Active,
        Completed,
        Failed
    }

    public enum ObjectiveType
    {
        DefeatEnemies,
        CollectItems,
        ReachLevel,
        CompleteStage,
        EvolveCard,
        WinBattles,
        UseSkill
    }
}
