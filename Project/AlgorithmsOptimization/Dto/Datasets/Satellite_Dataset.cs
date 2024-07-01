﻿using AlgorithmsOptimization.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Dto.Datasets
{
    public class Satellite_Dataset : BaseDataset
    {

        public Satellite_Dataset()
        {
            inputFileName = "sat_normalized.csv";
            outputFileName = "sat.csv";
        }

        public override string Name
        {
            get
            {
                return "Satellite";
            }
        }

        public override double OutliersCount
        {
            get
            {
                return 2644;
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
