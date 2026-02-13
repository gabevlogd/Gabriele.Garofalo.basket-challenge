using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MenuGameMode", menuName = "GameModes/MenuGameMode", order = 0)]
    public class MenuGameModeClass : GameModeClass
    {
        public override GameModeBase CreateGameMode()
        {
            return CreateGameMode<MenuGameMode>();
        }
    }
}