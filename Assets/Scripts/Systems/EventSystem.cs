using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Systems
{
    public class EventSystem : MonoBehaviour
    {
        private static EventSystem instance;
        public static EventSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<EventSystem>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("EventSystem");
                        instance = obj.AddComponent<EventSystem>();
                    }
                }
                return instance;
            }
        }

        private List<EventData> allEvents;
        private EventData currentEvent;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeEvents();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeEvents()
        {
            allEvents = DataManager.Instance.GetAllEvents();
        }

        public void TriggerEvent(string eventId)
        {
            EventData eventData = allEvents.FirstOrDefault(e => e.eventId == eventId);
            if (eventData == null)
                return;

            currentEvent = eventData;
        }

        public void MakeChoice(string choiceId, PlayerData player)
        {
            if (currentEvent == null)
                return;

            EventChoice choice = currentEvent.choices.FirstOrDefault(c => c.choiceId == choiceId);
            if (choice == null)
                return;

            // Check requirements
            if (choice.requirements != null && choice.requirements.Length > 0)
            {
                foreach (string requirement in choice.requirements)
                {
                    if (!CheckRequirement(requirement, player))
                        return;
                }
            }

            // Apply outcome
            ApplyOutcome(choice.outcome, player);

            // Check for next event
            if (!string.IsNullOrEmpty(choice.outcome.nextEventId))
            {
                TriggerEvent(choice.outcome.nextEventId);
            }
            else
            {
                currentEvent = null;
            }
        }

        private bool CheckRequirement(string requirement, PlayerData player)
        {
            // Simple requirement checking
            // Format: "level:5", "gold:100", "card:card_001"
            string[] parts = requirement.Split(':');
            if (parts.Length != 2)
                return false;

            switch (parts[0].ToLower())
            {
                case "level":
                    return player.level >= int.Parse(parts[1]);
                case "gold":
                    return player.resources.gold >= int.Parse(parts[1]);
                case "gems":
                    return player.resources.gems >= int.Parse(parts[1]);
                case "card":
                    return player.ownedCards.Any(c => c.id == parts[1]);
                default:
                    return false;
            }
        }

        private void ApplyOutcome(EventOutcome outcome, PlayerData player)
        {
            player.resources.gold += outcome.goldChange;
            
            // Apply health change to party
            if (outcome.healthChange != 0)
            {
                foreach (var card in player.ownedCards.Take(3))
                {
                    card.currentStats.health += outcome.healthChange;
                    card.currentStats.health = Mathf.Clamp(card.currentStats.health, 0, card.currentStats.maxHealth);
                }
            }

            // Add items
            if (outcome.itemsGained != null)
            {
                foreach (string itemId in outcome.itemsGained)
                {
                    // For now, just award cards
                    CardData card = CardSystem.Instance.CreateCard(itemId, 1);
                    player.ownedCards.Add(card);
                }
            }
        }

        public EventData GetCurrentEvent()
        {
            return currentEvent;
        }

        public bool IsEventActive()
        {
            return currentEvent != null;
        }

        public void TriggerRandomEvent(EventType type)
        {
            List<EventData> eventsOfType = allEvents.Where(e => e.type == type).ToList();
            if (eventsOfType.Count == 0)
                return;

            int index = Random.Range(0, eventsOfType.Count);
            TriggerEvent(eventsOfType[index].eventId);
        }
    }
}
