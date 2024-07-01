using AlgorithmsOptimization.Algorithms;
using AlgorithmsOptimization.Dto;
using AlgorithmsOptimization.Extensions;
using AlgorithmsOptimization.Formatting;
using AlgorithmsOptimization.Measurements;
using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Testers
{
    public class AlgorithmsComparer
    {
        public static List<T3> ApplyFormula<T1, T2, T3>(List<T1> values1, List<T2> values2, Func<T1, T2, T3> formula)
        {
            var result = new List<T3>();
            var count = Math.Min(values1.Count, values2.Count);
            for (var i = 0; i < count; i++)
            {
                result.Add(formula(values1[i], values2[i]));
            }
            return result;
        }

        public static void CalculateDerivativeMetrics(Dictionary<string, List<double>>  metrics)
        {
            metrics[Metrics.TIPrefix + Metrics.IndexingTimePercent] = ApplyFormula(metrics[Metrics.TIPrefix + Metrics.IndexingTime], metrics[Metrics.TIPrefix + Metrics.RunTime], (x, y) => x * 100 / y);
            metrics[Metrics.TIPrefix + Metrics.RunTimePercent] = ApplyFormula(metrics[Metrics.TIPrefix + Metrics.RunTime], metrics[Metrics.BasePrefix + Metrics.RunTime], (x, y) => x * 100 / y);
            metrics[Metrics.TIPrefix + Metrics.DistanceCalculationsPercent] = ApplyFormula(metrics[Metrics.TIPrefix + Metrics.DistanceCalculationsCount], metrics[Metrics.BasePrefix + Metrics.DistanceCalculationsCount], (x, y) => x * 100 / y);
        }

        public static void MergeWithPrefix<T>(Dictionary<string, T> sourceDictionary, IEnumerable<string> sourceKeySet, string prefix, Dictionary<string, T> destinationDictionary)
        {
            foreach(var key in sourceKeySet)
            {
                if (!sourceDictionary.ContainsKey(key)) continue;
                var newKey = prefix + key;
                destinationDictionary[newKey] = sourceDictionary[key];
            }
        }

        public static Dictionary<string, double> CalculateAvgAndSigma(Dictionary<string, List<double>> originalData)
        {
            var result = new Dictionary<string, double>();
            foreach(var key in originalData.Keys)
            {
                double avg = 0;
                double sigma = 0;
                var values = originalData[key];
                if(values!=null && values.Count > 0)
                {
                    avg = values.Average();
                    if (values.Count > 1) sigma = Math.Sqrt(values.Select(x => (x - avg) * (x - avg)).Sum()/(values.Count - 1));
                }
                result[key + Metrics.AvgSuffix] = avg;
                result[key + Metrics.SigmaSuffix] = sigma;
            }
            return result;
        }

        public Dictionary<string, List<double>> CompareOnParams(AlgorithmParams algorithmParams, TestParams testParams)
        {
            var result = new Dictionary<string, List<double>>();

            var baseTester = new AlgorithmTester<BaseAlgorithm>();
            var baseResult = baseTester.Test(algorithmParams, testParams);

            var tiTester = new AlgorithmTester<TIAlgorithm>();
            var tiResult = tiTester.Test(algorithmParams, testParams);

            var tiTesterWithMetrics = new AlgorithmTester<TIAlgorithmWithMetrics>();
            var tiResultWithMetrics = tiTesterWithMetrics.Test(algorithmParams, testParams);

            MergeWithPrefix(baseResult, baseResult.Keys, Metrics.BasePrefix, result);
            MergeWithPrefix(tiResult, tiResult.Keys, Metrics.TIPrefix, result);
            MergeWithPrefix(tiResultWithMetrics, tiResultWithMetrics.Keys.Except(tiResult.Keys), Metrics.TIPrefix, result);

            CalculateDerivativeMetrics(result);

            return result;
        }

        public Dictionary<string, double> CompareOnParamsRow(AlgorithmParams algorithmParams, TestParams testParams)
        {
            var fullData = CompareOnParams(algorithmParams, testParams);
            var result = CalculateAvgAndSigma(fullData);
            result[Metrics.RunTimePercentAvgRatio] = result[Metrics.TIPrefix + Metrics.RunTime + Metrics.AvgSuffix] * 100 / result[Metrics.BasePrefix + Metrics.RunTime + Metrics.AvgSuffix];
            return result;
        }

        public DataFrame CompareForReports(AlgorithmParams baseAlgorithmParams, TestParams baseTestParams, int[] samplesCounts, int[] repeats)
        {
            var dicts = new List<Dictionary<string, double>>();
            for(var i=0; i<Math.Min(samplesCounts.Length, repeats.Length); i++)
            {
                var algorithmParams = baseAlgorithmParams.Clone();
                algorithmParams.SamplesCount = samplesCounts[i];
                var testParams = baseTestParams.Clone();
                testParams.Repeats = repeats[i];
                var rowResult = CompareOnParamsRow(algorithmParams, testParams);
                dicts.Add(rowResult);
            }

            var columnNames = dicts[0].Keys;

            List<DataFrameColumn> columnsList = new List<DataFrameColumn>();
            columnsList.Add(new Int32DataFrameColumn(Metrics.Samples, samplesCounts));
            foreach (var columnName in columnNames)
            {
                var columnData = new double[samplesCounts.Length];
                for (var i = 0; i<samplesCounts.Length; i++)
                {
                    columnData[i] = dicts[i][columnName];
                }
                columnsList.Add(new DoubleDataFrameColumn(columnName, columnData));
            }

            return new DataFrame(columnsList);
        }

        public static string CommonSigmaString(double avgValue, double sigmaValue, double sigmaFactor)
        {
            var pmValue = sigmaValue * sigmaFactor;
            return avgValue.ToString() + "±" + pmValue.ToString();
        }


    }
}
