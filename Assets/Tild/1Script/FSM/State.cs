namespace Tild.FSM
{
    public abstract class State
    {
        protected Boss Boss;

        protected State(Boss boss)
        {
            Boss = boss;
        }

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
        
    }
}