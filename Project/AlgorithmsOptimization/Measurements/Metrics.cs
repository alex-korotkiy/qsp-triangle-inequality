using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Measurements

{
    public static class Metrics
    {
        public const string RunTime = "RunTime";
        public const string RunTimePercent = "RunTimePercent";
        public const string DistanceCalculationsCount = "DistanceCalculationsCount";
        public const string DistanceCalculationsPercent = "DistanceCalculationsPercent";
        public const string IndexingTime = "IndexingTime";
        public const string IndexingTimePercent = "IndexingTimePercent";
        public const string Samples = "Samples";

        public const string BasePrefix = "Base_";
        public const string TIPrefix = "TI_";

        public const string AvgSuffix = "_Avg";
        public const string SigmaSuffix = "_Sigma";

        public const string RunTimePercentAvgRatio = "RunTimePercentAvgRatio";
    }
}
