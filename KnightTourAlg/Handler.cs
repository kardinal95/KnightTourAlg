using System.Collections.Generic;
using System.Linq;

namespace KnightTourAlg
{
    class Handler
    {
        private readonly List<Coords> moves = new List<Coords>
        {
            new Coords(-1, -2),
            new Coords(-2, -1),
            new Coords(-2, 1),
            new Coords(1, -2),
            new Coords(-1, 2),
            new Coords(2, -1),
            new Coords(1, 2),
            new Coords(2, 1)
        };

        private readonly int width;
        private readonly int height;
        private readonly bool[,] passed;

        private int step;
        private readonly Coords start;
        private readonly Coords end;
        private Coords? lastCancelled;

        public Handler(int width, int height, Coords start, Coords end)
        {
            this.width = width;
            this.height = height;
            this.start = start;
            this.end = end;
            step = 0;
            lastCancelled = null;
            passed = new bool[width, height];
        }

        public bool Execute(out List<Coords> path)
        {
            path = new List<Coords> {start};
            passed[start.X, start.Y] = true;

            for (step = 1; step < width * height;)
            {
                var move = MakeMoveFrom(path.Last(), step);
                if (move.HasValue)
                {
                    step++;
                    path.Add(move.Value);
                    passed[move.Value.X, move.Value.Y] = true;
                    lastCancelled = null;
                }
                else
                {
                    if (path.Count == 1)
                    {
                        break;
                    }

                    step--;
                    var last = path.Last();
                    passed[last.X, last.Y] = false;
                    lastCancelled = last;
                    path.Remove(last);
                }
            }

            return step == width * height;
        }

        private Coords? MakeMoveFrom(Coords from, int step)
        {
            var availableMoves = SortVarnsdorf(FindAvailableMoves(from, step), step);
            if (availableMoves.Count == 0)
            {
                return null;
            }

            if (!lastCancelled.HasValue)
            {
                return availableMoves[0];
            }

            var index = availableMoves.Count;
            for (var i = 0; i < availableMoves.Count; i++)
            {
                if (availableMoves[i] != lastCancelled.Value)
                {
                    continue;
                }

                index = i + 1;
                break;
            }

            if (index >= availableMoves.Count)
            {
                return null;
            }

            return availableMoves[index];
        }

        public bool ExecuteRecursive(out List<Coords> path)
        {
            path = new List<Coords> {start};
            passed[start.X, start.Y] = true;

            if (!TryFindPath(1, start, out var furtherPath))
            {
                return false;
            }

            path.AddRange(furtherPath);
            return true;
        }

        private bool TryFindPath(int move, Coords from, out List<Coords> path)
        {
            path = new List<Coords>();
            if (move == width * height)
            {
                return true;
            }

            var available = SortVarnsdorf(FindAvailableMoves(from, move), move);
            foreach (var coords in available)
            {
                passed[coords.X, coords.Y] = true;
                if (TryFindPath(move + 1, coords, out var furtherPath))
                {
                    path.Add(coords);
                    path.AddRange(furtherPath);
                    return true;
                }

                passed[coords.X, coords.Y] = false;
            }

            return false;
        }

        private List<Coords> SortVarnsdorf(IReadOnlyCollection<Coords> input, int move)
        {
            var result = input;
            return new List<Coords>(result.OrderBy(x => FindAvailableMoves(x, move).Count));
        }

        private List<Coords> FindAvailableMoves(Coords from, int step)
        {
            var available = new List<Coords>();

            foreach (var move in moves)
            {
                var target = move + from;
                if (target.X < 0 || target.X >= width)
                {
                    continue;
                }

                if (target.Y < 0 || target.Y >= height)
                {
                    continue;
                }

                if (passed[target.X, target.Y])
                {
                    continue;
                }

                if (step != width * height - 1 && target == end)
                {
                    continue;
                }

                if (step == width * height - 1 && target != end)
                {
                    continue;
                }

                available.Add(target);
            }

            return available;
        }
    }
}