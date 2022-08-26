using System.Collections.Generic;
using DefaultNamespace;
using Unit;

namespace Match3.Solver
{
    public class SequenceSolver : IMatch3Solver
    {
        public SolveData SolveBoard(Match3Board gameBoard, List<GridPosition> positions)
        {
            var solveData = new SolveData();
            foreach (var gridPos in positions)
            {
                if (solveData.Contain(gameBoard.GetSlot(gridPos))) continue;
                // check horizontal
                List<GridPosition> horizontalDir = new List<GridPosition>()
                {
                    GridPosition.Left,
                    GridPosition.Right
                };
                var sequenceSlots = GetSequenceByDirection(gameBoard, gridPos, horizontalDir);
                if (sequenceSlots.Count >= GameConst.MinLengthSequence)
                {
                    solveData.AddSlot(gridPos, sequenceSlots);
                }
                // check vertical
                List<GridPosition> verticalDir = new List<GridPosition>()
                {
                    GridPosition.Up,
                    GridPosition.Down
                };
                sequenceSlots = GetSequenceByDirection(gameBoard, gridPos, verticalDir);
                if (sequenceSlots.Count >= GameConst.MinLengthSequence)
                {
                    solveData.AddSlot(gridPos, sequenceSlots);
                }
            }
            
            return solveData;
        }

        private static HashSet<ISlot> GetSequenceByDirection(Match3Board gameBoard, GridPosition startPos, List<GridPosition> dirs)
        {
            var slots = new HashSet<ISlot>();
            var slotToCheck = gameBoard.GetSlot(startPos);
            foreach (var dir in dirs)
            {
                var currentPos = startPos;
                while (gameBoard.IsPointInBoard(currentPos))
                {
                    var currentSlot = gameBoard.GetSlot(currentPos);
                    if (SequenceSolver.IsMatch(slotToCheck, currentSlot)) slots.Add(currentSlot);
                    else break;
                    currentPos += dir;
                }
            }
            
            return slots;
        }

        public static bool IsMatch(ISlot slot1, ISlot slot2)
        {
            if (!slot1.GetState().CanContainItem() || !slot2.GetState().CanContainItem()) return false;
            if (slot1.GetItem() == null || slot2.GetItem() == null) return false;
            return slot1.GetItem().GetMaterial() == slot2.GetItem().GetMaterial();
        }
    }
}