using System;
using System.Collections.Generic;

namespace KnightTourAlg
{
    class MoveListSwitcher
    {
        public int MaxReturn { get; set; }
        public int CurrentReturn { get; set; }
        public int CurrentSet { get; set; }
        public const int LastSet = 4;

        public List<List<Coords>> Sets { get; }

        public void SwitchSet()
        {
            if (CurrentSet == LastSet)
            {
                CurrentSet = 0;
                MaxReturn *= 10;
            }
            else
            {
                CurrentSet++;
            }
        }

        public MoveListSwitcher(int max = 1)
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
                    new Coords(1, 2),
                    new Coords(-1, 2),
                    new Coords(2, -1),
                    new Coords(-2, 1),
                    new Coords(2, 1),
                    new Coords(1, -2),
                    new Coords(-1, -2),
                    new Coords(-2, -1)
                },
                new List<Coords>
                {
                    new Coords(1, -2),
                    new Coords(-1, 2),
                    new Coords(1, 2),
                    new Coords(-1, -2),
                    new Coords(2, 1),
                    new Coords(-2, 1),
                    new Coords(-2, -1),
                    new Coords(2, -1)
                },
                new List<Coords>
                {
                    new Coords(-2, -1),
                    new Coords(-1, -2),
                    new Coords(-2, 1),
                    new Coords(2, -1),
                    new Coords(1, 2),
                    new Coords(1, -2),
                    new Coords(2, 1),
                    new Coords(-1, 2)
                },
                new List<Coords>
                {
                    new Coords(-2, 1),
                    new Coords(-1, -2),
                    new Coords(1, 2),
                    new Coords(-2, -1),
                    new Coords(-1, 2),
                    new Coords(1, -2),
                    new Coords(2, -1),
                    new Coords(2, 1)
                }
            };

            CurrentReturn = 0;
            CurrentSet = 0;
            MaxReturn = max;
        }
    }
}