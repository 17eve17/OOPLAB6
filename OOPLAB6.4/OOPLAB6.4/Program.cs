using System;
using System.Collections.Generic;
using System.Linq;

public delegate void TaskExecution<TTask>(TTask task);

public class TaskScheduler<TTask, TPriority> where TPriority : IComparable
{
    private readonly SortedList<TPriority, Queue<TTask>> _tasks;

    public TaskScheduler()
    {
        _tasks = new SortedList<TPriority, Queue<TTask>>();
    }

    public void AddTask(TTask task, TPriority priority)
    {
        if (!_tasks.ContainsKey(priority))
        {
            _tasks[priority] = new Queue<TTask>();
        }

        _tasks[priority].Enqueue(task);
    }

    public void ExecuteNext(TaskExecution<TTask> taskExecution)
    {
        if (_tasks.Count == 0)
        {
            Console.WriteLine("Немає завдань для виконання.");
            return;
        }

        var highestPriority = _tasks.Keys.Last();
        var taskQueue = _tasks[highestPriority];

        var task = taskQueue.Dequeue();
        taskExecution(task);

        if (taskQueue.Count == 0)
        {
            _tasks.Remove(highestPriority);
        }
    }

    public void ShowTasks()
    {
        foreach (var priority in _tasks.Keys.Reverse())
        {
            Console.WriteLine($"Пріоритет {priority}:");
            foreach (var task in _tasks[priority])
            {
                Console.WriteLine($" - {task}");
            }
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var scheduler = new TaskScheduler<string, int>();

        scheduler.AddTask("Завдання 1", 2);
        scheduler.AddTask("Завдання 2", 1);
        scheduler.AddTask("Завдання 3", 3);

        TaskExecution<string> taskExecution = task => Console.WriteLine($"Виконано завдання: {task}");

        Console.WriteLine("Завдання в черзі:");
        scheduler.ShowTasks();

        Console.WriteLine("\nВиконання завдання з найвищим пріоритетом:");
        scheduler.ExecuteNext(taskExecution);

        Console.WriteLine("\nОновлені завдання в черзі:");
        scheduler.ShowTasks();

        while (true)
        {
            Console.WriteLine("\nВведіть завдання (або 'exit' для завершення):");
            string inputTask = Console.ReadLine();

            if (inputTask.ToLower() == "exit")
                break;

            Console.WriteLine("Введіть пріоритет завдання (число):");
            if (int.TryParse(Console.ReadLine(), out int priority))
            {
                scheduler.AddTask(inputTask, priority);
                Console.WriteLine($"Завдання '{inputTask}' з пріоритетом {priority} додано.");
            }
            else
            {
                Console.WriteLine("Невірний ввід пріоритету.");
            }

            Console.WriteLine("\nВиконання завдання з найвищим пріоритетом:");
            scheduler.ExecuteNext(taskExecution);
        }
    }
}
