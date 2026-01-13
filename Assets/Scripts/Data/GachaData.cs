using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardStrategyRPG.Data
{
    [Serializable]
    public class GachaPoolData
    {
        public string poolId;
        public string poolName;
        public GachaPoolType poolType;
        public List<GachaPoolItem> items;
        public int costGems;
        public int costTickets;
        public bool hasGuaranteedRarity;
        public CardRarity guaranteedRarity;
        public int pityThreshold;
    }

    [Serializable]
    public class GachaPoolItem
    {
        public string cardId;
        public CardRarity rarity;
        public float dropRate;
        public bool isRateUp;
    }

    public enum GachaPoolType
    {
        Standard,
        Premium,
        Event,
        Friendship
    }

    [Serializable]
    public class GachaResult
    {
        public List<CardData> acquiredCards;
        public bool isPityTrigger;
        public int newPityCounter;
    }
}
