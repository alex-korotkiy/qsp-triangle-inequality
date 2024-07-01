﻿using AlgorithmsOptimization.Extensions;
using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOptimization.Dto.Datasets
{
    public class ForestCover_Dataset : BaseDataset
    {

        public ForestCover_Dataset()
        {
            inputFileName = "covtype_normalized.csv";
            outputFileName = "covtype.csv";
        }

        protected override DataFrame ReadFile()
        {
            return DataFrame.LoadCsv("Data\\" + inputFileName, ',', true);
        }

        public override string Name
        {
            get
            {
                return "ForestCover";
            }
        }

        public override double OutliersCount
        {
            get
            {
                return 2747;
            }
        }

        public override double[][] GetX()
        {
            var df = ReadFile();
            for(var i=df.Columns.Count-1; i>=10; i--)
            {
                df.Columns.RemoveAt(i);
            }
            
            return df.ToDoubleArray();
        }
    }
}
