using AlgorithmsOptimization.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Algorithms
{
    public interface IBaseAlgorithm
    {
        public void Init(AlgorithmParams algorithmParams);
        public bool[] FitTransform();
        public Dictionary<string, double> GetMetrics();
    }
}
