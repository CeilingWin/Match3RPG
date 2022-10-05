using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3;
using Rpg.Units;
using UnityEngine;
using Enum;
using Utils;
using Material = UnityEngine.Material;

namespace Rpg
{
    public class RpgModule
    {
        private readonly List<Machine> listMachines;
        private readonly List<Monster> listMonsters;
        private readonly List<KeyValuePair<int, List<MonsterType>>> listMonsterToGenerate;
        private readonly YourBase yourBase;
        private readonly List<GameObject> listMoveArea;

        public RpgModule()
        {
            listMachines = new List<Machine>();
            listMonsters = new List<Monster>();
            listMoveArea = new List<GameObject>();
            listMonsterToGenerate = new List<KeyValuePair<int, List<MonsterType>>>();
            yourBase = Game.instance.YourBase.GetComponent<YourBase>();
            yourBase.SetMaxHp(12);
        }

        public async UniTask Init(CancellationToken cancellationToken)
        {
            // load config
            await UniTask.CompletedTask;
        }

        public async UniTask SpawnMachine(GridPosition triggerPosition, Enum.Material material, List<GridPosition> gridPositions,
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
                case Enum.Material.Machinery:
                    machineName = "Brawler";
                    break;
                default:
                    machineName = "Wall";
                    break;
            }

            List<GridPosition> listPositionToGenMachine;
            if (material == Enum.Material.Biology)
            {
                listPositionToGenMachine = gridPositions;
            }
            else listPositionToGenMachine = new List<GridPosition>() {triggerPosition};

            foreach (var gridPosition in listPositionToGenMachine)
            {
                if  (!CanSpawnMachine(gridPosition, material)) continue;
                var machineObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Units/" + machineName));
                var machine = machineObject.GetComponent<Machine>();
                machine.SetGridPosition(gridPosition);
                listMachines.Add(machine);
            }
            Debug.Log("spawned new unit");
        }

        public bool CanSpawnMachine(GridPosition gridPosition, Enum.Material material)
        {
            return CanPutUnit(gridPosition);
        }

        public bool CanPutUnit(GridPosition gridPosition)
        {
            return GetMachine(gridPosition) == null && GetMonster(gridPosition) == null;
        }

        public async UniTask GenerateMonster(int turn, CancellationToken cancellationToken)
        {
            var currentWave = Game.instance.GetState().GetWave();
            Debug.Log("GenerateMonster wave " + currentWave);
            var monstersToGen = listMonsterToGenerate.Find(data => data.Key == turn);
            if (monstersToGen.Value == null) return;
            var boardSize = Game.instance.Match3Module.GetBoardSize();
            var listJobs = new List<UniTask>();
            foreach (var monsterType in monstersToGen.Value)
            {
                int minRow = monsterType == MonsterType.Wasp ? boardSize.RowIndex - 4 : boardSize.RowIndex - 1;
                int numLooped = 0;
                const int maxLoop = 100;
                while (numLooped < maxLoop)
                {
                    var gridPosition = new GridPosition(Random.Range(minRow, boardSize.RowIndex),
                        Random.Range(0, boardSize.ColumnIndex));
                    if (Game.instance.Match3Module.CanPutUnit(gridPosition) && CanPutUnit(gridPosition))
                    {
                        listJobs.Add(SpawnMonster(gridPosition, monsterType));
                        break;
                    }

                    numLooped++;
                }
            }

            listMonsterToGenerate.Remove(monstersToGen);
            await UniTask.WhenAll(listJobs);
        }

        private async UniTask SpawnMonster(GridPosition gridPosition, MonsterType type)
        {
            Debug.Log("spawn" + type);
            string monsterName;
            switch (type)
            {
                case MonsterType.Bulldog:
                    monsterName = "Bulldog";
                    break;
                case MonsterType.Shark:
                    monsterName = "Shark";
                    break;
                case MonsterType.Wasp:
                    monsterName = "Wasp";
                    break;
                default:
                    monsterName = "Octopus";
                    break;
            }

            var machineObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Units/Monsters/" + monsterName));
            var monster = machineObject.GetComponent<Monster>();
            monster.SetGridPosition(gridPosition);
            listMonsters.Add(monster);
            await UniTask.CompletedTask;
        }

