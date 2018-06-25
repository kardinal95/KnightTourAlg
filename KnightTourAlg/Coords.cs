namespace KnightTourAlg
{
    struct Coords
    {
        public readonly int X;
        public readonly int Y;

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Coords operator +(Coords left, Coords right)
        {
            return new Coords(left.X + right.X, left.Y + right.Y);
        }

        public static bool operator ==(Coords left, Coords right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Coords left, Coords right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}