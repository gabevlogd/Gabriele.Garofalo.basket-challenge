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
        public readonly float perfectMin;
        public readonly float perfectMax;
        public readonly float backboardMin;
        public readonly float backboardMax;
        public readonly float rimMin;
        public readonly float rimMax;

        public ThrowPowerRangesRuntime(float pMin, float pMax, float bMin, float bMax, float rMin, float rMax)
        {
            perfectMin = pMin; perfectMax = pMax;
            backboardMin = bMin; backboardMax = bMax;
            rimMin = rMin; rimMax = rMax;
        }
    }
    
    public enum ThrowOutcome
    {
        Perfect,
        NearRim,
        FarRim,
        BackboardMake,
        BackboardMiss,
        ShortMiss,
        LongMiss
    }
    
    [System.Serializable]
    public class ThrowPowerProcessor
    {
        [SerializeField] private PowerRangesContainer rangesContainer;
        
        public ThrowPowerRangesRuntime CalculateRanges(float perfectPower)
        {
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
            float pMin = currentRanges.perfectMin; float pMax = currentRanges.perfectMax;
            float bMin = currentRanges.backboardMin; float bMax = currentRanges.backboardMax;
            float rimMin = currentRanges.rimMin; float rimMax = currentRanges.rimMax;

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
