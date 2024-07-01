using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math.Distances;
using AlgorithmsOptimization.Dto;
using AlgorithmsOptimization.Measurements;
namespace AlgorithmsOptimization.Algorithms
{
    public class BaseAlgorithm: IBaseAlgorithm
    {
        protected double[] scores;
        protected bool[] outliersMap;
        protected Dictionary<string, double> metrics = new Dictionary<string, double>();

        public double[][] X { get; set; }
        public double OutliersCount { get; set; }
        public int SamplesCount { get; set; }
        public IDistance<double[]> Distance { get; set; }

        public virtual int[] SampleIndexes { get; set; }

        public virtual void Init(AlgorithmParams algorithmParams)
        {
            X = algorithmParams.X;
            OutliersCount = algorithmParams.OutliersCount;
            SamplesCount = algorithmParams.SamplesCount;
            Distance = algorithmParams.Distance;

            if (algorithmParams.SampleIndexes == null)
            {
                SampleIndexes = GenerateSamples(SamplesCount, X.Length);
            }
            else
            {
                SampleIndexes = algorithmParams.SampleIndexes;
            }

        }

        public BaseAlgorithm()
        {

        }

        public BaseAlgorithm(AlgorithmParams algorithmParams)
        {
            Init(algorithmParams);
        }

        public virtual double[] Scores
        {
            get
            {
                return scores;
            }
        }

        public virtual int[] GenerateSamples(int samplesCount, int totalCount)
        {
            var resultSet = new HashSet<int>();
            var random = new Random();
            while(resultSet.Count < samplesCount)
            {
                var index = random.Next(0, totalCount);
                if (!resultSet.Contains(index)) resultSet.Add(index);
            }

            return resultSet.ToArray();
        }

        public virtual double CalculateScore(int index)
        {
            var result = double.PositiveInfinity;

            foreach (var sampleIndex in SampleIndexes)
            {
                if (index != sampleIndex)
                {
                    var distance = Distance.Distance(X[index], X[sampleIndex]);
                    if (distance < result) result = distance;
                }
            }

            return result;
        }

        public virtual bool[] FitTransform()
        {
            var length = X.Length;
            bool[] result = new bool[length];

            var outliersNumber = OutliersCount;

            int count = 0;

            if (outliersNumber >= 1)
                count = (int)Math.Round(outliersNumber); 
            else
                count = (int)Math.Round(outliersNumber*length);

            
            var scoresWithIndexes = new Tuple<int, double>[length];
            
            for (var i = 0; i<length; i++)
            {
                var score = CalculateScore(i);
                var element = new Tuple<int, double>(i, score);
                scoresWithIndexes[i] = element;
            }

            scores = scoresWithIndexes.Select(e => e.Item2).ToArray();
            var outlierIndexes = scoresWithIndexes.OrderByDescending(e => e.Item2).Take(count).Select(e => e.Item1);
            
            foreach (var outlierIndex in outlierIndexes)
            {
                result[outlierIndex] = true;
            }

            return result;
        }

        public virtual Dictionary<string, double> GetMetrics()
        {
            metrics[Metrics.DistanceCalculationsCount] = (X.Length - SamplesCount) * SamplesCount;
            return metrics;
        }
    }
}
