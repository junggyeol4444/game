using System;
using System.Collections.Generic;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Systems
{
    public class IdleSystem : MonoBehaviour
    {
        private static IdleSystem instance;
        public static IdleSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<IdleSystem>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("IdleSystem");
                        instance = obj.AddComponent<IdleSystem>();
                    }
                }
                return instance;
            }
        }

        private const int MAX_TRAINING_SLOTS = 3;
        private const int MAX_DISPATCH_MISSIONS = 5;

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

        public bool StartTraining(CardData card, int durationMinutes, PlayerData player)
        {
            if (player.idleData.trainingSlots.Count >= MAX_TRAINING_SLOTS)
                return false;

            TrainingSlot slot = new TrainingSlot
            {
                cardId = card.id,
                startTime = DateTime.Now,
                durationMinutes = durationMinutes,
                isCompleted = false
            };

            player.idleData.trainingSlots.Add(slot);
            return true;
        }

        public CardData CompleteTraining(TrainingSlot slot, PlayerData player)
        {
            if (!slot.isCompleted)
            {
                TimeSpan elapsed = DateTime.Now - slot.startTime;
                if (elapsed.TotalMinutes < slot.durationMinutes)
                    return null;
                
                slot.isCompleted = true;
            }

            // Find the card and give experience
            CardData card = player.ownedCards.Find(c => c.id == slot.cardId);
            if (card != null)
            {
                int expGained = slot.durationMinutes * 10;
                CardSystem.Instance.AddExperience(card, expGained);
            }

            player.idleData.trainingSlots.Remove(slot);
            return card;
        }

        public bool StartDispatchMission(string missionId, List<string> cardIds, int durationMinutes, PlayerData player)
        {
            if (player.idleData.activeMissions.Count >= MAX_DISPATCH_MISSIONS)
                return false;

            if (cardIds.Count == 0)
                return false;

            DispatchMission mission = new DispatchMission
            {
                missionId = missionId,
                assignedCardIds = cardIds,
                startTime = DateTime.Now,
                durationMinutes = durationMinutes,
                isCompleted = false
            };

            player.idleData.activeMissions.Add(mission);
            return true;
        }

        public void CompleteDispatchMission(DispatchMission mission, PlayerData player)
        {
            if (!mission.isCompleted)
            {
                TimeSpan elapsed = DateTime.Now - mission.startTime;
                if (elapsed.TotalMinutes < mission.durationMinutes)
                    return;
                
                mission.isCompleted = true;
            }

            // Award rewards based on mission
            int goldReward = mission.durationMinutes * 5;
            player.resources.gold += goldReward;

            // Small chance for item drops
            if (UnityEngine.Random.value < 0.3f)
            {
                player.resources.gems += UnityEngine.Random.Range(1, 5);
            }

            player.idleData.activeMissions.Remove(mission);
        }

        public void UpdateIdleRewards(PlayerData player)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeSinceLastClaim = now - player.idleData.lastIdleRewardClaim;
            
            if (timeSinceLastClaim.TotalMinutes < 1)
                return;

            int minutesElapsed = Mathf.Min((int)timeSinceLastClaim.TotalMinutes, 480); // Max 8 hours
            
            // Calculate idle rewards
            int goldPerMinute = player.level * 2;
            int expPerMinute = player.level;
            
            player.resources.gold += goldPerMinute * minutesElapsed;
            player.experience += expPerMinute * minutesElapsed;
            
            player.idleData.lastIdleRewardClaim = now;
            player.idleData.idleTimeAccumulated += minutesElapsed;
        }

        public void UpdateTrainingProgress(PlayerData player)
        {
            List<TrainingSlot> completed = new List<TrainingSlot>();
            
            foreach (var slot in player.idleData.trainingSlots)
            {
                TimeSpan elapsed = DateTime.Now - slot.startTime;
                if (elapsed.TotalMinutes >= slot.durationMinutes && !slot.isCompleted)
                {
                    slot.isCompleted = true;
                    completed.Add(slot);
                }
            }
        }

        public void UpdateDispatchProgress(PlayerData player)
        {
            List<DispatchMission> completed = new List<DispatchMission>();
            
            foreach (var mission in player.idleData.activeMissions)
            {
                TimeSpan elapsed = DateTime.Now - mission.startTime;
                if (elapsed.TotalMinutes >= mission.durationMinutes && !mission.isCompleted)
                {
                    mission.isCompleted = true;
                    completed.Add(mission);
                }
            }
        }

        public List<TrainingSlot> GetActiveTrainingSlots(PlayerData player)
        {
            return player.idleData.trainingSlots;
        }

        public List<DispatchMission> GetActiveDispatchMissions(PlayerData player)
        {
            return player.idleData.activeMissions;
        }
    }
}
