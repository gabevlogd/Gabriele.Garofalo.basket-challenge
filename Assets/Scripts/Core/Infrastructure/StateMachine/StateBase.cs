using UnityEngine;

namespace BasketChallenge.Core
{
    public abstract class StateBase
    {
        public string StateName => GetType().Name;
        
        public StateMachine RelativeStateMachine;
        
        public GameObject StateContext;
        
        protected bool IsInitialized;

        internal void Init(GameObject context, StateMachine stateMachine)
        {
            if (!context || !stateMachine)
            {
                Debug.LogError("StateBase Init failed: context or stateMachine is null.");
                IsInitialized = false;
                return;
            }
            StateContext = context;
            RelativeStateMachine = stateMachine;
            IsInitialized = true;
            OnInit(context, stateMachine);
        }

        internal void Enter()
        {
            if (!IsInitialized) return;
            OnEnter();
        }
        
        internal void Update()
        {
            if (!IsInitialized) return;
            OnUpdate();
        }
        
        internal void Exit(StateBase nextState)
        {
            if (!IsInitialized) return;
            OnExit(nextState);
        }

        protected virtual void OnInit(GameObject context, StateMachine stateMachine){}
        protected virtual void OnEnter(){}
        protected virtual void OnUpdate(){}
        protected virtual void OnExit(StateBase nextState){}
    }
}