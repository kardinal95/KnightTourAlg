using System;
using System.Collections.Generic;
using System.Linq;

namespace KnightTourAlg
{
    class Program
    {
        private static void Main(string[] args)
        {
            List<int> argsList;
            Handler handler;

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Args:width height posX-start posY-start posX-end posY-end");
                Console.ReadKey();
                return;
            }

            try
            {
                argsList = args.Select(int.Parse).ToList();
                if (argsList.Count < 6)
                {
                    Console.WriteLine($"Not enough parameters provided ({argsList.Count} of 6)");
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
                handler = new Handler(argsList[0], argsList[1], new Coords(argsList[2], argsList[3]),
                                      new Coords(argsList[4], argsList[5]));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.ReadKey();
                return;
            }

            /* DISABLED RECURSIVE */
            /* var success = handler.ExecuteRecursive(out var path); */

            var success = handler.Execute(out var path);
            Console.WriteLine(success ? $"Path: {string.Join(" ", path)}" : "No path!");
            Console.ReadKey();
        }
    }
}