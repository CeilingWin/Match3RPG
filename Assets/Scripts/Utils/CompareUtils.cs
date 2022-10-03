using System;
using System.Collections.Generic;
using Enum;
using Match3;
using Rpg.Ability;
using Rpg.Units;

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
            if (Math.Abs(dx - dy) < 0.01)
            {
                return x.GetComponent<Stat>().GetHp() - y.GetComponent<Stat>().GetHp();
            }
            if (dx < dy) return -1;
            return 1;
        }
    }

    public class HunterMachineOrder : IComparer<Machine>
    {
        private readonly GridPosition monsterPosition;

        public HunterMachineOrder(GridPosition currentPosition)
        {
            monsterPosition = currentPosition;
        }
        private int GetOrderByMaterial(Material material)
        {
            switch (material)
            {
                case Material.Biology:
                    return 0;
                case Material.Energy:
                    return 1;
                case Material.Chemistry:
                    return 2;
                case Material.Machinery:
                    return 3;
            }

            return 0;
        }

        public int Compare(Machine x, Machine y)
        {
            var dx = GridPosition.Distance(x.GetGridPosition(), monsterPosition);
            var dy = GridPosition.Distance(y.GetGridPosition(), monsterPosition);
            if (Math.Abs(dx - dy) < 0.01)
            {
                var xOrder = GetOrderByMaterial(x.GetMaterial());
                var yOrder = GetOrderByMaterial(y.GetMaterial());
                if (xOrder == yOrder)
                {
                    return x.GetComponent<Stat>().GetHp() - y.GetComponent<Stat>().GetHp();
                }
                return yOrder - xOrder;
            }
            if (dx < dy) return -1;
            return 1;
        }
    }
}