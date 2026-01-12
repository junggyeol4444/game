using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Systems
{
    public class GachaSystem : MonoBehaviour
    {
        private static GachaSystem instance;
        public static GachaSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GachaSystem>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("GachaSystem");
                        instance = obj.AddComponent<GachaSystem>();
                    }
                }
                return instance;
            }
        }

        private List<GachaPoolData> gachaPools;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGachaPools();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeGachaPools()
        {
            gachaPools = DataManager.Instance.GetAllGachaPools();
        }

        public GachaResult PerformSinglePull(string poolId, PlayerData player)
        {
            GachaPoolData pool = gachaPools.FirstOrDefault(p => p.poolId == poolId);
            if (pool == null)
                return null;

            // Check if player has enough currency
            if (player.resources.gems < pool.costGems && player.resources.tickets < pool.costTickets)
                return null;

            // Deduct currency
            if (player.resources.tickets > 0)
                player.resources.tickets--;
            else
                player.resources.gems -= pool.costGems;

            // Perform pull
            GachaResult result = new GachaResult { acquiredCards = new List<CardData>() };
            CardData card = DrawCard(pool, player);
            result.acquiredCards.Add(card);

            // Update pity counter
            player.gachaData.totalPulls++;
            player.gachaData.pityCounter++;

            if (card.rarity >= CardRarity.Legendary)
            {
                player.gachaData.pityCounter = 0;
                result.isPityTrigger = false;
            }
            else if (player.gachaData.pityCounter >= pool.pityThreshold)
            {
                // Pity system - guarantee high rarity
                CardData pityCard = DrawPityCard(pool);
                result.acquiredCards.Add(pityCard);
                player.gachaData.pityCounter = 0;
                result.isPityTrigger = true;
            }

            result.newPityCounter = player.gachaData.pityCounter;
            return result;
        }

        public GachaResult PerformMultiPull(string poolId, PlayerData player, int count = 10)
        {
            GachaPoolData pool = gachaPools.FirstOrDefault(p => p.poolId == poolId);
            if (pool == null)
                return null;

            int totalCostGems = pool.costGems * count;
            if (player.resources.gems < totalCostGems)
                return null;

            player.resources.gems -= totalCostGems;

            GachaResult result = new GachaResult { acquiredCards = new List<CardData>() };
            
            for (int i = 0; i < count; i++)
            {
                CardData card = DrawCard(pool, player);
                result.acquiredCards.Add(card);
                
                player.gachaData.totalPulls++;
                player.gachaData.pityCounter++;

                if (card.rarity >= CardRarity.Legendary)
                {
                    player.gachaData.pityCounter = 0;
                }
            }

            // Guarantee at least one rare in multi-pull
            bool hasRare = result.acquiredCards.Any(c => c.rarity >= CardRarity.Rare);
            if (!hasRare)
            {
                result.acquiredCards[result.acquiredCards.Count - 1] = DrawGuaranteedRareCard(pool);
            }

            result.newPityCounter = player.gachaData.pityCounter;
            return result;
        }

        private CardData DrawCard(GachaPoolData pool, PlayerData player)
        {
            float random = Random.value;
            float cumulative = 0f;

            // Sort items by rarity (lowest to highest drop rate)
            var sortedItems = pool.items.OrderBy(i => i.dropRate).ToList();

            foreach (var item in sortedItems)
            {
                cumulative += item.dropRate;
                if (random <= cumulative)
                {
                    return CardSystem.Instance.CreateCard(item.cardId, 1);
                }
            }

            // Fallback to common card
            var commonItem = pool.items.FirstOrDefault(i => i.rarity == CardRarity.Common);
            return CardSystem.Instance.CreateCard(commonItem?.cardId ?? pool.items[0].cardId, 1);
        }

        private CardData DrawPityCard(GachaPoolData pool)
        {
            var legendaryItems = pool.items.Where(i => i.rarity >= CardRarity.Legendary).ToList();
            if (legendaryItems.Count == 0)
                legendaryItems = pool.items.Where(i => i.rarity >= CardRarity.Epic).ToList();

            if (legendaryItems.Count > 0)
            {
                int index = Random.Range(0, legendaryItems.Count);
                return CardSystem.Instance.CreateCard(legendaryItems[index].cardId, 1);
            }

            return DrawCard(pool, null);
        }

        private CardData DrawGuaranteedRareCard(GachaPoolData pool)
        {
            var rareItems = pool.items.Where(i => i.rarity >= CardRarity.Rare).ToList();
            if (rareItems.Count > 0)
            {
                int index = Random.Range(0, rareItems.Count);
                return CardSystem.Instance.CreateCard(rareItems[index].cardId, 1);
            }

            return DrawCard(pool, null);
        }

        public GachaPoolData GetPool(string poolId)
        {
            return gachaPools.FirstOrDefault(p => p.poolId == poolId);
        }

        public List<GachaPoolData> GetAllPools()
        {
            return gachaPools;
        }
    }
}
