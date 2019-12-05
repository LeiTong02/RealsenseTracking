# -*- coding: utf-8 -*-
"""
Created on Sun Nov 10 23:03:21 2019

@author: hpsin
"""

import pandas as pd
import os
import sys
import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import mpl_toolkits.mplot3d.art3d as art3d
from matplotlib.patches import Ellipse
def plot_lissa_3D(csv,dir_path,direct_name):
    
    non_nan_acce= np.nan_to_num(csv.acceleration,copy=True)
    fig = plt.figure(figsize=(10,8))
    ax = fig.gca(projection='3d')
    
    
    proj_X = csv.X_Proj
    proj_Y =csv.Y_Proj
    proj_Z  = csv.Z_Proj
    
    #proj_X = np.append(proj_X,proj_X[0])
    #proj_Y = np.append(proj_Y,proj_Y[0])
    #proj_Z = np.append(proj_Z,proj_Z[0])
    #non_nan_acce = np.append(non_nan_acce,non_nan_acce[0])
    
    
    ells=[]
    first_acc = 0;
    speed_color= ['#30f522','#ff0000']
    default_color = speed_color[0]
    
    for z,y,acc in zip(proj_Z,proj_Y,non_nan_acce):
        ## #30f522  #ff0000
        if acc>first_acc and abs(acc-first_acc)>10:
            default_color = speed_color[0]
        elif acc<first_acc and abs(acc-first_acc)>10:
            default_color = speed_color[1]
        ells.append(Ellipse((z, y), 80, 0.1,edgecolor=default_color, lw=1, facecolor='none'))
        first_acc=acc      
     
    #a = plt.subplot(111)
     
    for e,x in zip(ells,proj_X):
      e.set_clip_box(ax.bbox)
      ax.add_patch(e)
      art3d.pathpatch_2d_to_3d(e, z=x, zdir="y")
    
    ax.set_xlim(proj_Z.min()-100, proj_Z.max()+100)
    ax.set_ylim(proj_X.min()-0.01, proj_X.max()+0.01)
    ax.set_zlim(proj_Y.min()-0.01, proj_Y.max()+0.01)
    ax.set_xlabel('Depth')
    ax.set_ylabel('Proj X')
    ax.set_zlabel('Proj Y')
    
    ax.plot(proj_Z,proj_X,proj_Y)
    ax.legend()
    # plt.xlim(proj_Z.min()-100, proj_Z.max()+100)
    # plt.ylim(proj_X.min()-0.01, proj_X.max()+0.01)
    #plt.zlim(proj_Y.min()-0.05, proj_Y.max()+0.05)
    plt.gca().invert_xaxis()
    #plt.gca().invert_yaxis()
    plt.gca().invert_zaxis()
    plt.savefig(dir_path+'/Lissajous_3D_{0}.png'.format(direct_name))

def plot_lissa_2D(csv,dir_path,direct_name):
   
    proj_X = csv.X_Proj
    proj_Y =csv.Y_Proj
    proj_Z  = csv.Z_Proj
    non_nan_acce= np.nan_to_num(csv.acceleration,copy=True)
    
    #proj_X = np.append(proj_X,proj_X[0])
    #proj_Y = np.append(proj_Y,proj_Y[0])
    #proj_Z = np.append(proj_Z,proj_Z[0])
    #non_nan_acce = np.append(non_nan_acce,non_nan_acce[0])
    
    fig = plt.figure(figsize=(10,8))
    
    
    ells=[]
    first_acc = 0;
    speed_color= ['#30f522','#ff0000']
    default_color = speed_color[0]
    
    for z,y,acc in zip(proj_Z,proj_Y,non_nan_acce):
        ## #30f522  #ff0000
        if acc>first_acc and abs(acc-first_acc)>10:
            default_color = speed_color[0]
        elif acc<first_acc and abs(acc-first_acc)>10:
            default_color = speed_color[1]
        ells.append(Ellipse((z, y), 80, 0.1,edgecolor=default_color, lw=1, facecolor='none'))
        first_acc=acc  
     
    a = plt.subplot(111)
     
    for e in ells:
      e.set_clip_box(a.bbox)
      
      a.add_artist(e)
     
    plt.xlabel('Depth')
    plt.ylabel('Proj Y')
    
    plt.plot(proj_Z,proj_Y,color='#0044ff')
    
    plt.xlim(proj_Z.min()-100, proj_Z.max()+100)
    plt.ylim(proj_Y.min()-0.05, proj_Y.max()+0.05)
    plt.gca().invert_xaxis()
    plt.gca().invert_yaxis()
    plt.savefig(dir_path+'/Lissajous_2D_{0}.png'.format(direct_name))
    
def func(path,direct_name):
    print('start!')
    print(path)
    txt_path = path+'/series_{0}.txt'.format(direct_name)
    file_name = txt_path.split('/')[-1]
    print(file_name)
    csv_path = os.path.dirname(txt_path) + '/series_{0}.csv'.format(direct_name)
    csv = pd.read_csv(txt_path,delimiter=',',header=None,names=['X_Real ','Y_Real',  'Z_Real ','X_Proj','Y_Proj' ,'Z_Proj','time_interval','acceleration'])
    csv.to_csv(csv_path)
    print ('save csv successfully')
    plot_lissa_3D(csv,path,direct_name)
    plot_lissa_2D(csv,path,direct_name)
    
    
    return "Generate successfully"

if __name__ == '__main__':
    print(func(sys.argv[1],sys.argv[2]))