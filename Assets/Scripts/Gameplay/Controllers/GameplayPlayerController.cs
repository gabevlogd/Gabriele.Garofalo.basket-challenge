using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class GameplayPlayerController : PlayerController
    {
        protected override void Awake()
        {
            base.Awake();
            PauseMenu.OnPauseMenuOpened += DisableTouchEvents;
            PauseMenu.OnPauseMenuClosed += EnableTouchEvents;
        }
    }
}
