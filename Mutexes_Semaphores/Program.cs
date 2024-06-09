namespace Mutexes_Semaphores
{
    class Program
    {
        static SemaphoreSlim semaphore = new SemaphoreSlim(3);

        static void Main(string[] args)
        {
            for (int i = 1; i <= 10; i++)
            {
                int threadId = i;
                Thread thread = new Thread(() => DoWork(threadId));
                thread.Start();
            }

            Console.ReadLine();
        }

        static void DoWork(int threadId)
        {
            semaphore.Wait(); 

            try
            {
                Console.WriteLine($"Thread {threadId} started.");

                Random random = new Random();
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Thread {threadId} output: {random.Next(100)}");
                }

                Console.WriteLine($"Thread {threadId} completed.");
            }
            finally
            {
                semaphore.Release(); 
            }
        }
    }
}
