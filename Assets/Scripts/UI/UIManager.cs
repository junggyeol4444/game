using UnityEngine;
using UnityEngine.UI;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<UIManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("UIManager");
                        instance = obj.AddComponent<UIManager>();
                    }
                }
                return instance;
            }
        }

        // UI Panel references (to be assigned in inspector or created at runtime)
        public GameObject mainMenuPanel;
        public GameObject battlePanel;
        public GameObject explorationPanel;
        public GameObject gachaPanel;
        public GameObject questPanel;
        public GameObject cardCollectionPanel;
        public GameObject idlePanel;
        public GameObject shopPanel;

        private GameObject currentPanel;

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

        private void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            HideAllPanels();
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
                currentPanel = mainMenuPanel;
            }
        }

        public void ShowBattle()
        {
            HideAllPanels();
            if (battlePanel != null)
            {
                battlePanel.SetActive(true);
                currentPanel = battlePanel;
            }
        }

        public void ShowExploration()
        {
            HideAllPanels();
            if (explorationPanel != null)
            {
                explorationPanel.SetActive(true);
                currentPanel = explorationPanel;
            }
        }

        public void ShowGacha()
        {
            HideAllPanels();
            if (gachaPanel != null)
            {
                gachaPanel.SetActive(true);
                currentPanel = gachaPanel;
            }
        }

        public void ShowQuests()
        {
            HideAllPanels();
            if (questPanel != null)
            {
                questPanel.SetActive(true);
                currentPanel = questPanel;
            }
        }

        public void ShowCardCollection()
        {
            HideAllPanels();
            if (cardCollectionPanel != null)
            {
                cardCollectionPanel.SetActive(true);
                currentPanel = cardCollectionPanel;
            }
        }

        public void ShowIdle()
        {
            HideAllPanels();
            if (idlePanel != null)
            {
                idlePanel.SetActive(true);
                currentPanel = idlePanel;
            }
        }

        public void ShowShop()
        {
            HideAllPanels();
            if (shopPanel != null)
            {
                shopPanel.SetActive(true);
                currentPanel = shopPanel;
            }
        }

        private void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (battlePanel != null) battlePanel.SetActive(false);
            if (explorationPanel != null) explorationPanel.SetActive(false);
            if (gachaPanel != null) gachaPanel.SetActive(false);
            if (questPanel != null) questPanel.SetActive(false);
            if (cardCollectionPanel != null) cardCollectionPanel.SetActive(false);
            if (idlePanel != null) idlePanel.SetActive(false);
            if (shopPanel != null) shopPanel.SetActive(false);
        }

        public void UpdatePlayerUI()
        {
            PlayerData player = Core.GameManager.Instance.GetPlayerData();
            // Update UI elements with player data
            // This would be implemented with actual UI elements
        }

        public void ShowNotification(string message, float duration = 2f)
        {
            Debug.Log("Notification: " + message);
            // Implement actual notification UI
        }

        public void ShowDialog(string title, string message)
        {
            Debug.Log("Dialog - " + title + ": " + message);
            // Implement actual dialog UI
        }
    }
}
