namespace Mutexes_Semaphores
{
    class Program
    {
        private static Mutex mutex = new Mutex();
        private static Random random = new Random();
        private static int totalPlayersVisited = 0;
        private static int totalPlayers = 0;
        private static string reportFileName = "casino_report.txt";
        private static int betAmount;

        static void Main(string[] args)
        {
            totalPlayers = random.Next(20, 101);
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < totalPlayers; i++)
            {
                Thread thread = new Thread(PlayerThread);
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("All players have finished playing. Generating report...");
            GenerateReport();
            Console.WriteLine("Report generated successfully.");
            Console.ReadLine();
        }

        static void PlayerThread()
        {
            int playerId = Interlocked.Increment(ref totalPlayersVisited);
            int money = random.Next(100, 1001);
            int betsPlaced = 0;
            int betsWon = 0;

            while (true)
            {
                int betAmount = random.Next(10, 101);
                int betNumber = random.Next(0, 37);

                mutex.WaitOne();
                try
                {
                    if (money <= 0)
                    {
                        break;
                    }

                    if (betsPlaced >= 100)
                    {
                        break;
                    }

                    int rouletteNumber = random.Next(0, 37);
                    if (rouletteNumber == betNumber)
                    {
                        money += betAmount;
                        betsWon++;
                    }
                    else
                    {
                        money -= betAmount;
                    }

                    betsPlaced++;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }

                Thread.Sleep(10);
            }

            mutex.WaitOne();
            try
            {
                using (StreamWriter writer = new StreamWriter(reportFileName, true))
                {
                    writer.WriteLine($"Player ID: {playerId}");
                    writer.WriteLine($"Initial Money: {money + betAmount}");
                    writer.WriteLine($"Bets Placed: {betsPlaced}");
                    writer.WriteLine($"Bets Won: {betsWon}");
                    writer.WriteLine($"Final Money: {money}");
                    writer.WriteLine();
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        static void GenerateReport()
        {
            mutex.WaitOne();
            try
            {
                using (StreamWriter writer = new StreamWriter(reportFileName, true))
                {
                    writer.WriteLine($"Total Players Visited: {totalPlayersVisited}");
                    writer.WriteLine($"Total Players: {totalPlayers}");
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
