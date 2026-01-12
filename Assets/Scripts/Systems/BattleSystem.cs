using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardStrategyRPG.Data;

namespace CardStrategyRPG.Systems
{
    public class BattleSystem : MonoBehaviour
    {
        private static BattleSystem instance;
        public static BattleSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<BattleSystem>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("BattleSystem");
                        instance = obj.AddComponent<BattleSystem>();
                    }
                }
                return instance;
            }
        }

        private BattleData currentBattle;
        private List<BattleUnit> turnOrder;

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

        public void StartBattle(BattleData battleData)
        {
            currentBattle = battleData;
            currentBattle.gridData = new GridData();
            currentBattle.currentTurn = 0;
            currentBattle.phase = BattlePhase.Setup;
            
            InitializeUnits();
            CalculateTurnOrder();
            currentBattle.phase = BattlePhase.PlayerTurn;
        }

        private void InitializeUnits()
        {
            // Place player units on the left side of the grid
            for (int i = 0; i < currentBattle.playerTeam.units.Length; i++)
            {
                var unit = currentBattle.playerTeam.units[i];
                unit.position = new Vector2Int(0, i);
                unit.currentHealth = unit.cardData.currentStats.maxHealth;
                unit.currentMana = 100;
                unit.hasMoved = false;
                unit.hasActed = false;
                currentBattle.gridData.cells[unit.position.y, unit.position.x].occupiedByUnitId = unit.unitId;
            }

            // Place enemy units on the right side of the grid
            for (int i = 0; i < currentBattle.enemyTeam.units.Length; i++)
            {
                var unit = currentBattle.enemyTeam.units[i];
                unit.position = new Vector2Int(6, i);
                unit.currentHealth = unit.cardData.currentStats.maxHealth;
                unit.currentMana = 100;
                unit.hasMoved = false;
                unit.hasActed = false;
                currentBattle.gridData.cells[unit.position.y, unit.position.x].occupiedByUnitId = unit.unitId;
            }
        }

        private void CalculateTurnOrder()
        {
            turnOrder = new List<BattleUnit>();
            turnOrder.AddRange(currentBattle.playerTeam.units);
            turnOrder.AddRange(currentBattle.enemyTeam.units);
            
            // Sort by speed (descending)
            turnOrder = turnOrder.OrderByDescending(u => u.cardData.currentStats.speed).ToList();
        }

        public bool MoveUnit(BattleUnit unit, Vector2Int targetPosition)
        {
            if (!IsValidMove(unit, targetPosition))
                return false;

            // Clear old position
            currentBattle.gridData.cells[unit.position.y, unit.position.x].occupiedByUnitId = null;
            
            // Set new position
            unit.position = targetPosition;
            currentBattle.gridData.cells[targetPosition.y, targetPosition.x].occupiedByUnitId = unit.unitId;
            unit.hasMoved = true;
            
            return true;
        }

        private bool IsValidMove(BattleUnit unit, Vector2Int targetPosition)
        {
            if (targetPosition.x < 0 || targetPosition.x >= currentBattle.gridData.width)
                return false;
            if (targetPosition.y < 0 || targetPosition.y >= currentBattle.gridData.height)
                return false;

            var cell = currentBattle.gridData.cells[targetPosition.y, targetPosition.x];
            if (!cell.isWalkable || !string.IsNullOrEmpty(cell.occupiedByUnitId))
                return false;

            // Check movement range (simple Manhattan distance)
            int distance = Mathf.Abs(unit.position.x - targetPosition.x) + 
                          Mathf.Abs(unit.position.y - targetPosition.y);
            int maxMoveRange = Mathf.Max(1, unit.cardData.currentStats.speed / 10);
            
            return distance <= maxMoveRange;
        }

        public void Attack(BattleUnit attacker, BattleUnit target)
        {
            if (attacker.hasActed)
                return;

            int damage = CalculateDamage(attacker, target);
            target.currentHealth -= damage;
            attacker.hasActed = true;

            if (target.currentHealth <= 0)
            {
                target.currentHealth = 0;
                OnUnitDefeated(target);
            }

            CheckBattleEnd();
        }

        public void UseSkill(BattleUnit user, SkillData skill, BattleUnit[] targets)
        {
            if (user.hasActed || user.currentMana < skill.manaCost)
                return;

            user.currentMana -= skill.manaCost;
            
            foreach (var target in targets)
            {
                ApplySkillEffects(skill, user, target);
            }
            
            user.hasActed = true;
            skill.currentCooldown = skill.cooldown;
        }

        public void Defend(BattleUnit unit)
        {
            if (unit.hasActed)
                return;

            // Add defense buff
            BattleEffect defenseBoost = new BattleEffect
            {
                type = EffectType.DefenseBuff,
                value = unit.cardData.currentStats.defense / 2,
                remainingTurns = 1
            };
            
            List<BattleEffect> effects = new List<BattleEffect>(unit.activeEffects ?? new BattleEffect[0]);
            effects.Add(defenseBoost);
            unit.activeEffects = effects.ToArray();
            unit.hasActed = true;
        }

        public void EndTurn()
        {
            ProcessEndOfTurn();
            
            if (currentBattle.phase == BattlePhase.PlayerTurn)
            {
                currentBattle.phase = BattlePhase.EnemyTurn;
                ExecuteEnemyAI();
            }
            else
            {
                currentBattle.phase = BattlePhase.PlayerTurn;
                currentBattle.currentTurn++;
            }

            ResetUnitActions();
        }

        private void ExecuteEnemyAI()
        {
            foreach (var enemyUnit in currentBattle.enemyTeam.units)
            {
                if (enemyUnit.currentHealth <= 0)
                    continue;

                // Simple AI: Find closest player unit and attack
                BattleUnit closestTarget = FindClosestPlayerUnit(enemyUnit);
                if (closestTarget != null)
                {
                    // Try to move closer if not in range
                    if (!IsInAttackRange(enemyUnit, closestTarget))
                    {
                        Vector2Int moveTarget = GetCloserPosition(enemyUnit, closestTarget);
                        MoveUnit(enemyUnit, moveTarget);
                    }
                    
                    // Attack if in range
                    if (IsInAttackRange(enemyUnit, closestTarget))
                    {
                        Attack(enemyUnit, closestTarget);
                    }
                }
            }
            
            EndTurn();
        }

        private BattleUnit FindClosestPlayerUnit(BattleUnit fromUnit)
        {
            BattleUnit closest = null;
            float minDistance = float.MaxValue;
            
            foreach (var playerUnit in currentBattle.playerTeam.units)
            {
                if (playerUnit.currentHealth <= 0)
                    continue;
                    
                float distance = Vector2Int.Distance(fromUnit.position, playerUnit.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = playerUnit;
                }
            }
            
            return closest;
        }

        private bool IsInAttackRange(BattleUnit attacker, BattleUnit target)
        {
            int distance = Mathf.Abs(attacker.position.x - target.position.x) + 
                          Mathf.Abs(attacker.position.y - target.position.y);
            return distance <= 1; // Adjacent cells
        }

        private Vector2Int GetCloserPosition(BattleUnit unit, BattleUnit target)
        {
            Vector2Int current = unit.position;
            Vector2Int best = current;
            float bestDistance = Vector2Int.Distance(current, target.position);
            
            // Check all adjacent positions
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1), new Vector2Int(0, -1),
                new Vector2Int(1, 0), new Vector2Int(-1, 0)
            };
            
            foreach (var dir in directions)
            {
                Vector2Int newPos = current + dir;
                if (IsValidMove(unit, newPos))
                {
                    float distance = Vector2Int.Distance(newPos, target.position);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        best = newPos;
                    }
                }
            }
            
            return best;
        }

        private int CalculateDamage(BattleUnit attacker, BattleUnit target)
        {
            int baseDamage = attacker.cardData.currentStats.attack;
            int defense = target.cardData.currentStats.defense;
            
            // Apply active effects
            foreach (var effect in attacker.activeEffects ?? new BattleEffect[0])
            {
                if (effect.type == EffectType.AttackBuff)
                    baseDamage += effect.value;
            }
            
            foreach (var effect in target.activeEffects ?? new BattleEffect[0])
            {
                if (effect.type == EffectType.DefenseBuff)
                    defense += effect.value;
            }
            
            int damage = Mathf.Max(1, baseDamage - defense / 2);
            return damage;
        }

        private void ApplySkillEffects(SkillData skill, BattleUnit user, BattleUnit target)
        {
            foreach (var effect in skill.effects)
            {
                if (Random.value > effect.chance)
                    continue;

                switch (effect.effectType)
                {
                    case EffectType.Damage:
                        target.currentHealth -= skill.power + effect.value;
                        break;
                    case EffectType.Heal:
                        target.currentHealth = Mathf.Min(target.currentHealth + effect.value, 
                                                        target.cardData.currentStats.maxHealth);
                        break;
                    default:
                        AddEffect(target, effect);
                        break;
                }
            }
        }

        private void AddEffect(BattleUnit unit, SkillEffect skillEffect)
        {
            BattleEffect battleEffect = new BattleEffect
            {
                type = skillEffect.effectType,
                value = skillEffect.value,
                remainingTurns = skillEffect.duration
            };
            
            List<BattleEffect> effects = new List<BattleEffect>(unit.activeEffects ?? new BattleEffect[0]);
            effects.Add(battleEffect);
            unit.activeEffects = effects.ToArray();
        }

        private void ProcessEndOfTurn()
        {
            // Process all active effects
            foreach (var unit in turnOrder)
            {
                if (unit.currentHealth <= 0)
                    continue;

                if (unit.activeEffects == null)
                    continue;

                List<BattleEffect> remainingEffects = new List<BattleEffect>();
                
                foreach (var effect in unit.activeEffects)
                {
                    // Apply per-turn effects
                    switch (effect.type)
                    {
                        case EffectType.Poison:
                        case EffectType.Burn:
                            unit.currentHealth -= effect.value;
                            break;
                    }
                    
                    effect.remainingTurns--;
                    if (effect.remainingTurns > 0)
                    {
                        remainingEffects.Add(effect);
                    }
                }
                
                unit.activeEffects = remainingEffects.ToArray();
            }
        }

        private void ResetUnitActions()
        {
            foreach (var unit in turnOrder)
            {
                unit.hasMoved = false;
                unit.hasActed = false;
            }
        }

        private void OnUnitDefeated(BattleUnit unit)
        {
            currentBattle.gridData.cells[unit.position.y, unit.position.x].occupiedByUnitId = null;
        }

        private void CheckBattleEnd()
        {
            bool allPlayersDead = currentBattle.playerTeam.units.All(u => u.currentHealth <= 0);
            bool allEnemiesDead = currentBattle.enemyTeam.units.All(u => u.currentHealth <= 0);
            
            if (allPlayersDead)
            {
                currentBattle.phase = BattlePhase.Defeat;
                OnBattleEnd(false);
            }
            else if (allEnemiesDead)
            {
                currentBattle.phase = BattlePhase.Victory;
                OnBattleEnd(true);
            }
        }

        private void OnBattleEnd(bool victory)
        {
            if (victory)
            {
                // Award rewards
                Debug.Log("Battle Won!");
            }
            else
            {
                Debug.Log("Battle Lost!");
            }
        }

        public BattleData GetCurrentBattle()
        {
            return currentBattle;
        }
    }
}
