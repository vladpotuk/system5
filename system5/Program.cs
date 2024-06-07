using System;
using System.IO;
using System.Threading;

class Program
{
    static int[] numbers;
    static int max, min;
    static double average;
    static readonly object lockObject = new object();
    static readonly string filePath = "results.txt";

    static void Main()
    {
        numbers = GenerateNumbers(10000);

        Thread maxThread = new Thread(FindMax);
        Thread minThread = new Thread(FindMin);
        Thread averageThread = new Thread(FindAverage);
        Thread writeThread = new Thread(WriteResultsToFile);

        maxThread.Start();
        minThread.Start();
        averageThread.Start();
        writeThread.Start();

        maxThread.Join();
        minThread.Join();
        averageThread.Join();
        writeThread.Join();
    }

    static int[] GenerateNumbers(int count)
    {
        Random rand = new Random();
        int[] numbers = new int[count];
        for (int i = 0; i < count; i++)
        {
            numbers[i] = rand.Next(1, 10001); // Генерація чисел від 1 до 10000
        }
        return numbers;
    }

    static void FindMax()
    {
        int maxVal = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] > maxVal)
            {
                maxVal = numbers[i];
            }
        }
        lock (lockObject)
        {
            max = maxVal;
        }
    }

    static void FindMin()
    {
        int minVal = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] < minVal)
            {
                minVal = numbers[i];
            }
        }
        lock (lockObject)
        {
            min = minVal;
        }
    }

    static void FindAverage()
    {
        double sum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            sum += numbers[i];
        }
        lock (lockObject)
        {
            average = sum / numbers.Length;
        }
    }

    static void WriteResultsToFile()
    {
        lock (lockObject)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Набір чисел:");
                foreach (int number in numbers)
                {
                    writer.WriteLine(number);
                }
                writer.WriteLine();
                writer.WriteLine($"Максимум: {max}");
                writer.WriteLine($"Мінімум: {min}");
                writer.WriteLine($"Середнє арифметичне: {average}");
            }
        }
    }
}
