namespace Mutexes_Semaphores
{
    class Program
    {
        private static Mutex _mutex = new Mutex();
        private static int[] _dataArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private const int ArbitraryNumber = 5; 

        static void Main(string[] args)
        {
            Task.Run(() => ModifyArray());
            Task.Run(() => FindMaxValue());

            Console.ReadLine();
        }

        private static void ModifyArray()
        {
            _mutex.WaitOne();
            try
            {
                for (int i = 0; i < _dataArray.Length; i++)
                {
                    _dataArray[i] += ArbitraryNumber;
                    Console.WriteLine($"Modified element {i}: {_dataArray[i]}");
                }
                Console.WriteLine("Array modification completed.");
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        private static void FindMaxValue()
        {
            _mutex.WaitOne();
            try
            {
                int maxValue = _dataArray.Max();
                Console.WriteLine($"Maximum value in the array: {maxValue}");
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}
