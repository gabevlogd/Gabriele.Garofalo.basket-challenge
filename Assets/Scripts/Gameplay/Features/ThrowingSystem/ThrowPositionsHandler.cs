using System.Collections;
using System.Collections.Generic;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ThrowPositionsHandler : Singleton<ThrowPositionsHandler>
    {
        [SerializeField] private List<Transform> backboardThrowPositions;
        
        [SerializeField] private Transform perfectThrowPosition;
        
        public static Vector3 GetPerfectThrowPosition()
        {
            if (Instance.perfectThrowPosition == null)
            {
                Debug.LogWarning("No perfect throw position assigned in BackboardThrowHandler.");
                return Vector3.zero;
            }
            return Instance.perfectThrowPosition.position;
        }

        public static Transform GetRandomBackboardThrowPosition()
        {
            if (Instance.backboardThrowPositions == null || Instance.backboardThrowPositions.Count == 0)
            {
                Debug.LogWarning("No backboard throw positions assigned in BackboardThrowHandler.");
                return null;
            }

            int randomIndex = Random.Range(0, Instance.backboardThrowPositions.Count);
            return Instance.backboardThrowPositions[randomIndex];
        }
        
        public static Transform GetNearestBackboardThrowPosition(Vector3 targetPosition)
        {
            if (Instance.backboardThrowPositions == null || Instance.backboardThrowPositions.Count == 0)
            {
                Debug.LogWarning("No backboard throw positions assigned in BackboardThrowHandler.");
                return null;
            }

            Transform nearestPosition = null;
            float nearestDistance = float.MaxValue;

            foreach (Transform backboardPosition in Instance.backboardThrowPositions)
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

        public static Transform GetFartherBackboardThrowPosition(Vector3 targetPosition)
        {
            if (Instance.backboardThrowPositions == null || Instance.backboardThrowPositions.Count == 0)
            {
                Debug.LogWarning("No backboard throw positions assigned in BackboardThrowHandler.");
                return null;
            }

            Transform fartherPosition = null;
            float fartherDistance = float.MinValue;

            foreach (Transform backboardPosition in Instance.backboardThrowPositions)
            {
                float distance = Vector3.Distance(backboardPosition.position, targetPosition);
                if (distance > fartherDistance)
                {
                    fartherDistance = distance;
                    fartherPosition = backboardPosition;
                }
            }

            return fartherPosition;
        }
    }
}