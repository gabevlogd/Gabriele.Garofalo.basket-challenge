using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Controllables/Character", order = 0)]
    public class CharacterClass : ControllableClass
    {
        public override ControllableBase CreateControllable()
        {
            return CreateControllable<Character>();
        }
    }
}