using System.Collections.Generic;

public delegate TResult Func<in T, out TResult>(T arg);

public static class IListExtensions
{
    public static T Find<T>(this IList<T> source, Func<T, bool> condition)
    {
        foreach (T item in source)
            if (condition(item))
                return item;
        return default(T);
    }

    public static IList<T> FindAll<T>(this IList<T> source, Func<T, bool> condition)
    {
        IList<T> all = new List<T>();
        foreach (T item in source)
            if (condition(item))
                all.Add(item);
        return all;
    }

    public static void RemoveAll<T>(this IList<T> source, Func<T, bool> condition)
    {
        foreach (var item in source.FindAll<T>(condition))
            source.Remove(item);
    }
}