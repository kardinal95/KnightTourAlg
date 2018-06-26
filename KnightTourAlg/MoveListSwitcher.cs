using System.Collections.Generic;

namespace KnightTourAlg
{
    class MoveListSwitcher
    {
        public int MaxReturn { get; }
        public int CurrentReturn { get; set; }
        public int CurrentSet { get; set; }
        public const int LastSet = 4;

        public List<List<Coords>> Sets { get; }

        public MoveListSwitcher(int max)
        {
            Sets = new List<List<Coords>>
            {
                new List<Coords>
                {
                    new Coords(-1, -2),
                    new Coords(-2, -1),
                    new Coords(-2, 1),
                    new Coords(1, -2),
                    new Coords(-1, 2),
                    new Coords(2, -1),
                    new Coords(1, 2),
                    new Coords(2, 1)
                },
                new List<Coords>
                {
                    new Coords(-1, 2),
                    new Coords(2, -1),
                    new Coords(1, 2),
                    new Coords(2, 1),
                    new Coords(-1, -2),
                    new Coords(-2, -1),
                    new Coords(-2, 1),
                    new Coords(1, -2)
                },
                new List<Coords>
                {
                    new Coords(1, 2),
                    new Coords(2, 1),
                    new Coords(-1, 2),
                    new Coords(2, -1),
                    new Coords(-2, 1),
                    new Coords(1, -2),
                    new Coords(-1, -2),
                    new Coords(-2, -1)
                },
                new List<Coords>
                {
                    new Coords(1, 2),
                    new Coords(-1, 2),
                    new Coords(2, -1),
                    new Coords(2, 1),
                    new Coords(-2, 1),
                    new Coords(-1, -2),
                    new Coords(1, -2),
                    new Coords(-2, -1)
                },
                new List<Coords>
                {
                    new Coords(2, 1),
                    new Coords(-1, -2),
                    new Coords(-2, -1),
                    new Coords(1, 2),
                    new Coords(-2, 1),
                    new Coords(2, -1),
                    new Coords(1, -2),
                    new Coords(-1, 2)
                }
            };

            CurrentReturn = 0;
            CurrentSet = 0;
            MaxReturn = max;
        }
    }
}