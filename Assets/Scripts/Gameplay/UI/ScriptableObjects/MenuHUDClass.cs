using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MenuHUD", menuName = "ScriptableObjects/HUDs/MenuHUD", order = 0)]
    public class MenuHUDClass : HUDClass
    {
        public override HUD CreateHUD()
        {
            return CreateHUD<MenuHUD>();
        }
    }
}