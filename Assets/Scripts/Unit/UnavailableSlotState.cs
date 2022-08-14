namespace Unit
{
    public class UnavailableSlotState : IStateSlot
    {
        public bool CanContainItem()
        {
            return false;
        }
    }
}