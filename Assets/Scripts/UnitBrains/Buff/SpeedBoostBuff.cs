using UnityEngine;
using Model.Runtime;
using Assets.Scripts.UnitBrains.Buff;

namespace Assets.Scripts.UnitBrains.Buff
{
    public class SpeedBoostBuff : BaseBuff
    {
        public float SpeedMultiplier = 1.5f;
        public SpeedBoostBuff(float duration)
        {
            Duration = duration;
        }

        public override void ApplyBuff(Unit unit)
        {
            unit.Speed *= SpeedMultiplier; // Увеличиваем скорость
        }

        public override void RemoveBuff(Unit unit)
        {
            unit.Speed /= SpeedMultiplier; // Восстанавливаем исходную скорость
        }
    }
}