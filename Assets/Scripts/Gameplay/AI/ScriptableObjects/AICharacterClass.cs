using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "AICharacter", menuName = "ScriptableObjects/Controllables/AICharacter", order = 0)]
    public class AICharacterClass : ControllableClass
    {
        public override ControllableBase CreateControllable()
        {
            return CreateControllable<AICharacter>();
        }
    }
}