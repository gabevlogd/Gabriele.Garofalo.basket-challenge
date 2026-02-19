using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "BackboardBonuses", menuName = "ScriptableObjects/BackboardBonuses", order = 0)]
    public class BackboardBonuses : ScriptableObject
    {
        public List<BackboardBonus> bonusList = new List<BackboardBonus>();
    }
}