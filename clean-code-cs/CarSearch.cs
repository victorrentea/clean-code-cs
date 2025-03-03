namespace Victor.Training.Cleancode
{
    public class CarSearch
    {
        // run tests
        public List<CarModel> FilterCarModels(CarSearchCriteria criteria, List<CarModel> carModels)
        {
            List<CarModel> results = carModels
                .Where(carModel => MathUtil.IntervalsIntersect(
                    criteria.StartYear, criteria.EndYear,
                    carModel.StartYear, carModel.EndYear))
                .ToList();
            Console.WriteLine("More filtering logic ...");
            return results;
        }
    }

    public class SomeOtherClientCode
    {
        private void ApplyLengthFilter() // pretend
        {
            Console.WriteLine(MathUtil.IntervalsIntersect(1000, 1600, 1250, 2000));
        }

        private void ApplyCapacityFilter() // pretend
        {
            Console.WriteLine(MathUtil.IntervalsIntersect(1000, 1600, 1250, 2000));
        }
    }

    public class MathUtil
    {
        public static bool IntervalsIntersect(int start1, int end1, int start2, int end2)
        {
            return start1 <= end2 && start2 <= end1;
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
            if (startYear > endYear) throw new ArgumentException("start larger than end");
            this.StartYear = startYear;
            this.EndYear = endYear;
        }
    }

   public class CarModel // Domain Model
    {
        public long? Id { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        public int StartYear { get; private set; }
        public int EndYear { get; private set; }

         public CarModel(string make, string model, int startYear, int endYear)
        {
            this.Make = make;
            this.Model = model;
            if (startYear > endYear) throw new ArgumentException("start larger than end");
            this.StartYear = startYear;
            this.EndYear = endYear;
        }
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

