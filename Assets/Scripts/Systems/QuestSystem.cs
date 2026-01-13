using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Systems
{
    public class QuestSystem : MonoBehaviour
    {
        private static QuestSystem instance;
        public static QuestSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<QuestSystem>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("QuestSystem");
                        instance = obj.AddComponent<QuestSystem>();
                    }
                }
                return instance;
            }
        }

        private List<QuestData> allQuests;
        private List<QuestData> activeQuests;
        private List<QuestData> completedQuests;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeQuests();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeQuests()
        {
            allQuests = DataManager.Instance.GetAllQuests();
            activeQuests = new List<QuestData>();
            completedQuests = new List<QuestData>();
        }

        public bool StartQuest(string questId, PlayerData player)
        {
            QuestData quest = allQuests.FirstOrDefault(q => q.id == questId);
            if (quest == null || quest.status != QuestStatus.Available)
                return false;

            // Check prerequisites
            if (quest.prerequisites != null && quest.prerequisites.Length > 0)
            {
                foreach (string prereqId in quest.prerequisites)
                {
                    if (!player.progress.completedQuestIds.Contains(prereqId))
                        return false;
                }
            }

            // Check level requirement
            if (player.level < quest.requiredLevel)
                return false;

            quest.status = QuestStatus.Active;
            activeQuests.Add(quest);
            player.progress.activeQuestIds.Add(questId);
            
            return true;
        }

        public void UpdateQuestProgress(string questId, ObjectiveType objectiveType, string targetId, int amount = 1)
        {
            QuestData quest = activeQuests.FirstOrDefault(q => q.id == questId);
            if (quest == null)
                return;

            foreach (var objective in quest.objectives)
            {
                if (objective.type == objectiveType && objective.targetId == targetId)
                {
                    objective.currentAmount += amount;
                    if (objective.currentAmount >= objective.requiredAmount)
                    {
                        objective.currentAmount = objective.requiredAmount;
                        objective.isCompleted = true;
                    }
                }
            }

            CheckQuestCompletion(quest);
        }

        private void CheckQuestCompletion(QuestData quest)
        {
            bool allCompleted = quest.objectives.All(o => o.isCompleted);
            if (allCompleted)
            {
                CompleteQuest(quest);
            }
        }

        public void CompleteQuest(QuestData quest)
        {
            if (quest.status != QuestStatus.Active)
                return;

            quest.status = QuestStatus.Completed;
            activeQuests.Remove(quest);
            completedQuests.Add(quest);

            // Award rewards
            PlayerData player = GameManager.Instance.GetPlayerData();
            AwardQuestRewards(quest, player);
        }

        private void AwardQuestRewards(QuestData quest, PlayerData player)
        {
            if (quest.rewards == null)
                return;

            player.resources.gold += quest.rewards.gold;
            player.resources.gems += quest.rewards.gems;
            player.experience += quest.rewards.experience;

            if (quest.rewards.cardIds != null)
            {
                foreach (string cardId in quest.rewards.cardIds)
                {
                    CardData card = CardSystem.Instance.CreateCard(cardId, 1);
                    player.ownedCards.Add(card);
                }
            }
        }

        public List<QuestData> GetAvailableQuests(PlayerData player)
        {
            return allQuests.Where(q => 
                q.status == QuestStatus.Available && 
                player.level >= q.requiredLevel &&
                !player.progress.activeQuestIds.Contains(q.id) &&
                !player.progress.completedQuestIds.Contains(q.id)
            ).ToList();
        }

        public List<QuestData> GetActiveQuests()
        {
            return activeQuests;
        }

        public List<QuestData> GetCompletedQuests()
        {
            return completedQuests;
        }

        public void RefreshDailyQuests()
        {
            var dailyQuests = allQuests.Where(q => q.type == QuestType.Daily).ToList();
            foreach (var quest in dailyQuests)
            {
                if (quest.status == QuestStatus.Active || quest.status == QuestStatus.Completed)
                {
                    // Reset daily quests
                    quest.status = QuestStatus.Available;
                    foreach (var objective in quest.objectives)
                    {
                        objective.currentAmount = 0;
                        objective.isCompleted = false;
                    }
                    activeQuests.Remove(quest);
                    completedQuests.Remove(quest);
                }
            }
        }

        public void RefreshWeeklyQuests()
        {
            var weeklyQuests = allQuests.Where(q => q.type == QuestType.Weekly).ToList();
            foreach (var quest in weeklyQuests)
            {
                if (quest.status == QuestStatus.Active || quest.status == QuestStatus.Completed)
                {
                    quest.status = QuestStatus.Available;
                    foreach (var objective in quest.objectives)
                    {
                        objective.currentAmount = 0;
                        objective.isCompleted = false;
                    }
                    activeQuests.Remove(quest);
                    completedQuests.Remove(quest);
                }
            }
        }
    }
}
