using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.UI
{
    [CreateAssetMenu(fileName = "MenuHUD", menuName = "HUDs/MenuHUD", order = 0)]
    public class MenuHUDClass : HUDClass
    {
        public override HUD CreateHUD()
        {
            return CreateHUD<MenuHUD>();
        }
    }
}