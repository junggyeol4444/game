using System;
using UnityEngine;

namespace CardStrategyRPG.Data
{
    [Serializable]
    public class BattleData
    {
        public string battleId;
        public BattleType battleType;
        public GridData gridData;
        public BattleTeam playerTeam;
        public BattleTeam enemyTeam;
        public int currentTurn;
        public BattlePhase phase;
        public string activeUnitId;
    }

    [Serializable]
    public class GridData
    {
        public int width = 7;
        public int height = 5;
        public GridCell[,] cells;
        
        public GridData()
        {
            cells = new GridCell[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[y, x] = new GridCell { x = x, y = y };
                }
            }
        }
    }

    [Serializable]
    public class GridCell
    {
        public int x;
        public int y;
        public TerrainType terrain;
        public string occupiedByUnitId;
        public bool isWalkable = true;
    }

    [Serializable]
    public class BattleTeam
    {
        public string teamId;
        public BattleUnit[] units;
        public bool isPlayerTeam;
    }

    [Serializable]
    public class BattleUnit
    {
        public string unitId;
        public CardData cardData;
        public Vector2Int position;
        public int currentHealth;
        public int currentMana;
        public bool hasMoved;
        public bool hasActed;
        public BattleEffect[] activeEffects;
    }

    [Serializable]
    public class BattleEffect
    {
        public EffectType type;
        public int value;
        public int remainingTurns;
    }

    public enum BattleType
    {
        Story,
        RandomEncounter,
        Boss,
        PvP,
        Training,
        Event
    }

    public enum BattlePhase
    {
        Setup,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }

    public enum TerrainType
    {
        Normal,
        Forest,
        Water,
        Mountain,
        Fire,
        Ice
    }

    public enum BattleAction
    {
        Move,
        Attack,
        UseSkill,
        Defend,
        Wait,
        EndTurn
    }
}
