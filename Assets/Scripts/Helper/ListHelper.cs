using UnityEngine;
using System.Collections.Generic;

public static class ListHelper {

    public static bool AddDistinct<T>(this List<T> list, T element) {
        if (list.Contains(element)) { return false; }
        list.Add(element);
        return true;
    }

    public static void AddRangeDistinct<T>(this List<T> list, IEnumerable<T> listToBeAdded) {
        foreach (T element in listToBeAdded) {
            list.AddDistinct(element);
        }
    }

	public static T GetNext<T>(this List<T> list, T current, int offset, bool loop = true) {
        int index = list.IndexOf(current);

        index += offset;

        if (!loop) {
            if (index >= list.Count || index < 0) {
                return default(T);
            }
        }

        while (index < 0) {
            index += list.Count;
        }
        index = index % list.Count;

        return list[index];
    }

	public static T GetAtIndex<T>(this List<T> list, int index, bool loop = true) {
		if (loop) {
			int loopIndex = index;
			while (loopIndex >= list.Count) {
				loopIndex -= list.Count;
			}
			return list[loopIndex];
		}

		return list[index];
	}

    public static T GetRandom<T>(this List<T> list) {
        if (list.Count == 0) { return default(T); }
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static T GetLast<T>(this List<T> list) {
        if (list.Count == 0) { return default(T); }
        return list[list.Count - 1];
    }

    public static List<T> GetAtIndices<T>(this List<T> list, List<int> indices) {
        List<T> result = new List<T>();
        foreach (int index in indices) {
            if (index < 0 || index > list.Count - 1) { continue; }
        }
        indices.ForEach(x => result.Add(list[x]));
        return result;
    }

    public static void Shuffle<T>(this List<T> list) {
        List<T> other = new List<T>(list);
        list.Clear();
        while (other.Count > 0) {
            int index = Random.Range(0, other.Count);
            list.Add(other[index]);
            other.RemoveAt(index);
        }
    }

    public static void Cap<T>(this List<T> list, int amount) {
        amount = Mathf.Max(0, amount);
        while (list.Count > amount) {
            list.RemoveAt(list.Count - 1);
        }
    }

	public static int GetElementCount<T>(this List<T> list, T element) {
		return list.FindAll(x => x.Equals(element)).Count;
	}

}
