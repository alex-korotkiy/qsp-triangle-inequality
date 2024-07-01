using AlgorithmsOptimization.Algorithms;
using AlgorithmsOptimization.Dto;
using AlgorithmsOptimization.Extensions;
using Microsoft.Data.Analysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsOptimizationTests
{
    [TestClass]
    public class AlgorithmsLogicTests
    {
        protected AlgorithmParams algorithmParams;

        public AlgorithmsLogicTests()
        {
            var df = DataFrame.LoadCsv("Data\\ALOI_norm.csv", ',', false);
            var colCount = df.Columns.Count;
            df.Columns.RemoveAt(colCount - 1);
            df.Columns.RemoveAt(colCount - 2);

            var x = df.ToDoubleArray();
            algorithmParams = new AlgorithmParams
            {
                X = x,
                OutliersCount = 1508
            };
            
            var genAlgorithm = new BaseAlgorithm();
            algorithmParams.SampleIndexes = genAlgorithm.GenerateSamples(algorithmParams.SamplesCount, x.Length);
        }

        /*

        [TestMethod]
        public void CompareScores()
        {
            var baseAlgorithm = new BaseAlgorithm(algorithmParams);
            var tiAlgorithm = new TIAlgorithm(algorithmParams);


            var s1 = baseAlgorithm.CalculateScore(1);
            var s2 = tiAlgorithm.CalculateScore(1);

            Assert.AreEqual(s1, s2);
        }
        */
        
        [TestMethod]
        public void TestThatAllAlgorithmsDetermineScoresAndOutliersTheSameWay()
        {
            // Arrange
            var baseAlgorithm = new BaseAlgorithm(algorithmParams);
            var tiAlgorithm = new TIAlgorithm(algorithmParams);
            var tiAlgorithmWithMetrics = new TIAlgorithmWithMetrics(algorithmParams);

            // Act
            var baseAlgorithmOutliersMap = baseAlgorithm.FitTransform();
            var tiAlgorithmOutliersMap = tiAlgorithm.FitTransform();
            var tiAlgorithmWithMetricsOutliersMap = tiAlgorithmWithMetrics.FitTransform();


            var baseAlgorithmScores = baseAlgorithm.Scores;
            var tiAlgorithmScores = tiAlgorithm.Scores;
            var tiAlgorithmWithMetricsScores = tiAlgorithmWithMetrics.Scores;

            // Assert
            Assert.AreEqual(baseAlgorithmOutliersMap.Length, algorithmParams.X.Length);
            Assert.AreEqual(tiAlgorithmOutliersMap.Length, algorithmParams.X.Length);
            Assert.AreEqual(tiAlgorithmWithMetricsOutliersMap.Length, algorithmParams.X.Length);
            

            Assert.AreEqual(baseAlgorithmScores.Length, algorithmParams.X.Length);
            Assert.AreEqual(tiAlgorithmScores.Length, algorithmParams.X.Length);
            Assert.AreEqual(tiAlgorithmWithMetricsScores.Length, algorithmParams.X.Length);


            for (var i = 0; i < baseAlgorithmOutliersMap.Length; i++)
            {
                Assert.AreEqual(baseAlgorithmOutliersMap[i], tiAlgorithmOutliersMap[i]);
                Assert.AreEqual(baseAlgorithmOutliersMap[i], tiAlgorithmWithMetricsOutliersMap[i]);

                Assert.AreEqual(baseAlgorithmScores[i], tiAlgorithmScores[i]);
                Assert.AreEqual(baseAlgorithmScores[i], tiAlgorithmWithMetricsScores[i]);
            }
        }
        
    }
}
