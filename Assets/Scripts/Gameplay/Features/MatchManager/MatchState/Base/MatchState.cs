using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public abstract class MatchState : StateBase
    {
        protected MatchManager MatchManager;
        
        protected override void OnInit(GameObject context, StateMachine stateMachine)
        {
            if (context.TryGetComponent(out MatchManager matchManager))
            {
                MatchManager = matchManager;
            }
        }
    }
}