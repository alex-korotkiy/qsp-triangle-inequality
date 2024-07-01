﻿using AlgorithmsOptimization.Algorithms;
using AlgorithmsOptimization.Measurements;
using AlgorithmsOptimization.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Testers
{
    public class AlgorithmTester<T> where T: IBaseAlgorithm, new()
    {
        public void MergeMetricsFromRun(Dictionary<string, List<double>> totalMetrics, Dictionary<string, double> metrics)
        {
            foreach (var key in metrics.Keys)
            {
                if (!totalMetrics.ContainsKey(key) || totalMetrics[key] == null) totalMetrics[key] = new List<double>();
                totalMetrics[key].Add(metrics[key]);
            }
        }

        public Dictionary<string, List<double>> Test(AlgorithmParams algorithmParams, TestParams testParams)
        {
            var result = new Dictionary<string, List<double>>();

            for (var i = 0; i < testParams.Repeats; i++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var algorithm = new T();
                algorithm.Init(algorithmParams);
                algorithm.FitTransform();
                stopwatch.Stop();

                var ticks = stopwatch.Elapsed.Ticks;
                var metrics = algorithm.GetMetrics();
                metrics[Metrics.RunTime] = ticks * 1.0 / Stopwatch.Frequency;

                MergeMetricsFromRun(result, metrics);
            }

            return result;
        }
    }
}
