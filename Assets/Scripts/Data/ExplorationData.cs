using System;
using UnityEngine;

namespace CardStrategyRPG.Data
{
    [Serializable]
    public class ExplorationData
    {
        public string currentAreaId;
        public Vector2Int playerPosition;
        public bool isInEncounter;
        public EncounterData currentEncounter;
    }

    [Serializable]
    public class EncounterData
    {
        public string encounterId;
        public EncounterType type;
        public string[] enemyIds;
        public bool isFixed;
        public EncounterReward rewards;
    }

    [Serializable]
    public class EncounterReward
    {
        public int gold;
        public int experience;
        public float cardDropChance;
        public string[] possibleCardDropIds;
    }

    public enum EncounterType
    {
        Normal,
        Elite,
        Boss,
        Event,
        Treasure
    }
}
