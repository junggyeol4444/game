using System;
using UnityEngine;

namespace CardStrategyRPG.Data
{
    [Serializable]
    public class SkillData
    {
        public string id;
        public string name;
        public string description;
        public SkillType type;
        public SkillTarget target;
        public int manaCost;
        public int cooldown;
        public int currentCooldown;
        public int range;
        public int power;
        public float accuracy;
        public SkillEffect[] effects;
    }

    [Serializable]
    public class SkillEffect
    {
        public EffectType effectType;
        public int value;
        public int duration;
        public float chance;
    }

    public enum SkillType
    {
        Damage,
        Heal,
        Buff,
        Debuff,
        Special
    }

    public enum SkillTarget
    {
        SingleEnemy,
        MultipleEnemies,
        AllEnemies,
        Self,
        SingleAlly,
        MultipleAllies,
        AllAllies,
        Area
    }

    public enum EffectType
    {
        Damage,
        Heal,
        AttackBuff,
        DefenseBuff,
        SpeedBuff,
        AttackDebuff,
        DefenseDebuff,
        SpeedDebuff,
        Stun,
        Poison,
        Burn,
        Freeze,
        Shield
    }
}
