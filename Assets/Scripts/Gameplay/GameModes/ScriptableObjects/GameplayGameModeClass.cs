using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayGameMode", menuName = "GameModes/GameplayGameMode")]
    public class GameplayGameModeClass : GameModeClass
    {
        public override GameModeBase CreateGameMode() 
        {
            return CreateGameMode<GameplayGameMode>();
        }
    }
}