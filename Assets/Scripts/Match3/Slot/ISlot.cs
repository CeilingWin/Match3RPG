using core;

namespace Unit
{
    public interface ISlot
    {
        public void SetState(IStateSlot state);
        public IStateSlot GetState();

        public bool CanSetItem();
        public bool CanPutMachine();
        public IItem GetItem();

        public void SetItem(IItem item);
    }
}