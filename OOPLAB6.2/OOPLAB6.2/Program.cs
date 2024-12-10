using System;
using System.Collections.Generic;
using System.Linq;

// Делегат для критеріїв
public delegate bool Criteria<T>(T item);

public class Repository<T>
{
    private readonly List<T> _items;

    public Repository()
    {
        _items = new List<T>();
    }

    // Метод для додавання елементів
    public void Add(T item)
    {
        _items.Add(item);
    }

    // Метод для пошуку за критерієм
    public IEnumerable<T> Find(Criteria<T> criteria)
    {
        return _items.Where(item => criteria(item));
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var intRepository = new Repository<int>();
        intRepository.Add(1);
        intRepository.Add(2);
        intRepository.Add(3);
        intRepository.Add(4);
        intRepository.Add(5);

        Criteria<int> isEven = x => x % 2 == 0;

        Console.WriteLine("Парні числа:");
        foreach (var item in intRepository.Find(isEven))
        {
            Console.WriteLine(item);
        }

        var stringRepository = new Repository<string>();
        stringRepository.Add("Apple");
        stringRepository.Add("Banana");
        stringRepository.Add("Cherry");
        stringRepository.Add("Date");

        Criteria<string> startsWithA = s => s.StartsWith("A");

        Console.WriteLine("\nСлова, що починаються на 'A':");
        foreach (var item in stringRepository.Find(startsWithA))
        {
            Console.WriteLine(item);
        }
    }
}
