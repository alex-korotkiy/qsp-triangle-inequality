using System;
using Microsoft.Data.Analysis;
using AlgorithmsOptimization.Extensions;
using AlgorithmsOptimization.Algorithms;
using System.Diagnostics;
using AlgorithmsOptimization.Dto;
using AlgorithmsOptimization.Testers;
using System.IO;
using System.Linq;
using AlgorithmsOptimization.Dto.Datasets;
using System.Collections.Generic;

namespace AlgorithmsOptimization
{
    class Program
    {

        static Dictionary<string, BaseDataset> DatasetStorage = new Dictionary<string, BaseDataset>(StringComparer.InvariantCultureIgnoreCase);

        static T RegisterDataset<T>() where T: BaseDataset, new()
        {
            var dataset = new T();
            DatasetStorage[dataset.Name] = dataset;
            return dataset;
        }

        static void RegisterDatasets()
        {
            RegisterDataset<ALOI_Dataset>();
            RegisterDataset<ForestCover_Dataset>();
            RegisterDataset<Optdigits_Dataset>();
            RegisterDataset<Shuttle_Dataset>();
            RegisterDataset<Satellite_Dataset>();
        }

        static void ProcessDataset(BaseDataset dataset, string outputPath)
        {
            var algorithmParams = new AlgorithmParams
            {
                X = dataset.GetX(),
                OutliersCount = dataset.OutliersCount
            };

            var testParams = new TestParams();

            var sampleCounts = new int[] { 10, 15, 20, 30, 50, 70, 100, 150, 200, 300, 500, 700, 1000 };
            //var sampleCounts = new int[] { 10, 20 };
            var repeats = sampleCounts.Reverse().ToArray();
            var comparer = new AlgorithmsComparer();

            var cResult = comparer.CompareForReports(algorithmParams, testParams, sampleCounts, repeats);
            var fullOutputFileName = Path.Combine(outputPath, dataset.OutputFileName);
            DataFrame.SaveCsv(cResult, fullOutputFileName);
        }

        static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("AlgorithmsOptimization OutputPath [DatasetName]");
                return;
            }

            var outputPath = args[0];
            RegisterDatasets();

            if (args.Length == 1)
            {
                foreach(var key in DatasetStorage.Keys)
                {
                    ProcessDataset(DatasetStorage[key], outputPath);
                }
            }
            else
            {
                var processKey = args[1];
                if(!DatasetStorage.ContainsKey(processKey))
                {
                    Console.WriteLine("DataSet name should be one of the following supported datasets:");
                    foreach (var key in DatasetStorage.Keys)
                    {
                        Console.WriteLine(key);
                    }
                    return;
                }
                ProcessDataset(DatasetStorage[processKey], outputPath);
            }
            
            
        }
    }
}
