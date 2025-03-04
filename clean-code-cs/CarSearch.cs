namespace Victor.Training.Cleancode
{
    public class CarSearch
    {
        // run tests
        public List<CarModel> FilterCarModels(CarSearchCriteria criteria, List<CarModel> carModels)
        {
            List<CarModel> results = carModels
                .Where(carModel => criteria.YearInterval.Intersects(carModel.YearInterval))
                .ToList();
            Console.WriteLine("More filtering logic ...");
            return results;
        }
    }

    public class SomeOtherClientCode
    {
        private void ApplyLengthFilter() // pretend
        {
            Console.WriteLine(new Interval(1000, 1600).Intersects(new Interval(1250, 2000)));
        }

        private void ApplyCapacityFilter() // pretend
        {
            Console.WriteLine(new Interval(1000, 1600).Intersects(new Interval(1250, 2000)));
        }
    }

    public readonly record struct Interval
    {
        public int Start { get; }
        public int End { get; }
        public Interval(int start, int end)
        {
            // Exceptions from constructors are SCARY for many teams.
            if (start > end) throw new ArgumentException("start larger than end");
            Start = start;
            End = end;
           
        }
        public bool Intersects(Interval other)
        {
            return Start <= other.End && other.Start <= End;
        }
    }

    public class CarSearchCriteria // a DTO received from JSON
    {
        public int StartYear { get; }
        public int EndYear { get; }
        public string Make { get; }

        public CarSearchCriteria(int startYear, int endYear, string make)
        {
            this.Make = make;
            
            this.StartYear = startYear;
            this.EndYear = endYear;
        }

        //private Interval _cached_interval;
        // PR rejected without a supporting benchmark to prove that TODAY this DOES bring a benefit
        // a) vs stale data if caching remote data, eg data from DB/API/FILE
        // b) measureable benefit in memoizing computations, eg Chess board state beating Kasparov

        // MUST be pure function: no side-effects, same result
        //_cached_interval= new Interval(StartYear, EndYear);
        internal Interval YearInterval => new Interval(StartYear, EndYear); // LOVE!!!!
    }

    public class CarModel // Domain Model
    {
        public long? Id { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        //public int StartYear { get; private set; }
        //public int EndYear { get; private set; }
        public Interval YearInterval { get; private set; }

        public CarModel(string make, string model, int startYear, int endYear)
        {
            this.Make = make;
            this.Model = model;
            if (startYear > endYear) throw new ArgumentException("start larger than end");
            //this.StartYear = startYear;
            //this.EndYear = endYear;
            this.YearInterval = new Interval(startYear, endYear);
        }
    }

    public class CarModelMapper
    {
        public CarModelDto ToDto(CarModel carModel)
        {
            CarModelDto dto = new CarModelDto();
            dto.Make = carModel.Make;
            dto.Model = carModel.Model;
            //dto.StartYear = carModel.StartYear;
            dto.StartYear = carModel.YearInterval.Start;
            dto.EndYear = carModel.YearInterval.End;
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

