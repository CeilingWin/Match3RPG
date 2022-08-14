

using core;

namespace Unit
{
    public class Slot : ISlot
    {
        private IItem item;
        private IStateSlot state;
        public Slot(IStateSlot state)
        {
            SetState(state);
        }
        public void SetState(IStateSlot state)
        {
            this.state = state;
        }

        public IStateSlot GetState()
        {
            return state;
        }

        public IItem GetItem()
        {
            return item;
        }

        public void SetItem(IItem item)
        {
            this.item = this.item;
        }
    }
}