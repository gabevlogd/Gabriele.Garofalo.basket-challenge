using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class MenuGameMode : GameModeBase
    {
        public override bool Init()
        {
            Debug.Log("MenuGameMode initializing...");
            if (!base.Init()) return false;
            return true;
        }
    }
}