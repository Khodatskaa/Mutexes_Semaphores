namespace Mutexes_Semaphores
{
    class Program
    {
        private static Mutex _mutex = new Mutex();

        static void Main(string[] args)
        {
            Task thread1 = Task.Run(() => DisplayAscendingNumbers());
            Task thread2 = Task.Run(() => DisplayDescendingNumbers());

            Task.WaitAll(thread1, thread2);
        }

        private static void DisplayAscendingNumbers()
        {
            _mutex.WaitOne(); 
            try
            {
                for (int i = 0; i <= 20; i++)
                {
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
