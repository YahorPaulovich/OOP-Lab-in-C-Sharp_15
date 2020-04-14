using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelComputingPlatform//Вариант 14
{/*№ 16 Платформа параллельных вычислений*/
    class Program
    {/*Задание
1. Используя TPL создайте длительную по времени задачу (на основе
Task) на выбор:
 поиск простых чисел (желательно взять «решето Эратосфена»),
 перемножение матриц,
 умножение вектора размера 100000 на число,
 создание множества Мандельброта
 или другой алгоритм.
1) Выведите идентификатор текущей задачи, проверьте во время
выполнения – завершена ли задача и выведите ее статус.
2) Оцените производительность выполнения используя объект
Stopwatch на нескольких прогонах.

Дополнительно:
Для сравнения реализуйте последовательный алгоритм.

2. Реализуйте второй вариант этой же задачи с токеном отмены
CancellationToken и отмените задачу.
3. Создайте три задачи с возвратом результата и используйте их для
выполнения четвертой задачи. Например, расчет по формуле.
4. Создайте задачу продолжения (continuation task) в двух вариантах:
1) C ContinueWith - планировка на основе завершения множества
предшествующих задач
2) На основе объекта ожидания и методов GetAwaiter(),GetResult();
5. Используя Класс Parallel распараллельте вычисления циклов For(),
ForEach(). Например, обработку (преобразования)последовательности,
генерация нескольких массивов по 1000000 элементов, быстрая
сортировка последователности, обработка текстов. Оцените
производительность по сравнению с обычными циклами
6. Используя Parallel.Invoke() распараллельте выполнение блока
операторов.
7. Используя Класс BlockingCollection реализуйте следующую задачу:
Есть 5 поставщиков бытовой техники, они завозят уникальные товары
на склад (каждый по одному) и 10 покупателей – покупают все подряд,
если товара нет - уходят. В вашей задаче: cпрос превышает
предложение. Изначально склад пустой. У каждого поставщика своя
скорость завоза товара. Каждый раз при изменении состоянии склада
выводите наименования товаров на складе.
8. Используя async и await организуйте асинхронное выполнение метода.*/       
        static void Main(string[] args)
        {
            // 1. Создание длительной по времени задачи (на основе Task), используя TPL
            Console.WriteLine("1. Создание длительной по времени задачи (на основе Task): решето Эратосфена, используя TPL:\n");

            Task task = Task.Run(() =>
            {
                Eratosfen eratosfen = new Eratosfen(100);

                // 1) Вывод идентификатора текущей задачи, проверка во время выполнения момента завершения задачи и вывод её статуса
                Console.WriteLine("1) Вывод идентификатора текущей задачи, проверка во время выполнения момента завершения задачи и вывод её статуса...");
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var item in eratosfen.Simple)
                {
                    Console.WriteLine($" {item} - id: {Task.CurrentId} Состояние: {Task.CompletedTask.IsCompleted} Статус: {Task.CompletedTask.Status}");
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }
           );
            task.Wait();         

            // 2) Оценка производительности выполнения, используя объект Stopwatch на нескольких прогонах
            Console.WriteLine("\n2) Оценка производительности выполнения, используя объект Stopwatch на нескольких прогонах...\n");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Task[] tasks = new Task[3]
            {
              new Task(() =>
              {
                Eratosfen eratosfen = new Eratosfen(20);
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var item in eratosfen.Simple)
                {
                    Console.WriteLine($" {item}");
                }               
                Console.WriteLine();
            }),
               new Task(() =>
              {
                Eratosfen eratosfen = new Eratosfen(50);
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var item in eratosfen.Simple)
                {
                    Console.WriteLine($" {item}");
                }
                Console.WriteLine();
            }),
               new Task(() =>
              {
                Eratosfen eratosfen = new Eratosfen(100);
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach (var item in eratosfen.Simple)
                {
                    Console.WriteLine($" {item}");
                }
                Console.WriteLine();
            })
            };
            // запуск задач в массиве
            foreach (var t in tasks)
            {
                t.Start();
                t.Wait();// ожидается завершения каждой задачи в массиве
            }
           
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadKey();
        }
    }
    #region Решето́ Эратосфе́на
    /// <summary>
    /// Решето́ Эратосфе́на — алгоритм нахождения всех простых чисел до некоторого целого числа n, который приписывают древнегреческому математику Эратосфену Киренскому.  
    /// Для нахождения всех простых чисел не больше заданного числа n, следуя методу Эратосфена, нужно выполнить следующие шаги:
    /// <list type="bullet">
    /// <item>
    /// <description>Выписать подряд все целые числа от двух до n(2, 3, 4, …, n);</description>
    /// </item>
    /// <item>
    /// <description>Пусть переменная p изначально равна двум — первому простому числу;</description>
    /// </item>  
    /// <item>
    /// <description>Вычеркнуть из списка все числа от 2p до n, делящиеся на p(то есть, числа 2p, 3p, 4p, …);</description>
    /// </item>
    /// <item>
    /// <description>Найти первое не вычеркнутое число, большее чем p, и присвоить значению переменной p это число;</description>
    /// </item>
    /// <item>
    /// <description>Повторять шаги 3 и 4 до тех пор, пока p не станет больше, чем n;</description>
    /// </item>
    /// <item>
    /// <description>Все не вычеркнутые числа в списке — простые числа.</description>
    /// </item>   
    /// </list>   
    /// </summary>
    #endregion
    class Eratosfen
    {
        List<int> simple;

        public Eratosfen(int MaxNumber)
        {
            simple = new List<int>();
            for (int i = 1; i < MaxNumber; i++)
                simple.Add(i);
            DoEratosfen();
        }

        int Step(int Prime, int startFrom)
        {
            int i = startFrom + 1;
            int Removed = 0;
            while (i < simple.Count)
                if (simple[i] % Prime == 0)
                {
                    simple.RemoveAt(i);
                    Removed++;
                }
                else
                    i++;
            return Removed;
        }

        void DoEratosfen()
        {
            int i = 1;
            while (i < simple.Count)
            {
                Step(simple[i], i);
                i++;
            }
        }

        public int[] Simple
        {
            get
            {
                return simple.ToArray();
            }
        }

    }
//</int></int>
}
