using System;
using System.Threading;
using Configator;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // var obj = new ConfigatorObject();
            //
            // Configator.Configator.Get(obj);
            // var newObj = Configator.Configator.Get<ConfigatorObject>();
            //
            // Console.ReadLine();
            maxInterval(new[] {10, -3, -12, 8, 42, 1, -7, 0, 3});
        }
        
        // Задача о поиске отрезка с максимальной суммой ⭐️⭐️
        // Дана последовательность из целых чисел Нужно найти закрытый интервал с максимальной суммой
        // assert([10, -3, -12, 8, 42, 1, -7, 0, 3] == [3, 5])
        static int[] maxInterval(int[] arr)
        {
            var from = 0;
            var to = 0;
            var sum = arr[0];
            var best = sum;

            for (var i = 1; i < arr.Length; ++i)
            {
                var nextSum = sum + arr[i];

                if (nextSum > best)
                {
                    sum = nextSum;
                    best = nextSum;
                    from = i;
                    to = i - from;
                }
                else
                {
                    ++to;
                }
            }

            Console.WriteLine($"[{from}, {to}]");
            return new[] {from, to};
        }
    }
}