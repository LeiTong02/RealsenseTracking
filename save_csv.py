# -*- coding: utf-8 -*-
"""
Created on Sun Nov 10 23:03:21 2019

@author: hpsin
"""

import pandas as pd
import os
import sys

def func(path,direct_name):
    print('start!')
    print(path)
    txt_path = path+'/series_{0}.txt'.format(direct_name)
    file_name = txt_path.split('/')[-1]
    print(file_name)
    csv_path = os.path.dirname(txt_path) + '/series_{0}.csv'.format(direct_name)
    csv = pd.read_csv(txt_path,delimiter=',',header=None,names=['X_Real','Y_Real',  'Z_Real','X_Proj','Y_Proj' ,'Z_Proj','time_interval','acceleration'])
    csv.to_csv(csv_path)
    
    return "Save successfully"

if __name__ == '__main__':
    print(func(sys.argv[1],sys.argv[2]))