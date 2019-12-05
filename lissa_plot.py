# -*- coding: utf-8 -*-
"""
Created on Sun Nov 17 15:53:42 2019

@author: hpsin
"""

import pandas as pd
import os
import sys

def func():
    print('start!')
    path = 'C:/Users/hpsin/Desktop/Lissajous'
    txt_path = path+'/series.txt'
    file_name = txt_path.split('/')[-1]
    print(file_name)
    csv_path = os.path.dirname(txt_path) + '/series.csv'
    csv = pd.read_csv(txt_path,delimiter=',',header=None,names=['X_Real ',' Y_Real',  'Z_Real ',' X_Proj ','Y_Proj' ,'Z_Proj','time_interval','acceleration'])
    csv.to_csv(csv_path)
    
    return "Save successfully"

if __name__ == '__main__':
    func()