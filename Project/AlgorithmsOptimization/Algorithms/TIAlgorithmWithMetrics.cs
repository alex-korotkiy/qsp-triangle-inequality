using Accord.Math.Distances;
using AlgorithmsOptimization.Dto;
using AlgorithmsOptimization.Measurements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Algorithms
{
    public class TIAlgorithmWithMetrics : TIAlgorithm, IBaseAlgorithm
    {
        protected long DistanceCalculations = 0;

        public TIAlgorithmWithMetrics() : base() { }

        public TIAlgorithmWithMetrics(AlgorithmParams algorithmParams) : base(algorithmParams) { }

        public override void CalculateSamplesData()
        {
            var iStopwatch = new Stopwatch();
            iStopwatch.Start();
            base.CalculateSamplesData();
            iStopwatch.Stop();
            metrics[Metrics.IndexingTime] = iStopwatch.ElapsedTicks * 1.0 / Stopwatch.Frequency;
        }

        public override double CalculateScore(int index)
        {
            var elementIndex = SampleIndexes[0];
            if (elementIndex == index) return SampleScores[0];

            var result = Distance.Distance(X[index], X[elementIndex]);
            DistanceCalculations++;
            int minSampleIndex = 0;

            for (int i = 1; i < SamplesCount; i++)
            {
                elementIndex = SampleIndexes[i];
                if (elementIndex == index) return SampleScores[i];
                if (TMatrix[minSampleIndex, i] >= result) continue;
                var newDistance = Distance.Distance(X[index], X[elementIndex]);
                DistanceCalculations++;
                if (newDistance < result)
                {
                    minSampleIndex = i;
                    result = newDistance;
                }
            }

            return result;
        }

        public override Dictionary<string, double> GetMetrics()
        {
            metrics[Metrics.DistanceCalculationsCount] = (SamplesCount - 1) * SamplesCount / 2 + DistanceCalculations;
            return metrics;
        }
    }
}
