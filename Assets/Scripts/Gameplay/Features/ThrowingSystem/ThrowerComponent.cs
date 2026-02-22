using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class ThrowerComponent : MonoBehaviour
    {
        /// <summary>
        /// The step by which the initial velocity will be increased until a valid throw velocity is found.
        /// Lower values will result in more accurate throws but may take longer to compute,
        /// while higher values will compute faster but may overshoot the target.
        /// </summary>
        [SerializeField]
        private float speedStep = 1f;
        
        [SerializeField]
        private float maxHorizontalDistance = 20f;
        
        [SerializeField]
        private float minHorizontalDistance = 3f;
        
        [SerializeField]
        private ThrowPowerProcessor powerProcessor = new ThrowPowerProcessor();
        public ThrowPowerProcessor PowerProcessor => powerProcessor;
        
        private ThrowOutcome _lastThrowOutcome;
        
        public event Action<float> OnPerfectPowerUpdated;

        public void UpdatePerfectPower(Vector3 targetPosition, Vector3 startPosition)
        {
            float perfectPower = GetPerfectPowerAmount(targetPosition, startPosition);
            OnPerfectPowerUpdated?.Invoke(perfectPower);
        }

        public ThrowOutcome Throw(Rigidbody objectToThrow, Vector3 targetPosition, float powerAmount = -1f)
        {
            if (objectToThrow == null)
            {
                Debug.LogError("Object to throw is null. Cannot perform throw.");
                return ThrowOutcome.None;
            }
            
#if UNITY_EDITOR
            Debugger.DrawDebugSphere(targetPosition, 0.5f, Color.green, 5f);
#endif
            
            if (powerAmount >= 0f)
            {
                targetPosition = GetAdjustedTargetPosition(powerAmount, objectToThrow.transform.position, targetPosition);
            }
            else
            {
                // If no power amount is provided, we assume it's a perfect throw and set the last throw outcome accordingly.
                _lastThrowOutcome = ThrowOutcome.Perfect;
            }

            bool preferLowAngle = _lastThrowOutcome is ThrowOutcome.BackboardMake or ThrowOutcome.BackboardMiss;
            objectToThrow.velocity = GetThrowVelocity(targetPosition, objectToThrow.transform.position, preferLowAngle);
            
#if UNITY_EDITOR
            Debugger.DrawDebugSphere(targetPosition, 0.5f, Color.red, 5f);
#endif
            
            return _lastThrowOutcome;
        }
        
        /// <summary>
        /// Returns an adjusted target position, where adjustment means move the target position from the
        /// perfect throw position in a direction and magnitude that depends on the last throw outcome,
        /// which is evaluated from the provided power amount and the perfect power amount for the given target position.
        /// </summary>
        /// <param name="startPosition"> The position from which the object is thrown, used to calculate the direction of the adjustment.</param>
        private Vector3 GetAdjustedTargetPosition(float powerAmount, Vector3 startPosition, Vector3 targetPosition)
        {
            powerAmount = Mathf.Clamp(powerAmount, 0f, 1f);
            float perfectPowerAmount = GetPerfectPowerAmount(targetPosition, startPosition);
            _lastThrowOutcome  = powerProcessor.EvaluateThrowOutcome(powerAmount, perfectPowerAmount);
            return targetPosition + CalculateAdjustment(_lastThrowOutcome, startPosition, targetPosition, powerAmount, perfectPowerAmount);
        }
        
        /// <summary>
        /// Calculates the perfect power amount based on the horizontal distance between the start position and the target position.
        /// </summary>
        /// <returns>
        /// The power amount needed to perfectly reach the target position, normalized between 0 and 1 based
        /// on the defined minimum and maximum horizontal distances.
        /// </returns>
        private float GetPerfectPowerAmount(Vector3 targetPosition, Vector3 startPosition)
        {
            Vector3 horizontalDistance = Vector3.ProjectOnPlane(targetPosition - startPosition, Vector3.up);
            float horizontalDistanceMagnitude = Mathf.Clamp(horizontalDistance.magnitude, minHorizontalDistance, maxHorizontalDistance);
            return (horizontalDistanceMagnitude - minHorizontalDistance) / (maxHorizontalDistance - minHorizontalDistance);
        }

        private Vector3 CalculateAdjustment(ThrowOutcome outcome, Vector3 startPosition, Vector3 targetPosition, float powerAmount, float perfectPowerAmount)
        {
            Vector3 adjustment = Vector3.zero;

            switch (outcome)
            {
                case ThrowOutcome.Perfect:
                    // No adjustment needed for a perfect throw, as the target position is already optimal.
                    break;
                case ThrowOutcome.NearRim:
                    adjustment = GetRimAdjustment(startPosition, targetPosition, true);
                    break;
                case ThrowOutcome.FarRim:
                    adjustment = GetRimAdjustment(startPosition, targetPosition, false);
                    break;
                case ThrowOutcome.BackboardMake:
                    adjustment = GetBackboardMakeAdjustment(startPosition, targetPosition);
                    break;
                case ThrowOutcome.BackboardMiss:
                    adjustment = GetBackboardMissAdjustment(startPosition, targetPosition);
                    break;
                case ThrowOutcome.ShortMiss:
                    adjustment = GetMissAdjustment(startPosition, targetPosition, powerAmount, perfectPowerAmount, true);
                    break;
                case ThrowOutcome.LongMiss:
                    adjustment = GetMissAdjustment(startPosition, targetPosition, powerAmount, perfectPowerAmount, false);
                    break;
                case ThrowOutcome.None:
                    return Vector3.zero;
            }

            return adjustment;
        }

        private Vector3 GetRimAdjustment(Vector3 startPosition, Vector3 targetPosition, bool isNearRim)
        {
            Vector3 horizontalDirection = Vector3.ProjectOnPlane(targetPosition - startPosition, Vector3.up).normalized;
            float adjustmentMagnitude = isNearRim ? -0.2f : 0.2f;
            return horizontalDirection * adjustmentMagnitude;
        }

        private Vector3 GetMissAdjustment(Vector3 startPosition, Vector3 targetPosition, float powerAmount, float perfectPowerAmount, bool isShortMiss)
        {
            Vector3 horizontalDistance = Vector3.ProjectOnPlane(targetPosition - startPosition, Vector3.up);
            float lerpAlpha = isShortMiss ? (perfectPowerAmount - powerAmount) : (powerAmount - perfectPowerAmount);
            float adjustment = Mathf.Lerp(0f, 1f, lerpAlpha);
            return horizontalDistance * (adjustment * (isShortMiss ? -1f : 1f));
        }
        
        private Vector3 GetBackboardMakeAdjustment(Vector3 startPosition, Vector3 targetPosition)
        {
            Transform backboardPosition = ThrowPositionsHandler.GetNearestBackboardThrowPosition(startPosition);
            if (backboardPosition == null)
            {
                Debug.LogWarning("No backboard throw positions available. Cannot calculate backboard make adjustment.");
                return Vector3.zero;
            }
            
            return backboardPosition.position - targetPosition;
        }
        
        private Vector3 GetBackboardMissAdjustment(Vector3 startPosition, Vector3 targetPosition)
        {
            Transform backboardPosition = ThrowPositionsHandler.GetFartherBackboardThrowPosition(startPosition);
            if (backboardPosition == null)
            {
                Debug.LogWarning("No backboard throw positions available. Cannot calculate backboard miss adjustment.");
                return Vector3.zero;
            }
            
            return backboardPosition.position - targetPosition;
        }

        /// <summary>
        /// Calculates the initial velocity required to throw an object from a start position to a target position, taking into account gravity.
        /// </summary>
        /// <param name="targetPosition"> The position the object should be thrown towards.</param>
        /// <param name="startPosition"> The position from which the object is thrown.</param>
        /// <param name="preferLowAngle"> If true, the method will calculate the throw velocity using the lower angle solution of the projectile motion equations.
        /// If false, it will use the higher angle solution. This allows for more control over the trajectory of the thrown object,
        /// depending on the desired outcome (e.g., a flatter trajectory vs. a higher arc).</param>
        /// <returns> A Vector3 representing the initial velocity that should be applied to the object to throw it towards the target position.</returns>
        private Vector3 GetThrowVelocity(Vector3 targetPosition, Vector3 startPosition, bool preferLowAngle = false)
        {
            
            float v0 = 1f; // v0 = initial velocity magnitude
            float g = Mathf.Abs(Physics.gravity.y); 
            Vector3 targetDistance = targetPosition - startPosition;
            float deltaY = targetDistance.y;
            float R = Vector3.ProjectOnPlane(targetDistance, Vector3.up).magnitude;
            float v0Sq = v0 * v0;
            float insideSqrt = v0Sq * v0Sq - g * (g * R * R + 2f * deltaY * v0Sq);
        
            // insideSqrt is the discriminant of the quadratic equation derived from the projectile motion equations.
            // If insideSqrt is negative, it means that with the current initial velocity v0, the target cannot be reached.
            while (insideSqrt < 0f)
            {
                v0 += speedStep;
                v0Sq = v0 * v0;
                insideSqrt = v0Sq * v0Sq - g * (g * R * R + 2f * deltaY * v0Sq);
            }

            float sqrt = Mathf.Sqrt(insideSqrt);
            
            // Depending on the preferLowAngle flag, we choose either the low angle or the high angle solution.
            // This happens because for a given initial velocity and target position, there are generally two possible angles of launch: one low and one high.
            float numerator = preferLowAngle ? v0Sq - sqrt : v0Sq + sqrt;
            
            float denominator = g * R;
            float throwAngle  = Mathf.Atan(numerator  / denominator);
            
            Vector3 throwHorizontalDirection = new Vector3(targetDistance.x, 0f, targetDistance.z).normalized;
            Vector3 throwVelocity = throwHorizontalDirection * (v0 * Mathf.Cos(throwAngle)) + Vector3.up * (v0 * Mathf.Sin(throwAngle));
            return throwVelocity;
        }
    }
}
