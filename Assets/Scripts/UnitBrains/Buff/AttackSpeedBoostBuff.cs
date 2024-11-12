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
            unit.AttackSpeed *= AttackSpeedMultiplier; // �������� �����
        }

        public override void RemoveBuff(Unit unit)
        {
            unit.AttackSpeed /= AttackSpeedMultiplier; // ��������������� �������� �����
        }
    }
}