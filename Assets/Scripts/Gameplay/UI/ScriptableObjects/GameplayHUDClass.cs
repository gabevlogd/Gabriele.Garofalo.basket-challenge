using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayHUD", menuName = "HUDs/GameplayHUD", order = 0)]
    public class GameplayHUDClass : HUDClass
    {
        public override HUD CreateHUD()
        {
            return CreateHUD<GameplayHUD>();
        }
    }
}