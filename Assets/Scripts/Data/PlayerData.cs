using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardStrategyRPG.Data
{
    [Serializable]
    public class PlayerData
    {
        public string playerId;
        public string playerName;
        public int level;
        public int experience;
        public int experienceToNextLevel;
        
        // Resources
        public PlayerResources resources;
        
        // Collections
        public List<CardData> ownedCards;
        public List<string> unlockedSkills;
        public List<string> unlockedAreas;
        
        // Progress
        public PlayerProgress progress;
        
        // Gacha
        public GachaData gachaData;
        
        // Idle Systems
        public IdleData idleData;
        
        public PlayerData()
        {
            ownedCards = new List<CardData>();
            unlockedSkills = new List<string>();
            unlockedAreas = new List<string>();
        }
    }

    [Serializable]
    public class PlayerResources
    {
        public int gold;
        public int gems;
        public int tickets;
        public int energy;
        public int maxEnergy;
        public DateTime lastEnergyUpdate;
    }

    [Serializable]
    public class PlayerProgress
    {
        public int currentStage;
        public int highestStage;
        public List<string> completedQuestIds;
        public List<string> activeQuestIds;
        public Dictionary<string, int> achievements;
        
        public PlayerProgress()
        {
            completedQuestIds = new List<string>();
            activeQuestIds = new List<string>();
            achievements = new Dictionary<string, int>();
        }
    }

    [Serializable]
    public class GachaData
    {
        public int totalPulls;
        public int pityCounter;
        public DateTime lastFreeGacha;
        public int freeGachaAvailable;
    }

    [Serializable]
    public class IdleData
    {
        public List<TrainingSlot> trainingSlots;
        public List<DispatchMission> activeMissions;
        public DateTime lastIdleRewardClaim;
        public int idleTimeAccumulated;
        
        public IdleData()
        {
            trainingSlots = new List<TrainingSlot>();
            activeMissions = new List<DispatchMission>();
        }
    }

    [Serializable]
    public class TrainingSlot
    {
        public string cardId;
        public DateTime startTime;
        public int durationMinutes;
        public bool isCompleted;
    }

    [Serializable]
    public class DispatchMission
    {
        public string missionId;
        public List<string> assignedCardIds;
        public DateTime startTime;
        public int durationMinutes;
        public bool isCompleted;
    }
}
