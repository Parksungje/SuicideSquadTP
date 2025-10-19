using UnityEngine;

namespace Tild.FSM
{
    public class StateMachine
    {
        private State _currentState;

        public void Initialize(State startingState)
        {
            _currentState = startingState;
            _currentState.OnEnter();
        }

        public void ChangeState(State newState)
        {
            if (_currentState == newState) return; 
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }

        public void Update()
        {
            _currentState?.OnUpdate();
        }
    }
}