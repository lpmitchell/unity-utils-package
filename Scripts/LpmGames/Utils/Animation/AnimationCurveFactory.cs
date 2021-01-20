using UnityEngine;

namespace LpmGames.Utils.Animation
{
    public static class AnimationCurveFactory
    {
        public static AnimationCurve EasedLinear(float timeStart, float valueStart, float timeEaseStart, float timeEnd, float valueEnd, float timeEaseEnd, float easeAmount = 2f)
        {
            if (timeEaseStart > timeEaseEnd) return AnimationCurve.Linear(timeStart, valueStart, timeEnd, valueEnd);
            
            var duration = timeEnd - timeStart;
            var fractionalEaseStart = (timeEaseStart - timeStart) / duration;
            var fractionalEaseEnd = (timeEaseEnd - timeStart) / duration;
            
            if(fractionalEaseEnd < 0.6f) return AnimationCurve.Linear(timeStart, valueStart, timeEnd, valueEnd);
            
            var easeStartValue = Mathf.Lerp(valueStart, valueEnd, fractionalEaseStart) / easeAmount;
            var easeEndValue = valueEnd - (valueEnd - Mathf.Lerp(valueStart, valueEnd, fractionalEaseEnd)) / easeAmount;
            var linearTangent = (easeEndValue - easeStartValue) / (timeEaseEnd - timeEaseStart);
            
            return new AnimationCurve(
                new Keyframe(timeStart, valueStart, 0.0f, 0.0f), 
                new Keyframe(timeEaseStart, easeStartValue, linearTangent, linearTangent), 
                new Keyframe(timeEaseEnd, easeEndValue, linearTangent, linearTangent), 
                new Keyframe(timeEnd, valueEnd, 0.0f, 0.0f)
            );
        }
        
    }
}