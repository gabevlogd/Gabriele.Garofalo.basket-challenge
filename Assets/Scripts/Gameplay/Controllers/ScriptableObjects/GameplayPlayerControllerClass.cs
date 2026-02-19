using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayPlayerController", menuName = "ScriptableObjects/Controllers/GameplayPlayerController", order = 0)]
    public class GameplayPlayerControllerClass : PlayerControllerClass
    {
        public override Controller CreateController()
        {
            return CreateController<GameplayPlayerController>();
        }
    }
}