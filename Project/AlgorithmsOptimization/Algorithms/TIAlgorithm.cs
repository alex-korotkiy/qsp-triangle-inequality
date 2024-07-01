using Accord.Math.Distances;
using AlgorithmsOptimization.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Algorithms
{
    public class TIAlgorithm : BaseAlgorithm, IBaseAlgorithm
    {
        protected double[,] TMatrix;
        protected double[] SampleScores;

        public TIAlgorithm(): base() { }

        public TIAlgorithm(AlgorithmParams algorithmParams): base(algorithmParams) { }

        public override int[] SampleIndexes
        {
            get
            {
                return base.SampleIndexes;
            }
            set
            {
                base.SampleIndexes = value;
                CalculateSamplesData();
            }
        }

        public virtual void CalculateSamplesData()
        {
            TMatrix = new double[SamplesCount, SamplesCount];
            SampleScores = new double[SamplesCount];

            for (var i = 0; i < SamplesCount; i++)
            {
                SampleScores[i] = double.PositiveInfinity;
            }

            for (var i = 0; i < SamplesCount; i++)
            {
                for(var j = i + 1; j < SamplesCount; j++)
                {
                    var distance = Distance.Distance(X[SampleIndexes[i]], X[SampleIndexes[j]]);
                    if (distance < SampleScores[i]) SampleScores[i] = distance;
                    if (distance < SampleScores[j]) SampleScores[j] = distance;
                    TMatrix[i, j] = distance/2;
                }
            }
        }

        public override double CalculateScore(int index)
        {
            var elementIndex = SampleIndexes[0];
            if (elementIndex == index) return SampleScores[0];

            var result = Distance.Distance(X[index], X[elementIndex]);
            int minSampleIndex = 0;

            for(int i = 1; i < SamplesCount; i++)
            {
                elementIndex = SampleIndexes[i];
                if (elementIndex == index) return SampleScores[i];
                if (TMatrix[minSampleIndex, i] >= result) continue;
                var newDistance = Distance.Distance(X[index], X[elementIndex]);
                if(newDistance < result)
                {
                    minSampleIndex = i;
                    result = newDistance;
                }
            }

            return result;
        }

        public override Dictionary<string, double> GetMetrics()
        {
            return metrics;
        }

    }
}
