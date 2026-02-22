using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [System.Serializable]
    public struct ThrowPowerRanges
    {
        [Range(0f, 0.5f)] public float perfectHalfWidth;
        [Range(0f, 0.5f)] public float rimExtraWidth;

        [Range(0f, 0.5f)] public float backboardOffset;
        [Range(0f, 0.5f)] public float backboardWidth;
    }
    
    public readonly struct ThrowPowerRangesRuntime
    {
        public readonly float PerfectMin;
        public readonly float PerfectMax;
        public readonly float BackboardMin;
        public readonly float BackboardMax;
        public readonly float RimMin;
        public readonly float RimMax;

        public ThrowPowerRangesRuntime(float pMin, float pMax, float bMin, float bMax, float rMin, float rMax)
        {
            PerfectMin = pMin; PerfectMax = pMax;
            BackboardMin = bMin; BackboardMax = bMax;
            RimMin = rMin; RimMax = rMax;
        }
    }
    
    public enum ThrowOutcome
    {
        None,
        Perfect,
        NearRim,
        FarRim,
        BackboardMake,
        BackboardMiss,
        ShortMiss,
        LongMiss
    }
    
    /// <summary>
    /// This class processes the throw power input and calculates the outcome of the throw based on predefined ranges for perfect throws, rim hits, backboard hits, and misses.
    /// </summary>
    [System.Serializable]
    public class ThrowPowerProcessor
    {
        [SerializeField] private PowerRangesContainer rangesContainer;
        
        public ThrowPowerRangesRuntime CalculateRanges(float perfectPower)
        {
            if (!rangesContainer)
            {
                Debug.LogError("ThrowPowerProcessor requires a reference to a PowerRangesContainer ScriptableObject.");
                return new ThrowPowerRangesRuntime(0, 0, 0, 0, 0, 0);
            }
            
            perfectPower = Mathf.Clamp01(perfectPower);
            
            ThrowPowerRanges ranges = rangesContainer.ranges;
            
            float pMin = perfectPower - ranges.perfectHalfWidth;
            float pMax = perfectPower + ranges.perfectHalfWidth;

            float rimMin = perfectPower - (ranges.perfectHalfWidth + ranges.rimExtraWidth);
            float rimMax = perfectPower + (ranges.perfectHalfWidth + ranges.rimExtraWidth);

            float bMin = rimMax + ranges.backboardOffset;
            float bMax = bMin + ranges.backboardWidth;

            // clamp in [0,1]
            pMin = Mathf.Clamp01(pMin); pMax = Mathf.Clamp01(pMax);
            rimMin = Mathf.Clamp01(rimMin); rimMax = Mathf.Clamp01(rimMax);
            bMin = Mathf.Clamp01(bMin); bMax = Mathf.Clamp01(bMax);

            return new ThrowPowerRangesRuntime(pMin, pMax, bMin, bMax, rimMin, rimMax);
        }
        
        public ThrowOutcome EvaluateThrowOutcome(float throwPower, float perfectPower)
        {
            throwPower   = Mathf.Clamp01(throwPower);
            
            ThrowPowerRangesRuntime currentRanges = CalculateRanges(perfectPower);
            float pMin = currentRanges.PerfectMin; float pMax = currentRanges.PerfectMax;
            float bMin = currentRanges.BackboardMin; float bMax = currentRanges.BackboardMax;
            float rimMin = currentRanges.RimMin; float rimMax = currentRanges.RimMax;

            // Evaluation 
            bool inBackboard = throwPower >= bMin && throwPower <= bMax;
            if (inBackboard)
                return ThrowOutcome.BackboardMake;

            bool inPerfect = throwPower >= pMin && throwPower <= pMax;
            if (inPerfect)
                return ThrowOutcome.Perfect;

            bool inNearRim = throwPower >= rimMin && throwPower < pMin;
            if (inNearRim)
                return ThrowOutcome.NearRim;
            
            bool isFarRim = throwPower > pMax && throwPower <= rimMax;
            if (isFarRim)                
                return ThrowOutcome.FarRim;
            
            bool wasBackboardIntent = throwPower < bMin && throwPower > rimMax;
            if (wasBackboardIntent)
                return ThrowOutcome.BackboardMiss;

            if (throwPower < rimMin)
                return ThrowOutcome.ShortMiss;

            return ThrowOutcome.LongMiss;
        }
    }
}
