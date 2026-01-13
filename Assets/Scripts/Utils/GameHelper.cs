using UnityEngine;
using System;

namespace CardStrategyRPG.Utils
{
    public static class GameHelper
    {
        public static int CalculateExpForLevel(int level)
        {
            return GameConstants.BASE_EXP_PER_LEVEL + (level - 1) * GameConstants.EXP_INCREASE_PER_LEVEL;
        }

        public static int GetMaxLevelForRarity(Data.CardRarity rarity)
        {
            switch (rarity)
            {
                case Data.CardRarity.Common:
                    return GameConstants.CARD_MAX_LEVEL_COMMON;
                case Data.CardRarity.Uncommon:
                    return GameConstants.CARD_MAX_LEVEL_UNCOMMON;
                case Data.CardRarity.Rare:
                    return GameConstants.CARD_MAX_LEVEL_RARE;
                case Data.CardRarity.Epic:
                    return GameConstants.CARD_MAX_LEVEL_EPIC;
                case Data.CardRarity.Legendary:
                    return GameConstants.CARD_MAX_LEVEL_LEGENDARY;
                case Data.CardRarity.Mythic:
                    return GameConstants.CARD_MAX_LEVEL_MYTHIC;
                default:
                    return GameConstants.CARD_MAX_LEVEL_COMMON;
            }
        }

        public static Color GetRarityColor(Data.CardRarity rarity)
        {
            switch (rarity)
            {
                case Data.CardRarity.Common:
                    return new Color(0.7f, 0.7f, 0.7f); // Gray
                case Data.CardRarity.Uncommon:
                    return new Color(0.2f, 0.8f, 0.2f); // Green
                case Data.CardRarity.Rare:
                    return new Color(0.2f, 0.5f, 1.0f); // Blue
                case Data.CardRarity.Epic:
                    return new Color(0.6f, 0.2f, 0.8f); // Purple
                case Data.CardRarity.Legendary:
                    return new Color(1.0f, 0.6f, 0.0f); // Orange
                case Data.CardRarity.Mythic:
                    return new Color(1.0f, 0.2f, 0.2f); // Red
                default:
                    return Color.white;
            }
        }

        public static string GetRarityDisplayName(Data.CardRarity rarity)
        {
            return rarity.ToString();
        }

        public static string FormatTime(TimeSpan timeSpan)
        {
            if (timeSpan.TotalHours >= 1)
                return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}m";
            else if (timeSpan.TotalMinutes >= 1)
                return $"{(int)timeSpan.TotalMinutes}m {timeSpan.Seconds}s";
            else
                return $"{timeSpan.Seconds}s";
        }

        public static string FormatNumber(int number)
        {
            if (number >= 1000000)
                return $"{number / 1000000f:F1}M";
            else if (number >= 1000)
                return $"{number / 1000f:F1}K";
            else
                return number.ToString();
        }

        public static float CalculateDamageMultiplier(Data.CardType attackerType, Data.CardType defenderType)
        {
            // Simple type effectiveness system
            // Warrior > Tank > Ranger > Assassin > Mage > Support > Warrior
            if (attackerType == Data.CardType.Warrior && defenderType == Data.CardType.Tank)
                return 1.25f;
            if (attackerType == Data.CardType.Tank && defenderType == Data.CardType.Ranger)
                return 1.25f;
            if (attackerType == Data.CardType.Ranger && defenderType == Data.CardType.Assassin)
                return 1.25f;
            if (attackerType == Data.CardType.Assassin && defenderType == Data.CardType.Mage)
                return 1.25f;
            if (attackerType == Data.CardType.Mage && defenderType == Data.CardType.Support)
                return 1.25f;
            if (attackerType == Data.CardType.Support && defenderType == Data.CardType.Warrior)
                return 1.25f;

            return 1.0f;
        }

        public static int CalculateDistance(Vector2Int pos1, Vector2Int pos2)
        {
            return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        }

        public static bool IsPositionValid(Vector2Int position, int gridWidth, int gridHeight)
        {
            return position.x >= 0 && position.x < gridWidth && 
                   position.y >= 0 && position.y < gridHeight;
        }
    }
}
