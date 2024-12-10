using System;

public class Calculator<T> where T : struct
{
    public delegate T Operation(T a, T b);
    public T Add(T a, T b, Operation operation) => operation(a, b);
    public T Subtract(T a, T b, Operation operation) => operation(a, b);
    public T Multiply(T a, T b, Operation operation) => operation(a, b);

    public T Divide(T a, T b, Operation operation)
    {
        if (b.Equals(default(T)))
        {
            throw new DivideByZeroException("Ділення на нуль недопустиме.");
        }
        return operation(a, b);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var intCalculator = new Calculator<int>();
        Calculator<int>.Operation intAdd = (a, b) => a + b;
        Calculator<int>.Operation intSubtract = (a, b) => a - b;
        Console.WriteLine("Int Add: " + intCalculator.Add(10, 5, intAdd));
        Console.WriteLine("Int Subtract: " + intCalculator.Subtract(10, 5, intSubtract));

        var doubleCalculator = new Calculator<double>();
        Calculator<double>.Operation doubleMultiply = (a, b) => a * b;
        Calculator<double>.Operation doubleDivide = (a, b) => a / b;
        Console.WriteLine("Double Multiply: " + doubleCalculator.Multiply(10.5, 5.2, doubleMultiply));
        Console.WriteLine("Double Divide: " + doubleCalculator.Divide(10.5, 2.1, doubleDivide));
    }
}

