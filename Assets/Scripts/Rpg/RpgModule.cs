using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Rpg.Units;
using UnityEngine;
using Enum;
using Material = UnityEngine.Material;

namespace Rpg
{
    public class RpgModule
    {
        private readonly List<Machine> listUnits;
        private readonly YourBase yourBase;
        private List<GameObject> listMoveArea;

        public RpgModule()
        {
            listUnits = new List<Machine>();
            listMoveArea = new List<GameObject>();
            yourBase = Game.instance.YourBase.GetComponent<YourBase>();
            yourBase.SetMaxHp(100);
        }

        public async UniTask Init(CancellationToken cancellationToken)
        {
            // load config
            await UniTask.CompletedTask;
        }

        public async UniTask SpawnMachine(GridPosition gridPosition, Enum.Material material,
            CancellationToken cancellationToken)
        {
            string machineName;
            switch (material)
            {
                case Enum.Material.Chemistry:
                    machineName = "Paralyzer";
                    break;
                case Enum.Material.Energy:
                    machineName = "EnergyBomb";
                    break;
                default:
                    machineName = "Brawler";
                    break;
            }

            var machineObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Units/" + machineName));
            var machine = machineObject.GetComponent<Machine>();
            machine.SetGridPosition(gridPosition);
            listUnits.Add(machine);
            Debug.Log("spawned new unit");
            // await UniTask.NextFrame(cancellationToken);
            // unit.GetComponent<Attack>().DoAttack();
        }

        public bool CanSpawnMachine(GridPosition gridPosition, Enum.Material material)
        {
            return material != Enum.Material.Biology && GetMachine(gridPosition) == null;
        }

        public async UniTask GenerateMonster(CancellationToken cancellationToken)
        {
            var currentWave = Game.instance.GetState().GetWave();
            Debug.Log("GenerateMonster wave " + currentWave);

            await UniTask.CompletedTask;
        }

        public async UniTask LetMachinesAttack(CancellationToken cancellationToken)
        {
            Debug.Log("LetMachinesAttack");
            // todo: attack
            
            // check count down
            List<Machine> listMachineDied = new List<Machine>();
            List<UniTask> jobs = new List<UniTask>();
            listUnits.ForEach(machine =>
            {
                var stat = machine.GetComponent<Stat>();
                stat.ChangeCountDown(-1);
                if (stat.GetCountDown() == 0)
                {
                    listMachineDied.Add(machine);
                }
            });
            listMachineDied.ForEach(machine =>
            {
                jobs.Add(machine.Die());
                listUnits.Remove(machine);
            });
            await UniTask.WhenAll(jobs);
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

        public Machine GetMachine(GridPosition gridPosition)
        {
            return listUnits.Find(unit => unit.GetGridPosition() == gridPosition);
        }

        public void ShowMoveAbleArea(Machine machine)
        {
            var boardSize = Game.instance.Match3Module.GetBoardSize();
            for (var rowIndex = 0; rowIndex < boardSize.RowIndex; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < boardSize.ColumnIndex; columnIndex++)
                {
                    var tile = Object.Instantiate(Resources.Load("Prefabs/MoveArea") as GameObject);
                    var position = new GridPosition(rowIndex, columnIndex);
                    Material material;
                    if (machine.CanMoveTo(position))
                    {
                        material = Resources.Load<Material>("Materials/MoveArea");
                    }
                    else
                    {
                        material = Resources.Load<Material>("Materials/CantMoveArea");
                    }
                    tile.GetComponent<MeshRenderer>().sharedMaterial = material;
                    var worldPosition = Game.instance.Match3Module.IndexToWorldPosition(position);
                    worldPosition.y = 0.02f;
                    tile.transform.position = worldPosition;
                    listMoveArea.Add(tile);
                }
            }
        }

        public void HideMoveAbleArea()
        {
            listMoveArea.ForEach(Object.Destroy);
            listMoveArea.Clear();
        }
    }
}