        public async UniTask LetMachinesAttack(CancellationToken cancellationToken)
        {
            Debug.Log("LetMachinesAttack");
            // check count down
            List<UniTask> jobs = new List<UniTask>();

            listMachines.Sort();
            var machines = listMachines.ToArray();
            foreach (var machine in machines)
            {
                await machine.Attack();
            }

            // update after attack
            foreach (var machine in machines)
            {
                jobs.Add(machine.UpdateUnit());
            }

            await UniTask.WhenAll(jobs);
        }

        public async UniTask LetMonstersAttack(CancellationToken cancellationToken)
        {
            Debug.Log("LetMonstersAttack");
            List<UniTask> jobs = new List<UniTask>();
            listMonsters.Sort(new MonsterMoveOrder());
            var monsters = listMonsters.ToArray();
            
            foreach (var monster in monsters)
            {
                await monster.Attack();
                if (yourBase.IsDied())
                {
                    return;
                }
            }

            // update after attack
            foreach (var monster in monsters)
            {
                jobs.Add(monster.UpdateUnit());
            }

            await UniTask.WhenAll(jobs);
        }

        public YourBase GetYourBase()
        {
            return yourBase;
        }

        public Machine GetMachine(GridPosition gridPosition)
        {
            return listMachines.Find(unit => unit.GetGridPosition() == gridPosition);
        }

        public Monster GetMonster(GridPosition gridPosition)
        {
            return listMonsters.Find(monster => monster.GetGridPosition() == gridPosition);
        }

        public Units.Unit GetUnit<T>(GridPosition gridPosition) where T : Units.Unit
        {
            var machine = GetMachine(gridPosition);
            var monster = GetMonster(gridPosition);
            if (typeof(T) == typeof(Machine))
            {
                return machine;
            }

            if (typeof(T) == typeof(Monster))
            {
                return monster;
            }

            if (machine) return machine;
            return monster;
        }

        public void OnMonsterDied(Monster monster)
        {
            listMonsters.Remove(monster);
        }

        public void OnMachineDied(Machine machine)
        {
            listMachines.Remove(machine);
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

        public int GetNumMonster()
        {
            return listMonsters.Count;
        }

        public void InitMonstersOfWave(int wave)
        {
            // todo: load wave config
            listMonsterToGenerate.Clear();
            var numTurnGenMonster = Random.Range(1, 10);
            for (var turn = 0; turn < numTurnGenMonster; turn++)
            {
                bool needGen = Random.Range(0, 3) > 0 ? true : false;
                var numMonster = needGen ? Random.Range(1, 4) : 0;
                List<MonsterType> listMonsterType = new List<MonsterType>();
                for (var j = 0; j < numMonster; j++)
                {
                    listMonsterType.Add((MonsterType) Random.Range(0, 4));
                }

                listMonsterToGenerate.Add(new KeyValuePair<int, List<MonsterType>>(turn, listMonsterType));
            }
        }

        public bool IsGenAllMonster()
        {
            return listMonsterToGenerate.Count == 0;
        }

        /*
         * update unit effect
         */
        public UniTask UpdateAllUnits()
        {
            foreach (var machine in listMachines.ToArray())
            {
                if (machine.IsDied())
                {
                    Object.Destroy(machine);
                }
            }

            foreach (var monster in listMonsters.ToArray())
            {
                if (monster.IsDied())
                {
                    Object.Destroy(monster);
                }
            }
            return UniTask.CompletedTask;
        }

        public List<Machine> GetAllMachines()
        {
            return listMachines;
        }

        public bool CanSwapMachines(GridPosition startPos, GridPosition endPos)
        {
            var monster1 = GetMonster(startPos);
            var monster2 = GetMonster(endPos);
            return monster1 == null && monster2 == null;
        }
        public async UniTask SwapMachines(GridPosition startPos, GridPosition endPos, CancellationToken cancellationToken)
        {
            var machine1 = GetMachine(startPos);
            var machine2 = GetMachine(endPos);
            var desPos1 = Game.instance.Match3Module.IndexToWorldPosition(endPos);
            var desPos2 = Game.instance.Match3Module.IndexToWorldPosition(startPos);
            List<UniTask> jobs = new List<UniTask>();
            if (machine1)
            {
                machine1.SetGridPosition(endPos);
                jobs.Add(machine1.transform.DOMove(desPos1, 0.4f).WithCancellation(cancellationToken));
            }

            if (machine2)
            {
                machine2.SetGridPosition(startPos);
                jobs.Add(machine2.transform.DOMove(desPos2, 0.4f).WithCancellation(cancellationToken));
            }
            await UniTask.WhenAll(jobs);
        }
    }
}