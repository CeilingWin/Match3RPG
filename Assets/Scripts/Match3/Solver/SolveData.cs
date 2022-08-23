using System.Collections.Generic;
using Enum;
using Unit;

namespace Match3.Solver
{
    public class SolveData
    {
        public HashSet<ISlot> SolvedSlot { get; }
        public Dictionary<GridPosition, HashSet<ISlot>> sequences;
        public Dictionary<GridPosition, Material> materials;
        public SolveData()
        {
            // todo:
            SolvedSlot = new HashSet<ISlot>();
            sequences = new Dictionary<GridPosition, HashSet<ISlot>>();
            materials = new Dictionary<GridPosition, Material>();
        }

        public void AddSlot(GridPosition gridPosition, ISlot slot)
        {
            if (!sequences.ContainsKey(gridPosition))
            {
                sequences.Add(gridPosition, new HashSet<ISlot>());
                materials.Add(gridPosition, slot.GetItem().GetMaterial());
            }

            sequences[gridPosition].Add(slot);
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

        public Material GetSequenceMaterial(GridPosition gridPosition)
        {
            return materials[gridPosition];
        }
    }
}