using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.UI
{
    [CreateAssetMenu(fileName = "RewardHUD", menuName = "HUDs/RewardHUD", order = 0)]
    public class RewardHUDClass : HUDClass
    {
        public override HUD CreateHUD()
        {
            return CreateHUD<RewardHUD>();
        }
    }
}