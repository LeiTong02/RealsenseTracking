
#main.py
import numpy as np
import sys


def multiplication(a,b):
	return a*b

def func(a,b):
    result=np.sqrt(multiplication(int(a),int(b)))
    np.save('./narry',result)
    return result
 
 
if __name__ == '__main__':
    print(func(sys.argv[1],sys.argv[2]))
