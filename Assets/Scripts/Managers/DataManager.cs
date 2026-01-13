using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Core
{
    public class DataManager : MonoBehaviour
    {
        private static DataManager instance;
        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<DataManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("DataManager");
                        instance = obj.AddComponent<DataManager>();
                    }
                }
                return instance;
            }
        }

        private Dictionary<string, CardData> cardTemplates;
        private Dictionary<string, SkillData> skillTemplates;
        private List<QuestData> questTemplates;
        private List<GachaPoolData> gachaPoolTemplates;
        private Dictionary<string, List<EncounterData>> encounterTemplates;
        private List<EventData> eventTemplates;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                LoadAllData();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void LoadAllData()
        {
            cardTemplates = new Dictionary<string, CardData>();
            skillTemplates = new Dictionary<string, SkillData>();
            questTemplates = new List<QuestData>();
            gachaPoolTemplates = new List<GachaPoolData>();
            encounterTemplates = new Dictionary<string, List<EncounterData>>();
            eventTemplates = new List<EventData>();

            LoadCardsFromJSON();
            LoadSkillsFromJSON();
            LoadQuestsFromJSON();
            LoadGachaPoolsFromJSON();
            LoadEncountersFromJSON();
            LoadEventsFromJSON();
        }

        private void LoadCardsFromJSON()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Data", "cards.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                CardDataList cardList = JsonUtility.FromJson<CardDataList>(json);
                if (cardList != null && cardList.cards != null)
                {
                    foreach (var card in cardList.cards)
                    {
                        cardTemplates[card.id] = card;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Cards data file not found at: " + path);
            }
        }

        private void LoadSkillsFromJSON()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Data", "skills.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SkillDataList skillList = JsonUtility.FromJson<SkillDataList>(json);
                if (skillList != null && skillList.skills != null)
                {
                    foreach (var skill in skillList.skills)
                    {
                        skillTemplates[skill.id] = skill;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Skills data file not found at: " + path);
            }
        }

        private void LoadQuestsFromJSON()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Data", "quests.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                QuestDataList questList = JsonUtility.FromJson<QuestDataList>(json);
                if (questList != null && questList.quests != null)
                {
                    questTemplates = new List<QuestData>(questList.quests);
                }
            }
            else
            {
                Debug.LogWarning("Quests data file not found at: " + path);
            }
        }

        private void LoadGachaPoolsFromJSON()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Data", "gacha_pools.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                GachaPoolDataList poolList = JsonUtility.FromJson<GachaPoolDataList>(json);
                if (poolList != null && poolList.pools != null)
                {
                    gachaPoolTemplates = new List<GachaPoolData>(poolList.pools);
                }
            }
            else
            {
                Debug.LogWarning("Gacha pools data file not found at: " + path);
            }
        }

        private void LoadEncountersFromJSON()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Data", "encounters.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                EncounterDataList encounterList = JsonUtility.FromJson<EncounterDataList>(json);
                if (encounterList != null && encounterList.encounters != null)
                {
                    foreach (var encounter in encounterList.encounters)
                    {
                        string areaId = encounter.encounterId.Split('_')[0] + "_" + encounter.encounterId.Split('_')[1];
                        if (!encounterTemplates.ContainsKey(areaId))
                        {
                            encounterTemplates[areaId] = new List<EncounterData>();
                        }
                        encounterTemplates[areaId].Add(encounter);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Encounters data file not found at: " + path);
            }
        }

        private void LoadEventsFromJSON()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Data", "events.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                EventDataList eventList = JsonUtility.FromJson<EventDataList>(json);
                if (eventList != null && eventList.events != null)
                {
                    eventTemplates = new List<EventData>(eventList.events);
                }
            }
            else
            {
                Debug.LogWarning("Events data file not found at: " + path);
            }
        }

        public CardData GetCardTemplate(string cardId)
        {
            if (cardTemplates.ContainsKey(cardId))
            {
                // Return a copy
                string json = JsonUtility.ToJson(cardTemplates[cardId]);
                return JsonUtility.FromJson<CardData>(json);
            }
            return null;
        }

        public SkillData GetSkillTemplate(string skillId)
        {
            if (skillTemplates.ContainsKey(skillId))
            {
                string json = JsonUtility.ToJson(skillTemplates[skillId]);
                return JsonUtility.FromJson<SkillData>(json);
            }
            return null;
        }

        public List<QuestData> GetAllQuests()
        {
            return new List<QuestData>(questTemplates);
        }

        public List<GachaPoolData> GetAllGachaPools()
        {
            return new List<GachaPoolData>(gachaPoolTemplates);
        }

        public Dictionary<string, List<EncounterData>> GetAllEncounters()
        {
            return new Dictionary<string, List<EncounterData>>(encounterTemplates);
        }

        public List<EventData> GetAllEvents()
        {
            return new List<EventData>(eventTemplates);
        }
    }

    [Serializable]
    public class CardDataList
    {
        public CardData[] cards;
    }

    [Serializable]
    public class SkillDataList
    {
        public SkillData[] skills;
    }

    [Serializable]
    public class QuestDataList
    {
        public QuestData[] quests;
    }

    [Serializable]
    public class GachaPoolDataList
    {
        public GachaPoolData[] pools;
    }

    [Serializable]
    public class EncounterDataList
    {
        public EncounterData[] encounters;
    }

    [Serializable]
    public class EventDataList
    {
        public EventData[] events;
    }
}
