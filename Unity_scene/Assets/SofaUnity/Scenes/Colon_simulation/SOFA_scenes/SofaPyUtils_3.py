import numpy as np
import os
import sys
import yaml
import random
import math
import json
#from pyquaternion import Quaternion


class ComponentData_txt:
    """ Read/Write component data to txt file 
    """
    def __init__(self, filename=None, node=None, component=None, dataList=[]):

        self._filename = filename
        self.component = component #component to access
        self.DataList = dataList #List of datafield to save inside the component
        self.Node = node #node to access
        print "class component created"

        if self._filename is None:
            self._filename = self.component.name+".txt"
        #self.file=open(_filename, 'a') #open file in append mode
        #self.file.write("TimeStep[s]   Position[m] \n")

        open(self._filename,'a') 

    def writeData(self):
        #self.file.write(self.component.findData[0].value)
        #print self.component.findData(0).value
        #print "Enter write data"

        componentData=dict()
        for d in self.DataList:
            componentData[d] = self.component.findData(d).value

        _filename = self._filename

        with open(_filename,'a') as file:
            json.dump(componentData, file, indent=4)
        #print "[ComponentDataIO]: component:", self.component.name, "data written to:", _filename

        # time=self.Node.getTime()
        # position=self.component.findData(self.DataList[0]).value[89:99]
        # force=self.component.findData(self.DataList[1]).value[89:99]

        # for x in range(10):
        #     self.file.write("\n %f  " % (time))
        #     for item in position[x]:
        #         self.file.write("%s " % (item))
        #     self.file.write("       ")
        #     for item in force[x]:
        #         self.file.write("%s " % (item))

    def closeFile (self):
        print "close file"
        self.file.close()