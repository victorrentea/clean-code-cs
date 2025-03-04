using System;
using System.Collections.Immutable;

namespace Victor.Training.Cleancode
{

    public static class ImmutableAdvanced
    {
        public static void Main()
        {
            ImmutableList<int> list = new[] { 1, 2, 3 }.ToImmutableList();

            Immutable immutable = new(new(1, 2), list, new Other(15));
            Console.WriteLine("Before: " + immutable);

            Point newPosition = Wilderness(immutable); // avoid reassigning variable for other meanings.
            Immutable translated = immutable with { Point = newPosition };

           
            Console.WriteLine("After:  " + translated);
        }

        private static Point Wilderness(Immutable immutable)
        {
            // dark, deep logic not expected to change the immutable object x+1,y+1
            //immutable.GetList().Clear();
            //immutable.X++;

            //Immutable copy = immutable with
            //{
            //    X = immutable.X + 1,
            //    Y = immutable.Y + 1
            //};

            return immutable.Point.Translate(+1, +1);
        }
    }
    public readonly record struct Point(int X, int Y)
    {
        internal Point Translate(int dx, int dy)
        {
            return this with
            {
                X = X + dx,
                Y = Y + dy
            };
        }
    }  

    // deep immutable
    public readonly record struct Immutable(
        //int X,
        //int Y,
        Point Point,
        ImmutableList<int> List,
        Other Other)
    {
        
    }


    public readonly record struct Other(int a) { }
}
