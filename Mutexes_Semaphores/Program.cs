namespace Mutexes_Semaphores
{
    class Program
    {
        private static Mutex _mutex = new Mutex();
        private static int[] _dataArray = new int[21];

        static void Main(string[] args)
        {
            Task thread1 = Task.Run(() => DisplayAscendingNumbers());
            Task thread2 = Task.Run(() => DisplayDescendingNumbers());

            Task.WaitAll(thread1, thread2);

            int maxValue = _dataArray[_dataArray.Length - 1]; 
            Console.WriteLine($"Maximum value in the array: {maxValue}");
            Console.WriteLine("Modified array:");
            for (int i = 0; i < _dataArray.Length; i++)
            {
                Console.WriteLine(_dataArray[i]);
            }
        }

        private static void DisplayAscendingNumbers()
        {
            _mutex.WaitOne(); 
            try
            {
                for (int i = 0; i <= 20; i++)
                {
                    _dataArray[i] = i; 
                    Console.WriteLine(i);
                    Thread.Sleep(100); 
                }
            }
            finally
            {
                _mutex.ReleaseMutex(); 
            }
        }

        private static void DisplayDescendingNumbers()
        {
            _mutex.WaitOne(); 
            try
            {
                for (int i = 10; i >= 0; i--)
                {
                    _dataArray[i] = i; 
                    Console.WriteLine(i);
                    Thread.Sleep(100);
                }
            }
            finally
            {
                _mutex.ReleaseMutex(); 
            }
        }
    }
}
