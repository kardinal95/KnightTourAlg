using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KnightTourAlg
{
    class Program
    {
        private static void Main(string[] args)
        {
            List<int> argsList;
            Handler handler;

            /*
            Huge tests here
            for (var x1 = 0; x1 < 8; x1++)
            {
                for (var y1 = 0; y1 < 8; y1++)
                {
                    for (var x2 = 0; x2 < 8; x2++)
                    {
                        for (var y2 = 0; y2 < 8; y2++)
                        {
                            Console.Write($"{x1}, {y1}, {x2}, {y2}: ");
                            handler = new Handler(new Coords(x1, y1), new Coords(x2, y2));
                            var res = handler.Execute(out var road);
                            Console.WriteLine(res ? "Success" : "Fail");
                        }
                    }
                }
            }
            */

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Args: posX-start posY-start posX-end posY-end");
                Console.ReadKey();
                return;
            }

            try
            {
                argsList = args.Select(int.Parse).ToList();
                if (argsList.Count < 4)
                {
                    Console.WriteLine($"Not enough parameters provided ({argsList.Count} of 4)");
                    Console.ReadKey();
                    return;
                }
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine($"Incorrect input: {e.Message}");
                Console.ReadKey();
                return;
            }

            try
            {
                handler = new Handler(new Coords(argsList[0], argsList[1]),
                                      new Coords(argsList[2], argsList[3]));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.ReadKey();
                return;
            }

            /* DISABLED RECURSIVE */
            /* var success = handler.ExecuteRecursive(out var path); */

            var watch = Stopwatch.StartNew();
            var success = handler.Execute(out var path);
            watch.Stop();
            Console.WriteLine(success ? $"Path: {string.Join(" ", path)}" : "No path!");
            Console.Write("Time of exectuion: ");
            Console.WriteLine(watch.ElapsedMilliseconds == 0 ? $"{watch.ElapsedTicks} ticks"
                                  : $"{watch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}