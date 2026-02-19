using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Core
{
    public sealed class StateMachine : MonoBehaviour
    {
        public event Action<StateBase, StateBase> OnStateChanged;
        
        [SerializeField] private StateMachineData configData;
        
        private Dictionary<string, StateBase> _states;
        
        private StateBase _currentState;
        
        private StateBase _previousState;
        
        private bool _started;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (!_started) return;
            _currentState?.Update();
        }
        
        public void StartStateMachine()
        {
            if (_started)
            {
                Debug.LogWarning("StateMachine StartStateMachine: already started.");
                return;
            }
            
            _started = true;
            _currentState?.Enter();
        }
        
        public string GetCurrentStateName()
        {
            return _currentState?.StateName;
        }
        
        public string GetPreviousStateName()
        {
            return _previousState?.StateName;
        }
        
        public StateBase GetCurrentState()
        {
            return _currentState;
        }
        
        public StateBase GetPreviousState()
        {
            return _previousState;
        }

        public void ChangeState(string newStateName)
        {
            if (string.IsNullOrEmpty(newStateName))
            {
                Debug.LogError("StateMachine ChangeState failed: newStateName is null or empty.");
                return;
            }
            
            if (_states == null || !_states.TryGetValue(newStateName, out StateBase newState))
            {
                Debug.LogError($"StateMachine ChangeState failed: state '{newStateName}' not found.");
                return;
            }
            
            ChangeState(newState);
        }

        public void ChangeState<T>() where T : StateBase
        {
            if (_states == null)
            {
                Debug.LogError("StateMachine ChangeState failed: states dictionary is null.");
                return;
            }
            
            if (!_states.TryGetValue(typeof(T).Name, out StateBase newState))
            {
                Debug.LogError($"StateMachine ChangeState failed: state of type '{typeof(T).Name}' not found.");
                return;
            }
            
            ChangeState(newState);
        }

        public void ChangeState(StateBase newState)
        {
            if (!_started)
            {
                Debug.LogWarning("StateMachine ChangeState: state machine not started yet. Call StartStateMachine() first.");
                return;
            }
            
            if (newState == null)
            {
                Debug.LogError("StateMachine ChangeState failed: newState is null.");
                return;
            }
            
            if (_currentState == newState)
            {
                Debug.LogWarning($"StateMachine ChangeState: already in state '{newState.StateName}'.");
                return;
            }
            
            _previousState = _currentState;
            _currentState?.Exit(newState);
            _currentState = newState;
            _currentState.Enter();
            OnStateChanged?.Invoke(_currentState, _previousState);
        }

        private void Init()
        {
            if (configData == null)
            {
                Debug.LogError("StateMachine Init failed: configData is null.");
                return;
            }
            
            _currentState = AddNewState(configData.entryState);
            
            foreach (StateClass stateClass in configData.states)
            {
                AddNewState(stateClass);
            }
        }
        
        private StateBase AddNewState(StateClass newStateClass)
        {
            if (newStateClass == null)
            {
                Debug.LogError("StateMachine AddNewState failed: newStateClass is null.");
                return null;
            }
            
            if (_states == null)
            {
                _states = new Dictionary<string, StateBase>();
            }
            
            StateBase newState = newStateClass.CreateState();
            if (newState == null)
            {
                Debug.LogError($"StateMachine AddNewState failed: CreateState returned null for {newStateClass.name}.");
                return null;
            }
            
            if (_states.ContainsKey(newState.StateName))
            {
                Debug.LogError($"StateMachine AddNewState failed: state with name '{newState.StateName}' already exists.");
                return null;
            }
            
            newState.Init(gameObject, this);
            _states.Add(newState.StateName, newState);
            return newState;
        }
    }
}