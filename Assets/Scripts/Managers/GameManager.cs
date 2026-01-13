using System;
using System.IO;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("GameManager");
                        instance = obj.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        private PlayerData currentPlayer;
        private const string SAVE_FILE_NAME = "player_save.json";

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGame();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeGame()
        {
            // Load or create player data
            if (!LoadPlayerData())
            {
                CreateNewPlayer();
            }

            // Initialize other managers
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            // Ensure all managers are initialized
            var dataManager = DataManager.Instance;
            var cardSystem = Systems.CardSystem.Instance;
            var battleSystem = Systems.BattleSystem.Instance;
            var gachaSystem = Systems.GachaSystem.Instance;
            var questSystem = Systems.QuestSystem.Instance;
            var idleSystem = Systems.IdleSystem.Instance;
            var explorationSystem = Systems.ExplorationSystem.Instance;
            var eventSystem = Systems.EventSystem.Instance;
        }

        private void CreateNewPlayer()
        {
            currentPlayer = new PlayerData
            {
                playerId = Guid.NewGuid().ToString(),
                playerName = "Player",
                level = 1,
                experience = 0,
                experienceToNextLevel = 100,
                resources = new PlayerResources
                {
                    gold = 1000,
                    gems = 100,
                    tickets = 5,
                    energy = 100,
                    maxEnergy = 100,
                    lastEnergyUpdate = DateTime.Now
                },
                progress = new PlayerProgress
                {
                    currentStage = 1,
                    highestStage = 1
                },
                gachaData = new GachaData
                {
                    totalPulls = 0,
                    pityCounter = 0,
                    lastFreeGacha = DateTime.Now,
                    freeGachaAvailable = 1
                },
                idleData = new IdleData
                {
                    lastIdleRewardClaim = DateTime.Now,
                    idleTimeAccumulated = 0
                }
            };

            // Give starter cards
            GiveStarterCards();
            SavePlayerData();
        }

        private void GiveStarterCards()
        {
            // Give 3 starter cards
            string[] starterCardIds = { "card_001", "card_002", "card_003" };
            
            foreach (string cardId in starterCardIds)
            {
                CardData card = Systems.CardSystem.Instance.CreateCard(cardId, 1);
                if (card != null)
                {
                    currentPlayer.ownedCards.Add(card);
                }
            }
        }

        public void AddExperience(int exp)
        {
            currentPlayer.experience += exp;
            
            while (currentPlayer.experience >= currentPlayer.experienceToNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentPlayer.level++;
            currentPlayer.experience -= currentPlayer.experienceToNextLevel;
            currentPlayer.experienceToNextLevel = CalculateExpForNextLevel(currentPlayer.level);
            
            // Increase max energy
            currentPlayer.resources.maxEnergy += 10;
            currentPlayer.resources.energy = currentPlayer.resources.maxEnergy;
            
            Debug.Log("Player leveled up to level " + currentPlayer.level);
        }

        private int CalculateExpForNextLevel(int level)
        {
            return 100 + (level - 1) * 50;
        }

        public void UpdateEnergy()
        {
            DateTime now = DateTime.Now;
            TimeSpan timeSinceLastUpdate = now - currentPlayer.resources.lastEnergyUpdate;
            
            int minutesElapsed = (int)timeSinceLastUpdate.TotalMinutes;
            if (minutesElapsed > 0)
            {
                int energyToAdd = minutesElapsed / 5; // 1 energy per 5 minutes
                currentPlayer.resources.energy = Mathf.Min(
                    currentPlayer.resources.energy + energyToAdd,
                    currentPlayer.resources.maxEnergy
                );
                currentPlayer.resources.lastEnergyUpdate = now;
            }
        }

        public bool SpendEnergy(int amount)
        {
            if (currentPlayer.resources.energy < amount)
                return false;
            
            currentPlayer.resources.energy -= amount;
            return true;
        }

        public bool SpendGold(int amount)
        {
            if (currentPlayer.resources.gold < amount)
                return false;
            
            currentPlayer.resources.gold -= amount;
            return true;
        }

        public bool SpendGems(int amount)
        {
            if (currentPlayer.resources.gems < amount)
                return false;
            
            currentPlayer.resources.gems -= amount;
            return true;
        }

        public void AddGold(int amount)
        {
            currentPlayer.resources.gold += amount;
        }

        public void AddGems(int amount)
        {
            currentPlayer.resources.gems += amount;
        }

        public PlayerData GetPlayerData()
        {
            return currentPlayer;
        }

        public bool SavePlayerData()
        {
            try
            {
                string json = JsonUtility.ToJson(currentPlayer, true);
                string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
                File.WriteAllText(path, json);
                Debug.Log("Game saved successfully to: " + path);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save game: " + e.Message);
                return false;
            }
        }

        public bool LoadPlayerData()
        {
            try
            {
                string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    currentPlayer = JsonUtility.FromJson<PlayerData>(json);
                    Debug.Log("Game loaded successfully from: " + path);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load game: " + e.Message);
                return false;
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SavePlayerData();
            }
            else
            {
                UpdateEnergy();
                Systems.IdleSystem.Instance.UpdateIdleRewards(currentPlayer);
            }
        }

        private void OnApplicationQuit()
        {
            SavePlayerData();
        }
    }
}
