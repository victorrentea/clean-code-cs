namespace clean_code_cs
{
    public class NeverDoThis
    {
        public static void Top()
        {    
            int counter = 0;

            // A: ref #hate
            RefParam(ref counter); // IN OUT in PL/SQL 

            // B: out params
            int delta;
            bool b1 = OutParam(out delta); // OUT in PL/SQL
            counter += delta;

            // C: return a tuple #love
            (bool b2, int delta2) = ReturnTuple();
            counter += delta2;

            // D: return a record with named #love fields
            Record record = ReturnRecord();
            counter += record.PointsEarned;
        }

       

        private static Boolean RefParam(ref int sumUpToThis)
        {
            sumUpToThis += 2; // in+out
            return true;
        }

        private static Boolean OutParam(out int sumUpToThis)
        {
            sumUpToThis = 2; // out (no out)
            return true;
        }
        

        private static (bool RightToForfeit,int PointEarned) ReturnTuple()
        {
            return (true,2);
        }

        public record class Record(bool RightToForfeit, int PointsEarned);

        private static Record ReturnRecord()
        {
            return new Record(true, 2);
        }
    }
}

