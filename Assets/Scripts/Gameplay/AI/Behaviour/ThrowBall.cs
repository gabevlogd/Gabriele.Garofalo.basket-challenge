using System;
using System.Collections;
using BasketChallenge.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BasketChallenge.Gameplay
{
    public class ThrowBall : AIBehaviour
    {
        private ThrowPowerProcessor _powerProcessor;
        private float _currentPerfectPower;
        
        private float _perfectThrowWeight;
        private float _backboardThrowWeight;
        private float _nearRimThrowWeight;
        private float _farRimThrowWeight;
        private float _shortMissThrowWeight;
        private float _longMissThrowWeight;
        private float _backboardMissThrowWeight;
        
        private Coroutine _throwCoroutine;

        protected override void OnInit(GameObject context, StateMachine stateMachine)
        {
            base.OnInit(context, stateMachine);
            if (AICharacter && AICharacter.TryGetComponent(out ThrowerComponent thrower))
            {
                thrower.OnPerfectPowerUpdated += perfectPower => _currentPerfectPower = perfectPower;

                _powerProcessor = thrower.PowerProcessor;

                _perfectThrowWeight = AICharacter.behaviourData.perfectThrowWeight;
                _backboardThrowWeight = AICharacter.behaviourData.backboardThrowWeight;
                _nearRimThrowWeight = AICharacter.behaviourData.nearRimThrowWeight;
                _farRimThrowWeight = AICharacter.behaviourData.farRimThrowWeight;
                _shortMissThrowWeight = AICharacter.behaviourData.shortMissThrowWeight;
                _longMissThrowWeight = AICharacter.behaviourData.longMissThrowWeight;
                _backboardMissThrowWeight = AICharacter.behaviourData.backboardMissThrowWeight;
            }
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            AICharacter.UpdatePerfectPower();
            _throwCoroutine = AICharacter.StartCoroutine(ThrowDelay());
        }

        protected override void OnExit(StateBase nextState)
        {
            base.OnExit(nextState);
            if (_throwCoroutine != null)
            {
                AICharacter.StopCoroutine(_throwCoroutine);
                _throwCoroutine = null;
            }
        }

        private IEnumerator ThrowDelay()
        {
            float delay = Random.Range(AICharacter.behaviourData.ThrowDelayMin, AICharacter.behaviourData.ThrowDelayMax);
            yield return new WaitForSeconds(delay);
            Throw();
            RelativeStateMachine.ChangeState<Wait>();
        }

        private void Throw()
        {
            ThrowPowerRangesRuntime ranges = _powerProcessor.CalculateRanges(_currentPerfectPower);

            // randomly select throw type based on weights:
            float total = _perfectThrowWeight + _backboardThrowWeight + _farRimThrowWeight + _nearRimThrowWeight + 
                          _shortMissThrowWeight + _longMissThrowWeight + _backboardMissThrowWeight;
            float randomThrowChoice = Random.value * total;

            // get power ranges from randomThrowChoice
            float min, max;
            if (randomThrowChoice < _perfectThrowWeight)
            {
                min = ranges.PerfectMin; 
                max = ranges.PerfectMax;
            }
            else if ((randomThrowChoice -= _perfectThrowWeight) < _backboardThrowWeight)
            {
                min = ranges.BackboardMin; 
                max = ranges.BackboardMax;
            }
            else if ((randomThrowChoice -= _backboardThrowWeight) < _farRimThrowWeight)
            {
                min = ranges.PerfectMax; 
                max = ranges.RimMax;
            }
            else if ((randomThrowChoice -= _farRimThrowWeight) < _nearRimThrowWeight)
            {
                min = ranges.RimMin; 
                max = ranges.PerfectMin;
            }
            else if ((randomThrowChoice -= _nearRimThrowWeight) < _shortMissThrowWeight)
            {
                min = 0; 
                max = ranges.RimMin;
            }
            else if ((randomThrowChoice -= _shortMissThrowWeight) < _longMissThrowWeight)
            {
                min = ranges.BackboardMax; 
                max = 1;
            }
            else
            {
                min = ranges.RimMax;
                max = ranges.BackboardMin;
            }

            // Generate a random power within the selected range
            float power = Random.Range(min, max);

            // Throw the ball with the calculated power
            AICharacter.ThrowBall(power);
        }
    }
}

