using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using UnityEngine;
using Material = Enum.Material;

namespace Rpg
{
    public class RpgModule
    {
        private readonly List<Units.Unit> listUnits;
        private YourBase yourBase;

        public RpgModule()
        {
            listUnits = new List<Units.Unit>();
            yourBase = Game.instance.YourBase.GetComponent<YourBase>();
            yourBase.SetMaxHp(100);
        }

        public async UniTask Init(CancellationToken cancellationToken)
        {
            // load config
            await UniTask.CompletedTask;
        }

        public async UniTask SpawnMachine(GridPosition gridPosition, Material material, CancellationToken cancellationToken)
        {
            string machineName;
            switch (material)
            {
                case Material.Chemistry:
                    machineName = "Paralyzer";
                    break;
                case Material.Energy:
                    machineName = "EnergyBomb";
                    break;
                default:
                    machineName = "Brawler";
                    break;
            }
            var machineObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Units/" + machineName));
            var unit = machineObject.GetComponent<Units.Unit>();
            unit.SetGridPosition(gridPosition);
            listUnits.Add(unit);
            await UniTask.NextFrame(cancellationToken);
            unit.GetComponent<Attack>().DoAttack();
        }

        public bool CanSpawnMachine(GridPosition gridPosition, Material material)
        {
            return material != Material.Biology && GetUnitAtGridPos(gridPosition) == null;
        }

        public Units.Unit GetUnitAtGridPos(GridPosition gridPosition)
        {
            return listUnits.Find(unit => unit.GetGridPosition() == gridPosition);
        }

        public async UniTask GenerateMonster(CancellationToken cancellationToken)
        {
            Debug.Log("GenerateMonster");
            await UniTask.CompletedTask;
        }

        public async UniTask LetMachinesAttack(CancellationToken cancellationToken)
        {
            Debug.Log("LetMachinesAttack");
            await UniTask.CompletedTask;
        }
        public async UniTask LetMonstersAttack(CancellationToken cancellationToken)
        {
            Debug.Log("LetMonstersAttack");
            await UniTask.CompletedTask;
        }

        public YourBase GetYourBase()
        {
            return yourBase;
        }
    }
}