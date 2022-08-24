using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3;
using UnityEngine;
using Material = Enum.Material;

namespace Rpg
{
    public class RpgModule
    {
        private List<Units.Unit> listUnits;

        public RpgModule()
        {
            listUnits = new List<Units.Unit>();
        }

        public async UniTask SpawnMachine(GridPosition gridPosition, Material material)
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
            await UniTask.CompletedTask;
        }

        public bool CanSpawnMachine(GridPosition gridPosition, Material material)
        {
            return material != Material.Biology && GetUnitAtGridPos(gridPosition) == null;
        }

        public Units.Unit GetUnitAtGridPos(GridPosition gridPosition)
        {
            return listUnits.Find(unit => unit.GetGridPosition() == gridPosition);
        }
    }
}