using System;
using System.Collections;
using UnityEngine;

namespace LpmGames.Utils
{
    public static class Tween
    {
        public static IEnumerator Start(float duration, Action<float> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            if (curve == null)
            {
                curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            }

            var start = Time.time;
            var end = start + duration;
            while (Time.time < end)
            {
                var progress = (Time.time - start) / duration;
                if (progress > 1f) progress = 1f;
                onUpdate(curve.Evaluate(progress));
                yield return null;
            }
            onUpdate(1f);
            onComplete();
        }
        
        public static IEnumerator Start(float duration, float start, float end, Action<float> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => { onUpdate(Mathf.LerpUnclamped(start, end, t)); }, onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Color start, Color end, Action<Color> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => { onUpdate(Color.LerpUnclamped(start, end, t)); }, onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Vector2 start, Vector2 end, Action<Vector2> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => { onUpdate(Vector2.LerpUnclamped(start, end, t)); }, onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Vector3 start, Vector3 end, Action<Vector3> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => { onUpdate(Vector3.LerpUnclamped(start, end, t)); }, onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Vector4 start, Vector4 end, Action<Vector4> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => { onUpdate(Vector4.LerpUnclamped(start, end, t)); }, onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Quaternion start, Quaternion end, Action<Quaternion> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => { onUpdate(Quaternion.LerpUnclamped(start, end, t)); }, onComplete, curve);
        }
    }
}