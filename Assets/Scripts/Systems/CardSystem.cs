using System.Collections.Generic;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Systems
{
    public class CardSystem : MonoBehaviour
    {
        private static CardSystem instance;
        public static CardSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CardSystem>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("CardSystem");
                        instance = obj.AddComponent<CardSystem>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public CardData CreateCard(string cardId, int level = 1)
        {
            CardData card = DataManager.Instance.GetCardTemplate(cardId);
            if (card != null)
            {
                card.level = level;
                CalculateStats(card);
            }
            return card;
        }

        public void LevelUpCard(CardData card)
        {
            if (card.level < card.maxLevel)
            {
                card.level++;
                card.experience = 0;
                card.experienceToNextLevel = CalculateExpRequired(card.level);
                CalculateStats(card);
            }
        }

        public void AddExperience(CardData card, int exp)
        {
            card.experience += exp;
            while (card.experience >= card.experienceToNextLevel && card.level < card.maxLevel)
            {
                LevelUpCard(card);
            }
        }

        public bool CanEvolve(CardData card)
        {
            if (card.evolutionPath == null || card.evolutionPath.Length == 0)
                return false;
            
            if (card.evolutionStage >= card.evolutionPath.Length)
                return false;
            
            return card.level >= card.maxLevel;
        }

        public CardData EvolveCard(CardData card)
        {
            if (!CanEvolve(card))
                return null;

            string nextEvolutionId = card.evolutionPath[card.evolutionStage];
            CardData evolvedCard = CreateCard(nextEvolutionId, 1);
            evolvedCard.evolutionStage = card.evolutionStage + 1;
            
            // Transfer some stats or bonuses
            evolvedCard.experience = 0;
            
            return evolvedCard;
        }

        public CardData BreedCards(CardData parent1, CardData parent2)
        {
            // Simple breeding logic - can be expanded
            if (parent1.breedingCompatibility == null || parent2.breedingCompatibility == null)
                return null;

            bool compatible = false;
            foreach (string compatibleType in parent1.breedingCompatibility)
            {
                if (compatibleType == parent2.id)
                {
                    compatible = true;
                    break;
                }
            }

            if (!compatible)
                return null;

            // Create offspring with random traits from parents
            CardData offspring = CreateCard(parent1.id, 1);
            offspring.baseStats.attack = (parent1.baseStats.attack + parent2.baseStats.attack) / 2;
            offspring.baseStats.defense = (parent1.baseStats.defense + parent2.baseStats.defense) / 2;
            offspring.baseStats.speed = (parent1.baseStats.speed + parent2.baseStats.speed) / 2;
            
            CalculateStats(offspring);
            return offspring;
        }

        public void UpdateRelationship(CardData card1, CardData card2, int amount)
        {
            if (!card1.relationships.ContainsKey(card2.id))
            {
                card1.relationships[card2.id] = 0;
            }
            card1.relationships[card2.id] += amount;
        }

        public int GetRelationshipLevel(CardData card1, CardData card2)
        {
            if (card1.relationships.ContainsKey(card2.id))
            {
                return card1.relationships[card2.id];
            }
            return 0;
        }

        private void CalculateStats(CardData card)
        {
            // Calculate stats based on level and base stats
            float levelMultiplier = 1.0f + (card.level - 1) * 0.1f;
            
            card.currentStats = new CardStats
            {
                maxHealth = Mathf.RoundToInt(card.baseStats.maxHealth * levelMultiplier),
                health = Mathf.RoundToInt(card.baseStats.health * levelMultiplier),
                attack = Mathf.RoundToInt(card.baseStats.attack * levelMultiplier),
                defense = Mathf.RoundToInt(card.baseStats.defense * levelMultiplier),
                speed = Mathf.RoundToInt(card.baseStats.speed * levelMultiplier),
                magic = Mathf.RoundToInt(card.baseStats.magic * levelMultiplier),
                resistance = Mathf.RoundToInt(card.baseStats.resistance * levelMultiplier)
            };
        }

        private int CalculateExpRequired(int level)
        {
            return level * 100 + (level - 1) * 50;
        }
    }
}
