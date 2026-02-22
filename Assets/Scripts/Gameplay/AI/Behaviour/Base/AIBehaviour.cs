using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class AIBehaviour : StateBase
    {
        protected AICharacter AICharacter;
        
        protected override void OnInit(GameObject context, StateMachine stateMachine)
        {
            base.OnInit(context, stateMachine);
            if (context.TryGetComponent(out AICharacter aiCharacter))
            {
                AICharacter = aiCharacter;
            }
            else
            {
                Debug.LogError("AIBehaviour requires an AICharacter component on the context GameObject.");
            }
        }
    }
}
