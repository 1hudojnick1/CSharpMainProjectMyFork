using UnityEngine;
using Model.Runtime;
using Assets.Scripts.UnitBrains.Buff;

namespace Assets.Scripts.UnitBrains.Buff
{

    public class SlowAttackBuff : BaseBuff
    {

        public float SlowAttackMultiplier = 0.5f;

        public SlowAttackBuff(float duration)
        {
            Duration = duration;
        }

        public override void ApplyBuff(Unit unit)
        {
            unit.AttackSpeed *= SlowAttackMultiplier; // Замедляем атаку
        }

        public override void RemoveBuff(Unit unit)
        {
            unit.AttackSpeed /= SlowAttackMultiplier; // Восстанавливаем скорость атаки
        }
    }
}