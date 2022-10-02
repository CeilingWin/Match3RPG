using System.Collections.Generic;
using System.Linq;
using Enum;
using Unit;

namespace Match3.Solver
{
    public class SolveData
    {
        public HashSet<ISlot> SolvedSlot { get; }
        private readonly Dictionary<GridPosition, HashSet<ISlot>> sequences;
        private readonly Dictionary<GridPosition, HashSet<GridPosition>> sequencesPosition;
        private readonly Dictionary<GridPosition, Material> materials;
        public SolveData()
        {
            SolvedSlot = new HashSet<ISlot>();
            sequences = new Dictionary<GridPosition, HashSet<ISlot>>();
            sequencesPosition = new Dictionary<GridPosition, HashSet<GridPosition>>();
            materials = new Dictionary<GridPosition, Material>();
        }

        public void AddSlot(GridPosition gridPosition, ISlot slot)
        {
            if (!sequences.ContainsKey(gridPosition))
            {
                sequences.Add(gridPosition, new HashSet<ISlot>());
                materials.Add(gridPosition, slot.GetItem().GetMaterial());
                sequencesPosition.Add(gridPosition, new HashSet<GridPosition>());
            }

            sequences[gridPosition].Add(slot);
            sequencesPosition[gridPosition].Add(Game.instance.Match3Module.GameBoard.GetSlotPosition(slot));
            SolvedSlot.Add(slot);
        }

        public void AddSlot(GridPosition gridPosition, IEnumerable<ISlot> slots)
        {
            foreach (var slot in slots)
            {
                AddSlot(gridPosition, slot);
            }
        }

        public bool Contain(ISlot slot)
        {
            return SolvedSlot.Contains(slot);
        }

        public List<GridPosition> GetAllTriggerGridPos()
        {
            return new List<GridPosition>(sequences.Keys);
        }

        public List<GridPosition> GetSequencePosition(GridPosition triggerPosition)
        {
            return sequencesPosition[triggerPosition]?.ToList();
        }

        public Material GetSequenceMaterial(GridPosition gridPosition)
        {
            return materials[gridPosition];
        }
    }
}