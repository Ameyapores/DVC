import Sofa 
import numpy as np
import os
import sys
import yaml
import random
import math
import SofaPyUtils_3 as sp 
import SofaPython.Tools

class control(Sofa.PythonScriptController):
    def initGraph(self,node):

        #initialization
        self.dt=node.findData('dt').value
        self.time=node.findData('time').value
        self.node=node
        self.ColonState=node.getObject('MechColon')

        global savedData
        savedData=sp.ComponentData_txt("Unity_1.txt", node, self.ColonState, ['position'])

        return 0

    def onBeginAnimationStep(self,dt):
        #Data saving    
        savedData.writeData()

        return 0

    def cleanup(self):
		savedData.closeFile()
		return 0

    def onEndAnimationStep(self, dt):
        return 0

    def onKeyPressed(self,k):
		return 0 