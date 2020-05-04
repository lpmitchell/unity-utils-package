using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace LpmGames.Utils.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Swap the array elements at the two specified indices
        /// </summary>
        /// <param name="array">The array to modify</param>
        /// <param name="index1">First index</param>
        /// <param name="index2">Second index</param>
        /// <typeparam name="T">Array type</typeparam>
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            if (index1 == index2) return;
            var tmp = array[index1];
            array[index1] = array[index2];
            array[index2] = tmp;
        }
        
        /// <summary>
        /// Swap the list elements at the two specified indices
        /// </summary>
        /// <param name="list">The list to modify</param>
        /// <param name="index1">First index</param>
        /// <param name="index2">Second index</param>
        /// <typeparam name="T">List type</typeparam>
        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            var tmp = list[index1];
            list[index1] = list[index2];
            list[index2] = tmp;
        }

        /// <summary>
        /// Set all of the GameObjects in this enumerable as active/inactive
        /// </summary>
        /// <param name="array">The enumerable</param>
        /// <param name="active">Active/Inactive</param>
        public static void AllSetActive(this IEnumerable<GameObject> array, bool active)
        {
            foreach (var go in array) go.SetActive(active);
        }
        
        /// <summary>
        /// Set all of the GameObject parents of these behaviours as active or inactive.
        /// </summary>
        /// <param name="array">Enumerable of behaviours</param>
        /// <param name="active">Active/Inactive</param>
        public static void AllSetActive(this IEnumerable<Behaviour> array, bool active)
        {
            foreach (var go in array) go.gameObject.SetActive(active);
        }
        
        /// <summary>
        /// Enable or disable all of the behaviours in this list
        /// </summary>
        /// <param name="array">The list/enumerable of behaviours</param>
        /// <param name="active">Whether to set them to active or inactive</param>
        public static void AllSetEnabled(this IEnumerable<Behaviour> array, bool active)
        {
            foreach (var go in array) go.enabled = active;
        }
        
        /// <summary>
        /// Enable or disable all of the renderers in this list
        /// </summary>
        /// <param name="array">The list/enumerable of renderers</param>
        /// <param name="active">Whether to set them to active or inactive</param>
        public static void AllSetEnabled(this IEnumerable<Renderer> array, bool active)
        {
            foreach (var go in array) go.enabled = active;
        }
        
        /// <summary>
        /// Destroy all of the objects in this list
        /// </summary>
        /// <param name="array">The list/enumerable of objects</param>
        public static void AllDestroy(this IEnumerable<Object> array)
        {
            foreach (var obj in array) Object.Destroy(obj);
        }
        
        /// <summary>
        /// Immediately destroy all of the objects in this list
        /// </summary>
        /// <param name="array">The list/enumerable of objects</param>
        public static void AllDestroyImmediate(this IEnumerable<Object> array)
        {
            foreach (var obj in array) Object.DestroyImmediate(obj);
        }

        /// <summary>
        /// Return a randomly selected element from the array
        /// </summary>
        /// <param name="array">The array to select from</param>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>A random element selected from the array</returns>
        public static T RandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
        
        /// <summary>
        /// Return a randomly selected element from the list
        /// </summary>
        /// <param name="list">The list to select from</param>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>A random element selected from the list</returns>
        public static T RandomElement<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Returns true if the array is null, or has a length of 0
        /// </summary>
        /// <param name="array">The array to check</param>
        /// <typeparam name="T">Array type</typeparam>
        /// <returns>true if null or empty, false otherwise</returns>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// Returns true if the collection is null, or has a count of 0
        /// </summary>
        /// <param name="array">The collection to check</param>
        /// <typeparam name="T">Collection type</typeparam>
        /// <returns>true if null or empty, false otherwise</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> array)
        {
            return array == null || array.Count == 0;
        }
    }
}