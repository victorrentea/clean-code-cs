namespace Victor.Training.Cleancode
{
    public class CarSearch
    {
        // run tests
        public List<CarModel> FilterCarModels(CarSearchCriteria criteria, List<CarModel> carModels)
        {
            List<CarModel> results = carModels
                .Where(carModel => criteria.YearRange.Intersects(carModel.YearRange))
                .ToList();
            Console.WriteLine("Imagine more filtering logic ...");
            return results;
        }
    }

    public class SomeOtherClientCode
    {
        private void ApplyLengthFilter() // pretend
        {
            Console.WriteLine(new YearRange(1000, 1600).Intersects(new YearRange(1250, 2000)));
        }

    }
    // 1) overly-specializd class; 0 chances of reuse outside of this method
    //record class CarCriteria { 
    //    int CriteriaStartYear;
    //    int CriteriaEndYear;
    //    int CarModelStartYear;
    //    int CarModelEndYear;
    //}

    // 2) extension function for CarModel, iff the CarModel is out of my reach
    // static public bool IntervalsIntersect(this CarModel carModel, int start, int end)

    // 3) method in the CarModel = move behavior next to state = OOP

    // 4) Extract an "Value Object"= small immutale group of data without a PK(identity)
    // eg: Money(ammount,currency)
    public record class YearRange(int Start, int End)
    {
        public bool Intersects(YearRange other)
        {
            return Start <= other.End && other.Start <= End;
        }
    }

    public static class MathUtil
    {
        //public static bool IntervalsIntersect(CarCriteria carCriteria) (#1)
        //static public bool IntervalsIntersect(this CarModel carModel, int start, int end) (#2)
        //public static bool IntervalsIntersect(int start1, int end1, int start2, int end2) (initial)
        //public static bool IntervalsIntersect(YearRange range1, YearRange range2) // #4
        //{
        //    return range1.Start <= range2.End && range2.Start <= range1.End;
        //}
    }

    public class CarSearchCriteria // a DTO received from JSON = part of your REST API
    {
        public int StartYear { get; } // rem
        public int EndYear { get; } // rem
        
        public string Make { get; }

        //"autoproperty": public YearRange yearRange; // good to reuse the VO; BAD: breaks your API 
        public YearRange YearRange => new YearRange(StartYear, EndYear);

        public CarSearchCriteria(int startYear, int endYear, string make)
        {
            this.Make = make;
            if (startYear > endYear) throw new ArgumentException("start larger than end");
            this.StartYear = startYear;
            this.EndYear = endYear;
        }
    }

    //static class YearRangeValidator { .......}

    public class CarModel
    {
        //public CarModel? Id { get; private set; }
        public long? Id { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }

        public int StartYear { get; private set; } // REM
        public int EndYear { get; private set; }// REM

        // if this is my internal "Domain Model"
        // = structures used in the most complex parts of code,
        //    kept internal, never exposed to the outside world,
        // then consider replacing the 2 properties above with a single one YearRange.
        public YearRange YearRange => new YearRange(StartYear, EndYear);

        public CarModel(string make, string model, int startYear, int endYear)
        {
            this.Make = make;
            this.Model = model;
            if (startYear > endYear) throw new ArgumentException("start larger than end");
            this.StartYear = startYear;
            this.EndYear = endYear;
        }

        // prefer over extension if you have access to CarModel (#3)
        //public bool IntervalsIntersect(int start, int end)
        //{
        //    return StartYear <= end && start <= EndYear;
        //}
    }

    public class CarModelMapper
    {
        public CarModelDto ToDto(CarModel carModel)
        {
            CarModelDto dto = new CarModelDto();
            dto.Make = carModel.Make;
            dto.Model = carModel.Model;
            dto.StartYear = carModel.StartYear;
            dto.EndYear = carModel.EndYear;
            return dto;
        }

        public CarModel FromDto(CarModelDto dto)
        {
            return new CarModel(dto.Make, dto.Model, dto.StartYear, dto.EndYear);
        }
    }

    public class CarModelDto
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
    }
}

