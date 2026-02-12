using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayGameMode : GameModeBase
    {
        public override bool Init()
        {
            Debug.Log("GameplayGameMode initializing...");
            if (!base.Init()) return false;
            return true;
        }
    }
}