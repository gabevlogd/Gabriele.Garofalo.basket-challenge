using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayGameMode : GameModeBase
    {
        public override bool Init(GameModeClass gameModeClass)
        {
            Debug.Log("GameplayGameMode initializing...");
            if (!base.Init(gameModeClass)) return false;
            return true;
        }
    }
}