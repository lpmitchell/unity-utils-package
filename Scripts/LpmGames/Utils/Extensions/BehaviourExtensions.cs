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

        private static IEnumerator DestroyInSecondsCoroutine(Object gameObject, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (gameObject != null) Object.Destroy(gameObject);
        }
    }
}