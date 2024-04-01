using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Model;
using Model.Runtime.Projectiles;
using UnityEngine;
using Utilities;
using static UnityEngine.GraphicsBuffer;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        private List<Vector2Int> _allCurrentTargets = new List<Vector2Int>();

        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            var temperature = GetTemperature();

            if (temperature >= overheatTemperature)
            {
                return;
            }

            for (int i = 0; i < temperature; i++)
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
            }

            IncreaseTemperature();

        }

        public override Vector2Int GetNextStep()
        {
            Vector2Int currentTarget = Vector2Int.zero;
            if (_allCurrentTargets.Count > 0)
            {
                currentTarget = _allCurrentTargets[0];
            }
            else
            {
                currentTarget = unit.Pos;
            }

            if (IsTargetInRange(currentTarget))
            {
                return unit.Pos;
            }
            else
            {
                return unit.Pos.CalcNextStepTowards(currentTarget);
            }
        }

        protected override List<Vector2Int> SelectTargets()
        {
         
            List<Vector2Int>mostDangerousTarget = new List<Vector2Int>();
            Vector2Int mostDangerousTargetPosition = Vector2Int.zero;

            float enemyWithMinDistanceToBaseValue = float.MaxValue;

            foreach (Vector2Int dangerTarget in GetAllTargets())
            {
                float enemyDistanceToBaseValue = DistanceToOwnBase(dangerTarget);

                if (enemyDistanceToBaseValue < enemyWithMinDistanceToBaseValue)
                {
                    enemyWithMinDistanceToBaseValue = enemyDistanceToBaseValue;
                    mostDangerousTargetPosition = dangerTarget;
                }
            }

            _allCurrentTargets.Clear();

            if (enemyWithMinDistanceToBaseValue < float.MaxValue)
            {
                _allCurrentTargets.Add(mostDangerousTargetPosition);

                if (IsTargetInRange(mostDangerousTargetPosition))
                {
                    mostDangerousTarget.Add(mostDangerousTargetPosition);
                }
            }
            else
            {
                if (IsPlayerUnitBrain)
                {
                    var enemyBaseTarget = runtimeModel.RoMap.Bases[IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId];
                    _allCurrentTargets.Add(enemyBaseTarget);
                }
            }
            return mostDangerousTarget;
        }



        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}