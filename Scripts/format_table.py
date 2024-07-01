import math
import numpy as np
import pandas as pd
import os
import sys


def round_to_n_significant_figures(num, n):
    if num == 0:
        return 0
    else:
        return np.sign(num)*round(num, -int(math.floor(math.log10(abs(num)))) + (n - 1))

def get_dot_sep():
    fnum = f"{0.1}"
    for sym in fnum:
        if sym != "0" and sym != "1":
            return sym
    return ""

def round_and_format(num, n):
    dot_sep = get_dot_sep()

    if num == 0:
        if n == 1:
            return "0"
        else:
            return "0." + "0"*(n-1)

    rounded_num = round_to_n_significant_figures(num, n)
    formatted_num = f"{rounded_num:.{n}g}"
    
    dotSepPresent = False
    digitsNumber = 0
    digitsStarted = False

    for sym in formatted_num:
        if(sym == dot_sep):
            dotSepPresent = True

        if (sym.isnumeric()):
            if sym != "0":
                digitsStarted = True
            
            if (digitsStarted):
                digitsNumber+=1

    if (digitsNumber < n):
        if (not dotSepPresent):
            formatted_num = formatted_num + dot_sep
        
        formatted_num = formatted_num + "0"*(n-digitsNumber)

    return formatted_num

def round_for_report(num):
    return round_to_n_significant_figures(num, 4)

def format_for_report(num):
    return round_and_format(num, 4)

vround_for_report = np.vectorize(round_for_report)
vformat_for_report = np.vectorize(format_for_report)

def get_script_path():
    return os.path.dirname(os.path.realpath(sys.argv[0]))

def prepare_formatted_table (result_csv, formatted_csv):
    df = pd.read_csv(result_csv)

    base_runtime = vround_for_report(df["Base_RunTime_Avg"])
    opt_runtime = vround_for_report(df["TI_RunTime_Avg"])

    base_runtime_f = vformat_for_report(df["Base_RunTime_Avg"])
    opt_runtime_f = vround_for_report(df["TI_RunTime_Avg"])

    runtime_ratio = vformat_for_report((opt_runtime/base_runtime)*100)
    dc_ratio = vformat_for_report(df["TI_DistanceCalculationsPercent_Avg"])
    ind_time_percent = vformat_for_report(df["TI_IndexingTimePercent_Avg"])

    dff = pd.DataFrame({
        '# of samples': df["Samples"],
        '# of distance calculations (base algorithm)': df["Base_DistanceCalculationsCount_Avg"],
        'Base algorithm execution time, s': base_runtime_f,
        'Optimized algorithm execution time, s': opt_runtime_f,
        'Optimized/base execution time ratio, %': runtime_ratio,
        'Optimized/base distance calculations ratio, %': dc_ratio,
        'Optimized algorithm indexing time/total execution time ratio, %': ind_time_percent

    })

    dff.to_csv(formatted_csv, index=False, header=True)
    
base_path = get_script_path()
prepare_formatted_table(base_path + "/../Output/ALOI.csv", base_path + "/../Formatted/ALOI_formatted.csv")
prepare_formatted_table(base_path + "/../Output/covtype.csv", base_path + "/../Formatted/covtype_formatted.csv")
prepare_formatted_table(base_path + "/../Output/Optdigits.csv", base_path + "/../Formatted/Optdigits_formatted.csv")
prepare_formatted_table(base_path + "/../Output/sat.csv", base_path + "/../Formatted/sat_formatted.csv")
prepare_formatted_table(base_path + "/../Output/Shuttle.csv", base_path + "/../Formatted/Shuttle_formatted.csv")
