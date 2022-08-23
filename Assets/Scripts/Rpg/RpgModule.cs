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
            var unit = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Units/Brawler"));
            unit.transform.position = Game.instance.Match3Module.IndexToWorldPosition(gridPosition);
            await UniTask.CompletedTask;
        }

        public bool CanSpawnMachine(GridPosition gridPosition, Material material)
        {
            return material != Material.Biology;
        }
    }
}