using UnityEngine;
using Model.Runtime;
using Assets.Scripts.UnitBrains.Buff;

namespace Assets.Scripts.UnitBrains.Buff
{
    public class AttackSpeedBoostBuff : BaseBuff
    {
        public float AttackSpeedMultiplier = 1.5f;
        public AttackSpeedBoostBuff(float duration)
        {
            Duration = duration;
        }
        public override void ApplyBuff(Unit unit)
        {
            unit.AttackSpeed *= AttackSpeedMultiplier; // Ускоряем атаку
        }

        public override void RemoveBuff(Unit unit)
        {
            unit.AttackSpeed /= AttackSpeedMultiplier; // Восстанавливаем скорость атаки
        }
    }
}