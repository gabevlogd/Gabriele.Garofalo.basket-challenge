using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MenuPlayerController", menuName = "Controllers/MenuPlayerController", order = 0)]
    public class MenuPlayerControllerClass : PlayerControllerClass
    {
        public override Controller CreateController()
        {
            return CreateController<MenuPlayerController>();
        }
    }
}