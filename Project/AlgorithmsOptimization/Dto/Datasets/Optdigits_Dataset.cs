using AlgorithmsOptimization.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Dto.Datasets
{
    public class Optdigits_Dataset : BaseDataset
    {

        public Optdigits_Dataset()
        {
            inputFileName = "Optdigits_normalized.csv";
            outputFileName = "Optdigits.csv";
        }

        public override string Name
        {
            get
            {
                return "Optdigits";
            }
        }

        public override double OutliersCount
        {
            get
            {
                return 150;
            }
        }

        public override double[][] GetX()
        {
            var df = ReadFile();
            var colCount = df.Columns.Count;
            df.Columns.RemoveAt(colCount - 1);

            return df.ToDoubleArray();
        }
    }
}
