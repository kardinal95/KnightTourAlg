using System;

namespace KnightTourAlg
{
    class Program
    {
        private static void Main(string[] args)
        {
            var handler = new Handler(6, 5, new Coords(0, 0), new Coords(2, 3));

            var success = handler.ExecuteRecursive(out var path);
            Console.WriteLine(string.Join(" ", path));
            Console.ReadKey();
        }
    }
}