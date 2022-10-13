using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Rpg.Ability;
using Rpg.Ability.Attack;
using Rpg.Ability.Detection;
using Unity.VisualScripting;
using UnityEngine;
using Material = Enum.Material;

namespace Rpg.Units.Machines
{
    public class EnergyBomb : Machine
    {
        [SerializeField] private GameObject explosionVfx;
        protected override void Start()
        {
            delayAttack = 0.5f;
            base.Start();
            material = Material.Energy;
            this.AddComponent<RangeDetection>().SetRange(3);
            GetComponent<Stat>().SetStat(1, 0, Int32.MaxValue, 2, 2);
            this.AddComponent<MultiTargetAttack>();
        }

        public override async UniTask Attack()
        {

            if (GetComponent<Stat>().GetCountDown() == 0)
            {
                var t = Instantiate(explosionVfx);
                t.transform.position = this.transform.position;
                t.transform.localScale = Vector3.one * 0.6f;
                await GetComponent<IAttack>().Attack<Monster>();
            }
        }

        public override async UniTask Die()
        {
            await base.Die();
            if (GetComponent<Stat>().GetCountDown() == 0)
            {
                await Attack();
                await Game.instance.Match3Module.RespawnItem(GetGridPosition(), Material.Energy, CancellationToken.None);
            }
        }
    }
}