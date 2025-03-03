using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Victor.Training.Cleancode
{
    public class CarSearchTest
    {
        private CarSearch searchEngine = new CarSearch();

        private CarModel fordFocusMk2 = new CarModel("Ford", "Focus", 2012, 2016);

        // Ford Focus:     [2012 ---- 2016]
        // Search:              [2014 ---- 2018]
        // can't afford a 2021 car
        [Test]
        public void ByYear_Match()
        {
            var criteria = new CarSearchCriteria(2014, 2018, "Ford");

            List<CarModel> models = searchEngine.FilterCarModels(criteria, new List<CarModel> { fordFocusMk2 });

            Assert.That(models, Has.Count.EqualTo(1));
        }

        [Test]
        public void ByYear_NoMatch()
        {
            var criteria = new CarSearchCriteria(2017, 2018, "Ford");

            List<CarModel> models = searchEngine.FilterCarModels(criteria, new List<CarModel> { fordFocusMk2 });

            Assert.That(models, Has.Count.EqualTo(0));
        }
    }
}

