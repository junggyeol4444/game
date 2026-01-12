using System.Collections.Generic;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Systems
{
    public class ExplorationSystem : MonoBehaviour
    {
        private static ExplorationSystem instance;
        public static ExplorationSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ExplorationSystem>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("ExplorationSystem");
                        instance = obj.AddComponent<ExplorationSystem>();
                    }
                }
                return instance;
            }
        }

        private ExplorationData currentExploration;
        private Dictionary<string, List<EncounterData>> areaEncounters;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeExploration();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeExploration()
        {
            areaEncounters = DataManager.Instance.GetAllEncounters();
            currentExploration = new ExplorationData
            {
                currentAreaId = "area_01",
                playerPosition = Vector2Int.zero,
                isInEncounter = false
            };
        }

        public void MovePlayer(Vector2Int direction)
        {
            if (currentExploration.isInEncounter)
                return;

            currentExploration.playerPosition += direction;
            
            // Check for encounters
            CheckForEncounter();
        }

        private void CheckForEncounter()
        {
            // Random encounter chance
            float encounterChance = 0.15f;
            
            if (Random.value < encounterChance)
            {
                TriggerRandomEncounter();
            }
        }

        private void TriggerRandomEncounter()
        {
            if (!areaEncounters.ContainsKey(currentExploration.currentAreaId))
                return;

            List<EncounterData> possibleEncounters = areaEncounters[currentExploration.currentAreaId]
                .FindAll(e => !e.isFixed);

            if (possibleEncounters.Count == 0)
                return;

            int index = Random.Range(0, possibleEncounters.Count);
            StartEncounter(possibleEncounters[index]);
        }

        public void TriggerFixedEncounter(string encounterId)
        {
            if (!areaEncounters.ContainsKey(currentExploration.currentAreaId))
                return;

            EncounterData encounter = areaEncounters[currentExploration.currentAreaId]
                .Find(e => e.encounterId == encounterId && e.isFixed);

            if (encounter != null)
            {
                StartEncounter(encounter);
            }
        }

        private void StartEncounter(EncounterData encounter)
        {
            currentExploration.isInEncounter = true;
            currentExploration.currentEncounter = encounter;

            // Initialize battle based on encounter
            BattleData battle = CreateBattleFromEncounter(encounter);
            BattleSystem.Instance.StartBattle(battle);
        }

        private BattleData CreateBattleFromEncounter(EncounterData encounter)
        {
            BattleData battle = new BattleData
            {
                battleId = System.Guid.NewGuid().ToString(),
                battleType = GetBattleTypeFromEncounter(encounter.type),
                playerTeam = CreatePlayerTeam(),
                enemyTeam = CreateEnemyTeam(encounter.enemyIds)
            };

            return battle;
        }

        private BattleType GetBattleTypeFromEncounter(EncounterType type)
        {
            switch (type)
            {
                case EncounterType.Boss: return BattleType.Boss;
                case EncounterType.Elite: return BattleType.Story;
                case EncounterType.Event: return BattleType.Event;
                default: return BattleType.RandomEncounter;
            }
        }

        private BattleTeam CreatePlayerTeam()
        {
            PlayerData player = GameManager.Instance.GetPlayerData();
            List<BattleUnit> units = new List<BattleUnit>();

            // Use first 3 cards from player's collection
            for (int i = 0; i < Mathf.Min(3, player.ownedCards.Count); i++)
            {
                BattleUnit unit = new BattleUnit
                {
                    unitId = System.Guid.NewGuid().ToString(),
                    cardData = player.ownedCards[i],
                    position = Vector2Int.zero
                };
                units.Add(unit);
            }

            return new BattleTeam
            {
                teamId = "player_team",
                units = units.ToArray(),
                isPlayerTeam = true
            };
        }

        private BattleTeam CreateEnemyTeam(string[] enemyIds)
        {
            List<BattleUnit> units = new List<BattleUnit>();

            foreach (string enemyId in enemyIds)
            {
                CardData enemyCard = CardSystem.Instance.CreateCard(enemyId, 1);
                BattleUnit unit = new BattleUnit
                {
                    unitId = System.Guid.NewGuid().ToString(),
                    cardData = enemyCard,
                    position = Vector2Int.zero
                };
                units.Add(unit);
            }

            return new BattleTeam
            {
                teamId = "enemy_team",
                units = units.ToArray(),
                isPlayerTeam = false
            };
        }

        public void EndEncounter(bool victory)
        {
            if (!currentExploration.isInEncounter)
                return;

            if (victory && currentExploration.currentEncounter.rewards != null)
            {
                AwardEncounterRewards(currentExploration.currentEncounter.rewards);
            }

            currentExploration.isInEncounter = false;
            currentExploration.currentEncounter = null;
        }

        private void AwardEncounterRewards(EncounterReward rewards)
        {
            PlayerData player = GameManager.Instance.GetPlayerData();
            player.resources.gold += rewards.gold;
            player.experience += rewards.experience;

            // Card drop chance
            if (Random.value < rewards.cardDropChance && rewards.possibleCardDropIds != null && rewards.possibleCardDropIds.Length > 0)
            {
                int index = Random.Range(0, rewards.possibleCardDropIds.Length);
                CardData droppedCard = CardSystem.Instance.CreateCard(rewards.possibleCardDropIds[index], 1);
                player.ownedCards.Add(droppedCard);
            }
        }

        public void ChangeArea(string newAreaId)
        {
            currentExploration.currentAreaId = newAreaId;
            currentExploration.playerPosition = Vector2Int.zero;
        }

        public ExplorationData GetCurrentExploration()
        {
            return currentExploration;
        }

        public bool IsInEncounter()
        {
            return currentExploration.isInEncounter;
        }
    }
}
