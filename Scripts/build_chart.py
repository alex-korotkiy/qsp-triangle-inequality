import matplotlib.pyplot as plt
from matplotlib import ticker
import numpy as np
import pandas as pd

import os
import sys

def get_script_path():
    return os.path.dirname(os.path.realpath(sys.argv[0]))


df = pd.read_csv(get_script_path() + "/../Output/sat.csv")

# Create some mock data
samples = df['Samples'].to_numpy()
base_time = df['Base_RunTime_Avg'].to_numpy()
ti_time = df['TI_RunTime_Avg'].to_numpy()
time_ratio =  df['RunTimePercentAvgRatio'].to_numpy()
distance_calculations_ratio = df['TI_DistanceCalculationsPercent_Avg'].to_numpy();

diag_color = 'black'
base_time_color = 'red'
ti_time_color = 'green'
time_ratio_color = 'blue'
distance_calculations_ratio_color = 'magenta'

fig, ax1 = plt.subplots()

ax1.set_xscale('log')
ax1.set_xlabel('# of samples')
ax1.set_xticks(samples)
ax1.get_xaxis().set_major_formatter(ticker.ScalarFormatter())
ax1.set_ylabel('Execution time, s', color=diag_color)
l1 = ax1.plot(samples, base_time, color=base_time_color, label='Base algorithm execution time, s')
l2 = ax1.plot(samples, ti_time, color=ti_time_color, label='Optimized algorithm execution time, s')
ax1.tick_params(axis='y', labelcolor=diag_color)

ax2 = ax1.twinx()  # instantiate a second Axes that shares the same x-axis


ax2.set_ylabel('%', color=diag_color)  # we already handled the x-label with ax1
l3 = ax2.plot(samples, time_ratio, time_ratio_color, label = 'Optimized/base execution time ratio, %')
l4 = ax2.plot(samples, distance_calculations_ratio, distance_calculations_ratio_color, label = 'Optimized/base distance calculations ratio, %')
ax2.tick_params(axis='y', labelcolor=diag_color)

#fig.tight_layout()  # otherwise the right y-label is slightly clipped

ax1.legend(loc='upper center', bbox_to_anchor=(0.5, 1.17), ncol=2, frameon = False)
ax2.legend(loc='upper center', bbox_to_anchor=(0.5, 1.09), ncol=2, frameon = False)

plt.show()