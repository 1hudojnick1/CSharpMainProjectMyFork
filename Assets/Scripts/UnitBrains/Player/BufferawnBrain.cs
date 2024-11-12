using Model.Runtime.Projectiles;
using Model.Runtime.ReadOnly;
using System.Collections.Generic;
using System.Linq;
using UnitBrains.Player;
using UnityEngine;
using Utilities;
using View;
using Assets.Scripts.UnitBrains.Buff;

namespace Assets.Scripts.UnitBrains.Player
{
    public class BuffawnBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Buffawn";
        private float BuffDelay = 0.5f; // ????? ?? ?????????? ?????
        private float MoveDelay = 0.0f; // ????? ?? ?????????? ????????

        public BuffawnBrain()
        {
        }

        // ???? ???? ?? ????????, ??????? ????? ??????
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            return;
        }

        public override void Update(float deltaTime, float time)
        {
            BuffDelay -= deltaTime;
            MoveDelay -= deltaTime;

            if (BuffDelay > 0)
            {
                return;
            }

            IReadOnlyUnit[] allFriendlyTargets = runtimeModel.RoUnits
                .Where(u => u.Config.IsPlayerUnit == IsPlayerUnitBrain && u.Pos != unit.Pos).ToArray();

            if (allFriendlyTargets.Length > 0)
            {
                IReadOnlyUnit targetUnit = allFriendlyTargets[Random.Range(0, allFriendlyTargets.Length)];
                BuffSystem.Instance.AddBuff((Model.Runtime.Unit)targetUnit, new AttackSpeedBoostBuff(10f));
                ServiceLocator.Get<VFXView>().PlayVFX(targetUnit.Pos, VFXView.VFXType.BuffApplied);

                BuffDelay = 5.0f; // ????? ?? ?????????? ?????
                MoveDelay = 0.5f; // ???????? ????? ????????? ?????????
            }
        }

        public override Vector2Int GetNextStep()
        {
            if (MoveDelay > 0.0f)
                return unit.Pos;

            // ?????????? ??????????? ?????? ???????? ? ????
            return base.GetNextStep();
        }
    }
}