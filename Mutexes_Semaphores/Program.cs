namespace Mutexes_Semaphores
{
    class Program
    {
        static void Main(string[] args)
        {
            string mutexName = "SingleInstanceMutex";
            Mutex? mutex = null;
            bool createdNew = false;

            try
            {
                mutex = new Mutex(true, mutexName, out createdNew);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return;
            }

            if (createdNew)
            {
                Console.WriteLine("Application started successfully. Press any key to exit.");
                Console.ReadKey();
                mutex.ReleaseMutex();
            }
            else
            {
                return;
            }
        }
    }
}
