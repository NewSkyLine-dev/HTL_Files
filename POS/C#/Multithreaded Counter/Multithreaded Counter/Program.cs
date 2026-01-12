using System;
using System.Threading;

namespace Multithreaded_Counter
{
    internal class Program
    {
        static int counter = 0;  // Gemeinsamer Counter
        private static object lockObject = new object();  // Lock-Objekt zur Sicherstellung der Synchronisation

        static void Main(string[] args)
        {
            // Überprüfen, ob die richtigen Argumente übergeben wurden
            if (args.Length != 2 || !int.TryParse(args[0], out int threadCount) || !int.TryParse(args[1], out int iterationsPerThread))
            {
                Console.WriteLine("Bitte geben Sie die Anzahl der Threads und die Anzahl der Iterationen pro Thread als Argumente an.");
                Console.WriteLine("Beispiel: 5 10");
                return;
            }

            // Array für die Threads
            Thread[] threads = new Thread[threadCount];

            // Threads erstellen und starten
            for (int i = 0; i < threadCount; i++)
            {
                int threadId = i + 2;  // Thread-ID beginnt bei 2
                threads[i] = new Thread(() => IncrementCounter(threadId, iterationsPerThread));
            }

            // Alle Threads gleichzeitig starten
            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            // Warten, bis alle Threads fertig sind
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        static void IncrementCounter(int id, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                lock (lockObject)  // Synchronisation, um Datenkorruption zu vermeiden (kritischer Abschnitt)
                {
                    if (counter % id == 0)
                    {
                        Console.WriteLine("ID: {0,3} Counter: {1,8} Modulo: {2}", id, counter, counter % id);
                    }

                    counter++;
                }
            }
        }
    }

}
