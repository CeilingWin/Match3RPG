namespace Unit
{
    public class AvailableSlotState : IStateSlot
    {
        public bool CanContainItem()
        {
            return true;
        }
    }
}