using UnityEngine;
using Model.Runtime;
using Assets.Scripts.UnitBrains.Buff;

namespace Assets.Scripts.UnitBrains.Buff
{

    public abstract class BaseBuff : MonoBehaviour
    {
        public float Duration { get; protected set; }
        public abstract void ApplyBuff(Unit unit);
        public abstract void RemoveBuff(Unit unit);
    }
}