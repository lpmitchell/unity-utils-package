using System;
using System.Collections;
using UnityEngine;

namespace LpmGames.Utils
{
        public static class Tween
    {
        private static readonly AnimationCurve DefaultAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        public static IEnumerator Start(float duration, Action<float> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            if (curve == null)
            {
                curve = DefaultAnimationCurve;
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
        
        private static void VerifyValidTypes(params Type[] types)
        {
            foreach (var t in types)
            {
                if (t == typeof(float)) continue;
                if (t == typeof(double)) continue;
                if (t == typeof(Color)) continue;
                if (t == typeof(Vector2)) continue;
                if (t == typeof(Vector3)) continue;
                if (t == typeof(Vector4)) continue;
                if (t == typeof(Quaternion)) continue;
                throw new ArgumentException($"Attempted to tween a type ({t.Name}) that cannot be interpolated. Valid types are float, double, Color, Vector2, Vector3, Vector4, Quaternion");
            }
        }

        private static T Interpolate<T>(object start, object end, float t)
        {
            // There's a lot of weird casting and jumping through hoops in this method. It's optimized for speed and 
            // minimal memory alloc. Most of the casts in this method don't actually happen (we know the source and
            // dest type are the same, but the compiler doesn't) so they are just in there to keep the compiler happy.
            var interpolatedType = typeof(T);
            if (interpolatedType == typeof(float))
            {
                var a = (float) start;
                var b = (float) end;
                return (T) (object) (a + (b - a) * t);
            }
            if (interpolatedType == typeof(Color))
            {
                var a = (Color) start;
                var b = (Color) end;
                a.r += (b.r - a.r) * t;
                a.g += (b.g - a.g) * t;
                a.b += (b.b - a.b) * t;
                a.a += (b.a - a.a) * t;
                return (T)(object)a;
            }

            if (interpolatedType == typeof(Vector2))
            {
                var a = (Vector2) start;
                var b = (Vector2) end;
                a.x += (b.x - a.x) * t;
                a.y += (b.y - a.y) * t;
                return (T)(object)a;
            }
            
            if (interpolatedType == typeof(Vector3))
            {
                var a = (Vector3) start;
                var b = (Vector3) end;
                a.x += (b.x - a.x) * t;
                a.y += (b.y - a.y) * t;
                a.z += (b.z - a.z) * t;
                return (T)(object)a;
            }
            if (interpolatedType == typeof(Vector4))
            {
                var a = (Vector4) start;
                var b = (Vector4) end;
                a.x += (b.x - a.x) * t;
                a.y += (b.y - a.y) * t;
                a.z += (b.z - a.z) * t;
                a.w += (b.w - a.w) * t;
                return (T)(object)a;
            }
            if (interpolatedType == typeof(Quaternion))
                return (T)(object)Quaternion.LerpUnclamped((Quaternion)start, (Quaternion)end, t);
            
            return default;
        }

        public static IEnumerator Start<T0,T1>(float duration, (T0,T1) start, (T0,T1) end, Action<T0,T1> onUpdate, Action<T0,T1> onComplete, AnimationCurve curve = null)
        {
            VerifyValidTypes(typeof(T0), typeof(T1));
            yield return Start(duration, t =>
                    onUpdate(
                        Interpolate<T0>(start.Item1, end.Item1, t), 
                        Interpolate<T1>(start.Item2, end.Item2, t)
                    )
                , () => 
                    onComplete(end.Item1, end.Item2), curve);
        }
        
        public static IEnumerator Start<T0,T1,T2>(float duration, (T0,T1,T2) start, (T0,T1,T2) end, Action<T0,T1,T2> onUpdate, Action<T0,T1,T2> onComplete, AnimationCurve curve = null)
        {
            VerifyValidTypes(typeof(T0), typeof(T1), typeof(T2));
            yield return Start(duration, t =>
                    onUpdate(
                        Interpolate<T0>(start.Item1, end.Item1, t), 
                        Interpolate<T1>(start.Item2, end.Item2, t), 
                        Interpolate<T2>(start.Item3, end.Item3, t)
                    )
                , () => 
                    onComplete(end.Item1, end.Item2, end.Item3), curve);
        }
        
        public static IEnumerator Start<T0,T1,T2,T3>(float duration, (T0,T1,T2,T3) start, (T0,T1,T2,T3) end, Action<T0,T1,T2,T3> onUpdate, Action<T0,T1,T2,T3> onComplete, AnimationCurve curve = null)
        {
            VerifyValidTypes(typeof(T0), typeof(T1), typeof(T2), typeof(T3));
            yield return Start(duration, t =>
                    onUpdate(
                        Interpolate<T0>(start.Item1, end.Item1, t), 
                        Interpolate<T1>(start.Item2, end.Item2, t), 
                        Interpolate<T2>(start.Item3, end.Item3, t), 
                        Interpolate<T3>(start.Item4, end.Item4, t)
                    )
                , () => 
                    onComplete(end.Item1, end.Item2, end.Item3, end.Item4), curve);
        }
        
        public static IEnumerator Start<T0,T1,T2,T3,T4>(float duration, (T0,T1,T2,T3,T4) start, (T0,T1,T2,T3,T4) end, Action<T0,T1,T2,T3,T4> onUpdate, Action<T0,T1,T2,T3,T4> onComplete, AnimationCurve curve = null)
        {
            VerifyValidTypes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            yield return Start(duration, t =>
                    onUpdate(
                        Interpolate<T0>(start.Item1, end.Item1, t), 
                        Interpolate<T1>(start.Item2, end.Item2, t), 
                        Interpolate<T2>(start.Item3, end.Item3, t), 
                        Interpolate<T3>(start.Item4, end.Item4, t), 
                        Interpolate<T4>(start.Item5, end.Item5, t)
                    )
                , () => 
                    onComplete(end.Item1, end.Item2, end.Item3, end.Item4, end.Item5), curve);
        }
        
        public static IEnumerator Start<T0,T1,T2,T3,T4,T5>(float duration, (T0,T1,T2,T3,T4,T5) start, (T0,T1,T2,T3,T4,T5) end, Action<T0,T1,T2,T3,T4,T5> onUpdate, Action<T0,T1,T2,T3,T4,T5> onComplete, AnimationCurve curve = null)
        {
            VerifyValidTypes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            yield return Start(duration, t =>
                    onUpdate(
                        Interpolate<T0>(start.Item1, end.Item1, t), 
                        Interpolate<T1>(start.Item2, end.Item2, t), 
                        Interpolate<T2>(start.Item3, end.Item3, t), 
                        Interpolate<T3>(start.Item4, end.Item4, t), 
                        Interpolate<T4>(start.Item5, end.Item5, t), 
                        Interpolate<T5>(start.Item6, end.Item6, t)
                    )
                , () => 
                    onComplete(end.Item1, end.Item2, end.Item3, end.Item4, end.Item5, end.Item6), curve);
        }
        
        public static IEnumerator Start<T0,T1,T2,T3,T4,T5,T6>(float duration, (T0,T1,T2,T3,T4,T5,T6) start, (T0,T1,T2,T3,T4,T5,T6) end, Action<T0,T1,T2,T3,T4,T5,T6> onUpdate, Action<T0,T1,T2,T3,T4,T5,T6> onComplete, AnimationCurve curve = null)
        {
            VerifyValidTypes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            yield return Start(duration, t =>
                    onUpdate(
                        Interpolate<T0>(start.Item1, end.Item1, t), 
                        Interpolate<T1>(start.Item2, end.Item2, t), 
                        Interpolate<T2>(start.Item3, end.Item3, t), 
                        Interpolate<T3>(start.Item4, end.Item4, t), 
                        Interpolate<T4>(start.Item5, end.Item5, t), 
                        Interpolate<T5>(start.Item6, end.Item6, t), 
                        Interpolate<T6>(start.Item7, end.Item7, t)
                    )
                , () => 
                    onComplete(end.Item1, end.Item2, end.Item3, end.Item4, end.Item5, end.Item6, end.Item7), curve);
        }
        
        public static IEnumerator Start<T0,T1,T2,T3,T4,T5,T6,T7>(float duration, (T0,T1,T2,T3,T4,T5,T6,T7) start, (T0,T1,T2,T3,T4,T5,T6,T7) end, Action<T0,T1,T2,T3,T4,T5,T6,T7> onUpdate, Action<T0,T1,T2,T3,T4,T5,T6,T7> onComplete, AnimationCurve curve = null)
        {
            VerifyValidTypes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            yield return Start(duration, t =>
                    onUpdate(
                        Interpolate<T0>(start.Item1, end.Item1, t), 
                        Interpolate<T1>(start.Item2, end.Item2, t), 
                        Interpolate<T2>(start.Item3, end.Item3, t), 
                        Interpolate<T3>(start.Item4, end.Item4, t), 
                        Interpolate<T4>(start.Item5, end.Item5, t), 
                        Interpolate<T5>(start.Item6, end.Item6, t), 
                        Interpolate<T6>(start.Item7, end.Item7, t), 
                        Interpolate<T7>(start.Item8, end.Item8, t)
                    )
                , () => 
                    onComplete( end.Item1, end.Item2, end.Item3, end.Item4, end.Item5, end.Item6, end.Item7, end.Item8), curve);
        }
        
        public static IEnumerator Start(float duration, float start, float end, Action<float> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => onUpdate(Mathf.LerpUnclamped(start, end, t)), onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Color start, Color end, Action<Color> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => onUpdate(Color.LerpUnclamped(start, end, t)), onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Vector2 start, Vector2 end, Action<Vector2> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => onUpdate(Vector2.LerpUnclamped(start, end, t)), onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Vector3 start, Vector3 end, Action<Vector3> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => onUpdate(Vector3.LerpUnclamped(start, end, t)), onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Vector4 start, Vector4 end, Action<Vector4> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => onUpdate(Vector4.LerpUnclamped(start, end, t)), onComplete, curve);
        }
        
        public static IEnumerator Start(float duration, Quaternion start, Quaternion end, Action<Quaternion> onUpdate, Action onComplete, AnimationCurve curve = null)
        {
            yield return Start(duration, t => onUpdate(Quaternion.LerpUnclamped(start, end, t)), onComplete, curve);
        }
    }
}