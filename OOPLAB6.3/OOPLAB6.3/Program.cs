using System;
using System.Collections.Generic;
using System.Linq;

public class FunctionCache<TKey, TResult>
{
    private class CacheEntry
    {
        public TResult Result { get; set; }
        public DateTime ExpiryTime { get; set; }
    }

    private readonly Dictionary<TKey, CacheEntry> _cache = new Dictionary<TKey, CacheEntry>();
    private readonly TimeSpan _cacheDuration;

    public FunctionCache(TimeSpan cacheDuration)
    {
        _cacheDuration = cacheDuration;
    }

    public TResult GetOrAdd(TKey key, Func<TKey, TResult> function)
    {
        if (_cache.ContainsKey(key))
        {
            var entry = _cache[key];
            if (entry.ExpiryTime > DateTime.Now)
            {

                return entry.Result;
            }
            else
            {
                _cache.Remove(key);
            }
        }

        TResult result = function(key);

        _cache[key] = new CacheEntry
        {
            Result = result,
            ExpiryTime = DateTime.Now.Add(_cacheDuration) 
        };

        return result;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var cache = new FunctionCache<int, int>(TimeSpan.FromSeconds(5));

        Func<int, int> square = x => x * x;

        Console.WriteLine("Обчислення 5^2: " + cache.GetOrAdd(5, square));
        Console.WriteLine("Обчислення 5^2 з кешу: " + cache.GetOrAdd(5, square));

        System.Threading.Thread.Sleep(6000);

        Console.WriteLine("Обчислення 5^2 після закінчення терміну дії кешу: " + cache.GetOrAdd(5, square));
    }
}

