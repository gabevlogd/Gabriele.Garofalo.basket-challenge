using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "PlayerCharacter", menuName = "ScriptableObjects/Controllables/PlayerCharacter", order = 0)]
    public class PlayerCharacterClass : ControllableClass
    {
        public override ControllableBase CreateControllable()
        {
            return CreateControllable<PlayerCharacter>();
        }
    }
}