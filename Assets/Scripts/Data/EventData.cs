using System;
using UnityEngine;

namespace CardStrategyRPG.Data
{
    [Serializable]
    public class EventData
    {
        public string eventId;
        public string name;
        public string description;
        public EventType type;
        public EventChoice[] choices;
        public EventReward[] rewards;
    }

    [Serializable]
    public class EventChoice
    {
        public string choiceId;
        public string text;
        public EventOutcome outcome;
        public string[] requirements;
    }

    [Serializable]
    public class EventOutcome
    {
        public string resultText;
        public int goldChange;
        public int healthChange;
        public string[] itemsGained;
        public string nextEventId;
    }

    [Serializable]
    public class EventReward
    {
        public RewardType type;
        public string itemId;
        public int amount;
    }

    public enum EventType
    {
        NPC,
        Dungeon,
        Random,
        Story
    }

    public enum RewardType
    {
        Gold,
        Gems,
        Card,
        Item,
        Experience
    }
}
