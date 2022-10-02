using System;
using System.Collections.Generic;
using Enum;
using Match3;
using Rpg.Ability;

namespace Utils
{
    public static class CompareUtils
    {
        public static readonly Material[] Materials = new[]
        {
            Material.Chemistry,
            Material.Energy,
            Material.Machinery,
            Material.Biology
        };
    }

    public class MonsterOrder : IComparer<Rpg.Units.Unit>
    {
        private readonly GridPosition machinePosition;
        public MonsterOrder(GridPosition machinePosition)
        {
            this.machinePosition = machinePosition;
        }
        public int Compare(Rpg.Units.Unit x, Rpg.Units.Unit y)
        {
            var dx = GridPosition.Distance(x.GetGridPosition(), machinePosition);
            var dy = GridPosition.Distance(y.GetGridPosition(), machinePosition);
            var xPos = x.GetGridPosition();
            var yPos = y.GetGridPosition();
            if (Math.Abs(dx - dy) < 0.01)
            {
                if (xPos.RowIndex == yPos.RowIndex)
                {
                    return x.GetComponent<Stat>().GetHp() - y.GetComponent<Stat>().GetHp();
                }
                return xPos.RowIndex - yPos.RowIndex;
            }
            if (dx < dy) return -1;
            return 1;
        }
    }

    public class MonsterMoveOrder : IComparer<Rpg.Units.Unit>
    {
        public int Compare(Rpg.Units.Unit x, Rpg.Units.Unit y)
        {
            var xPos = x.GetGridPosition();
            var yPos = y.GetGridPosition();
            return xPos.RowIndex - yPos.RowIndex;
        }
    }

    public class MachineOrder : IComparer<Rpg.Units.Unit>
    {
        private readonly GridPosition monsterPosition;
        public MachineOrder(GridPosition monsterPosition)
        {
            this.monsterPosition = monsterPosition;
        }
        public int Compare(Rpg.Units.Unit x, Rpg.Units.Unit y)
        {
            var dx = GridPosition.Distance(x.GetGridPosition(), monsterPosition);
            var dy = GridPosition.Distance(y.GetGridPosition(), monsterPosition);
            var xPos = x.GetGridPosition();
            var yPos = y.GetGridPosition();
            if (xPos == yPos)
            {
                return xPos.RowIndex - yPos.RowIndex;
            }
            if (dx < dy) return -1;
            return 1;
        }
    }
}