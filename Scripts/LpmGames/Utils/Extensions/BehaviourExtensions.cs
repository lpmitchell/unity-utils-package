using System.Collections;
using UnityEngine;

namespace LpmGames.Utils.Extensions
{
    public static class BehaviourExtensions
    {
        public static Coroutine DestroyGameObjectInSeconds(this MonoBehaviour behaviour, float seconds)
        {
            return behaviour.StartCoroutine(DestroyInSecondsCoroutine(behaviour.gameObject, seconds));
        }
        
        public static Coroutine DestroyBehaviourInSeconds(this MonoBehaviour behaviour, float seconds)
        {
            return behaviour.StartCoroutine(DestroyInSecondsCoroutine(behaviour, seconds));
        }

        public static void DestroyEditorSafe(this Object obj)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(obj);
            }
            else
            {
                Object.DestroyImmediate(obj);
            }
        }

        private static IEnumerator DestroyInSecondsCoroutine(Object gameObject, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (gameObject != null) Object.Destroy(gameObject);
        }
    }
}