using System.Collections;
using System.Collections.Generic;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class BasketBackboard : Singleton<BasketBackboard>
    {
        [SerializeField] private List<Transform> backboardShotPositions;
        
        [SerializeField] private Transform perfectShotPosition;
        
        public static Vector3 GetPerfectShotPosition()
        {
            if (Instance.perfectShotPosition == null)
            {
                Debug.LogWarning("No perfect shot position assigned in BackboardThrowHandler.");
                return Vector3.zero;
            }
            return Instance.perfectShotPosition.position;
        }

        public static Transform GetRandomBackboardThrowPosition()
        {
            if (Instance.backboardShotPositions == null || Instance.backboardShotPositions.Count == 0)
            {
                Debug.LogWarning("No backboard throw positions assigned in BackboardThrowHandler.");
                return null;
            }

            int randomIndex = Random.Range(0, Instance.backboardShotPositions.Count);
            return Instance.backboardShotPositions[randomIndex];
        }
        
        public static Transform GetNearestBackboardThrowPosition(Vector3 targetPosition)
        {
            if (Instance.backboardShotPositions == null || Instance.backboardShotPositions.Count == 0)
            {
                Debug.LogWarning("No backboard throw positions assigned in BackboardThrowHandler.");
                return null;
            }

            Transform nearestPosition = null;
            float nearestDistance = float.MaxValue;

            foreach (Transform backboardPosition in Instance.backboardShotPositions)
            {
                float distance = Vector3.Distance(backboardPosition.position, targetPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPosition = backboardPosition;
                }
            }

            return nearestPosition;
        }
    }
}