using System.Collections.Generic;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ShootingPositionsHandler : Singleton<ShootingPositionsHandler>
    {
        [SerializeField]
        private List<Transform> shootingPositions = new List<Transform>();
        
        private int _lastRandomIndex = -1;
        
        public static Transform GetRandomShootingPosition()
        {
            if (Instance.shootingPositions.Count == 0)
            {
                Debug.LogWarning("No shooting positions assigned to ShootingPositionsHandler.");
                return null;
            }

            int randomIndex = Instance._lastRandomIndex;
            while (randomIndex == Instance._lastRandomIndex)
            {
                randomIndex = Random.Range(0, Instance.shootingPositions.Count);
            }
            Instance._lastRandomIndex = randomIndex;
            return Instance.shootingPositions[randomIndex];
        }
    }
}
