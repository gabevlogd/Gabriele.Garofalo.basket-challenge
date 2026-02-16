using System;
using BasketChallenge.Core;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        public event Action<float> OnPerfectPowerUpdated;

        public void UpdatePerfectPower(Vector3 targetPosition, Vector3 startPosition)
        {
            float perfectPower = GetPerfectPowerAmount(targetPosition, startPosition);
            OnPerfectPowerUpdated?.Invoke(perfectPower);
        }

        public void Throw(Rigidbody objectToThrow, Vector3 targetPosition, float powerAmount = -1f)
        {
            if (objectToThrow == null)
            {
                Debug.LogError("Object to throw is null. Cannot perform throw.");
                return;
            }
            
            Debugger.DrawDebugSphere(targetPosition, 0.5f, Color.green, 5f);
            if (powerAmount >= 0f)
            {
                targetPosition = GetAdjustedTargetPosition(powerAmount, objectToThrow.transform.position, targetPosition);
            }
            objectToThrow.velocity = GetThrowVelocity(targetPosition, objectToThrow.transform.position);
            Debugger.DrawDebugSphere(targetPosition, 0.5f, Color.red, 5f);
        }
        
        private Vector3 GetAdjustedTargetPosition(float powerAmount, Vector3 startPosition, Vector3 targetPosition)
        {
            powerAmount = Mathf.Clamp(powerAmount, 0f, 1f);
            float perfectPowerAmount = GetPerfectPowerAmount(targetPosition, startPosition);
            ThrowOutcome outcome = powerProcessor.EvaluateThrowOutcome(powerAmount, perfectPowerAmount);
            Debug.Log(outcome);
            return targetPosition + CalculateAdjustment(outcome, startPosition, targetPosition, powerAmount, perfectPowerAmount);
        }
        
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
                    // TODO: Implement backboard make adjustment logic. This could involve calculating a new target position that simulates the ball hitting the backboard and then going into the basket, which would likely involve a small horizontal adjustment away from the backboard and a vertical adjustment to account for the bounce off the backboard.
                    break;
                case ThrowOutcome.BackboardMiss:
                    // TODO: Implement backboard miss adjustment logic. This could involve calculating a new target position that simulates the ball hitting the backboard and then missing the basket, which would likely involve a combination of horizontal and vertical adjustments based on the angle of the throw and the position of the backboard.
                    break;
                case ThrowOutcome.ShortMiss:
                    adjustment = GetMissAdjustment(startPosition, targetPosition, powerAmount, perfectPowerAmount, true);
                    break;
                case ThrowOutcome.LongMiss:
                    adjustment = GetMissAdjustment(startPosition, targetPosition, powerAmount, perfectPowerAmount, false);
                    break;
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
