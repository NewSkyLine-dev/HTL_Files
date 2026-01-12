using Primzahlberechnung;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Primzahlen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[0], out int threadCount) || !int.TryParse(args[1], out int maxPrim))
            {
                Console.WriteLine("Bitte geben Sie die Anzahl der Threads und den Höchstwert der Primzahlen an.");
                return;
            }

            // Multithreaded run
            Stopwatch multiWatch = new Stopwatch();
            multiWatch.Start();
            int multiMaxPrim = 0, multiNumber = 0, multiTests = 0;
            Calculator.Prim(maxPrim, threadCount, out multiMaxPrim, out multiNumber, out multiTests);
            multiWatch.Stop();
            Console.WriteLine("Mit {0} Threads:", threadCount);
            Console.WriteLine("Es wurden {0} Primzahlen gefunden", multiNumber);
            Console.WriteLine("Die höchste gefundene Primzahl ist {0}", multiMaxPrim);
            Console.WriteLine("Die Laufzeit betrug {0:F0} Millisekunden", multiWatch.ElapsedMilliseconds);
            Console.WriteLine("Es wurden {0} Vergleiche durchgeführt", multiTests);
        }

        private static void Prim(int max, out int maxPrim, out int number, out int tests)
        {
            List<int> prims = new List<int>();
            int i = 5;
            tests = 0;
            prims.Add(2);
            prims.Add(3);
            while (i < max)
            {
                int maxTeiler = (int)Math.Sqrt(i) + 1;
                int j = 0;
                while (true)
                {
                    int n = prims[j];
                    int rest = (i % n);
                    ++tests;
                    if (rest == 0)
                        break; // Keine Primzahl
                    if (n >= maxTeiler)
                    {
                        prims.Add(i);
                        break;
                    }
                    ++j;
                }
                i += 2;
            }
            number = prims.Count;
            maxPrim = prims[number - 1];
        }
    }
}