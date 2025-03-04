using System;
using System.Collections.Immutable;

namespace Victor.Training.Cleancode
{

    public static class ImmutableAdvanced
    {
        public static void Main()
        {
            ImmutableList<int> list = new[] { 1, 2, 3 }.ToImmutableList();

            Immutable immutable = new(1, 2, list, new Other(15));
            Console.WriteLine("Before: " + immutable);

            Wilderness(immutable);
            list.Clear();

            Console.WriteLine("After:  " + immutable);
        }

        private static void Wilderness(Immutable immutable)
        {
            // dark, deep logic not expected to change the immutable object x,y
            //immutable.GetList().Clear();
        }
    }

    // shallow immutable
    public readonly record struct Immutable(
        int X,
        int Y,
        ImmutableList<int> List,
        Other Other)
    { }


    public readonly record struct Other(int a) { }
}
