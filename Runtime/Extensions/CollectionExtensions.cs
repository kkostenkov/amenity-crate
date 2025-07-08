using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this T[] array) => array == null || array.Length == 0;

    public static T PickRandom<T>(this IEnumerable<T> collection)
    {
        if (!collection.Any())
            return default;

        int randomIndex = Random.Range(0, collection.Count());
        return collection.ElementAt(randomIndex);
    }

    public static T GetInBounds<T>(this IList<T> collection, ref int index)
    {
        if (index < 0) {
            index = 0;
        }
        else if (index >= collection.Count) {
            index = collection.Count - 1;
        }

        return collection[index];
    }

    /// <summary>
    /// https://stackoverflow.com/questions/273313/randomize-a-listt
    /// </summary>
    public static void Shuffle<T>(this IList<T> list, System.Random random)
    {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}