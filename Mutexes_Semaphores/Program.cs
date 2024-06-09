namespace Mutexes_Semaphores
{
    class Program
    {
        private static Mutex mutex = new Mutex();
        private static string inputFileName = "numbers.txt";
        private static string outputFileName1 = "prime_numbers.txt";
        private static string outputFileName2 = "prime_numbers_ending_with_7.txt";

        static void Main(string[] args)
        {
            Thread thread1 = new Thread(GenerateRandomNumbers);
            Thread thread2 = new Thread(FilterPrimeNumbers);
            Thread thread3 = new Thread(FilterPrimeNumbersEndingWith7);

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();

            Console.WriteLine("All threads have completed their tasks.");
            Console.ReadLine();
        }

        static void GenerateRandomNumbers()
        {
            mutex.WaitOne();
            try
            {
                Random rand = new Random();
                List<int> numbers = new List<int>();
                for (int i = 0; i < 100; i++)
                {
                    numbers.Add(rand.Next(1, 1000));
                }
                File.WriteAllLines(inputFileName, numbers.Select(n => n.ToString()));
                Console.WriteLine("Random numbers have been generated and written to file.");
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        static void FilterPrimeNumbers()
        {
            mutex.WaitOne();
            try
            {
                var numbers = File.ReadAllLines(inputFileName).Select(int.Parse).ToList();
                var primeNumbers = numbers.Where(IsPrime).ToList();
                File.WriteAllLines(outputFileName1, primeNumbers.Select(n => n.ToString()));
                Console.WriteLine("Prime numbers have been filtered and written to file.");
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        static void FilterPrimeNumbersEndingWith7()
        {
            mutex.WaitOne();
            try
            {
                var primeNumbers = File.ReadAllLines(outputFileName1).Select(int.Parse).ToList();
                var primeNumbersEndingWith7 = primeNumbers.Where(n => n % 10 == 7).ToList();
                File.WriteAllLines(outputFileName2, primeNumbersEndingWith7.Select(n => n.ToString()));
                Console.WriteLine("Prime numbers ending with 7 have been filtered and written to file.");
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        static bool IsPrime(int num)
        {
            if (num <= 1)
                return false;
            if (num == 2 || num == 3)
                return true;
            if (num % 2 == 0 || num % 3 == 0)
                return false;
            int i = 5;
            while (i * i <= num)
            {
                if (num % i == 0 || num % (i + 2) == 0)
                    return false;
                i += 6;
            }
            return true;
        }
    }
}
