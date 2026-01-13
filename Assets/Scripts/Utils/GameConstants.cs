using UnityEngine;

namespace CardStrategyRPG.Utils
{
    public static class GameConstants
    {
        // Game Settings
        public const int TARGET_FRAME_RATE = 60;
        public const float ENERGY_REGEN_MINUTES = 5f;
        
        // Battle Constants
        public const int BATTLE_GRID_WIDTH = 7;
        public const int BATTLE_GRID_HEIGHT = 5;
        public const int MAX_PARTY_SIZE = 3;
        
        // Gacha Constants
        public const int STANDARD_GACHA_COST_GEMS = 100;
        public const int PREMIUM_GACHA_COST_GEMS = 300;
        public const int MULTI_PULL_COUNT = 10;
        public const int PITY_THRESHOLD_STANDARD = 90;
        public const int PITY_THRESHOLD_PREMIUM = 50;
        
        // Idle System Constants
        public const int MAX_TRAINING_SLOTS = 3;
        public const int MAX_DISPATCH_MISSIONS = 5;
        public const int MAX_IDLE_HOURS = 8;
        
        // Progression Constants
        public const int BASE_EXP_PER_LEVEL = 100;
        public const int EXP_INCREASE_PER_LEVEL = 50;
        public const int BASE_ENERGY_PER_LEVEL = 10;
        
        // Card Constants
        public const int CARD_MAX_LEVEL_COMMON = 50;
        public const int CARD_MAX_LEVEL_UNCOMMON = 60;
        public const int CARD_MAX_LEVEL_RARE = 70;
        public const int CARD_MAX_LEVEL_EPIC = 80;
        public const int CARD_MAX_LEVEL_LEGENDARY = 90;
        public const int CARD_MAX_LEVEL_MYTHIC = 100;
    }
}
