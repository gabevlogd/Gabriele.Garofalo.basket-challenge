using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class MenuGameMode : GameModeBase
    {
        public override bool Init(GameModeClass gameModeClass)
        {
            Debug.Log("MenuGameMode initializing...");
            if (!base.Init(gameModeClass)) return false;
            return true;
        }
    }
}