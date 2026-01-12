using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardStrategyRPG.Data
{
    [Serializable]
    public class CardData
    {
        public string id;
        public string name;
        public string description;
        public CardRarity rarity;
        public CardType type;
        public int level;
        public int maxLevel;
        public int experience;
        public int experienceToNextLevel;
        
        // Stats
        public CardStats baseStats;
        public CardStats currentStats;
        
        // Evolution
        public string[] evolutionPath;
        public int evolutionStage;
        
        // Skills
        public List<string> skillIds;
        
        // Relationships
        public Dictionary<string, int> relationships;
        
        // Breeding
        public string[] breedingCompatibility;
        
        public CardData()
        {
            skillIds = new List<string>();
            relationships = new Dictionary<string, int>();
        }
    }

    [Serializable]
    public class CardStats
    {
        public int health;
        public int maxHealth;
        public int attack;
        public int defense;
        public int speed;
        public int magic;
        public int resistance;
    }

    public enum CardRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    public enum CardType
    {
        Warrior,
        Mage,
        Ranger,
        Tank,
        Support,
        Assassin
    }
}
