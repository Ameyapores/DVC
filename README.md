# DVC
This is the codebase for colonoscopy navigation using end-to-end deep visuomotor control. This repository contains the Unity scene used to implement autonomous control for colonoscopy. 

<img src="images/video.gif" width="300"> 

## Prerequisites
mlagents 0.16.1
tensorflow 2.3.0
tensorboard 2.7.0

## How to run

To run this demo you need to have Unity-64bit (has been tested on version 2019.4.13f1) installed on your system.
Afterwards, you have to:
1. Clone or download this repo.
2. Start Unity.
3. Select 'Open Project' and select the root folder that you have just cloned.
4. Press 'Play' button to run a sample trajectory.

## How to train
The endoscopic agents can be trained in two ways. First, an external python script containing the DRL agent can be interfaced with unity using [gym_unity](https://github.com/Unity-Technologies/ml-agents/blob/main/gym-unity/README.md). Second, [MlAgents toolkit](https://github.com/Unity-Technologies/ml-agents) can be used provide Unity technologies. We present the second way here:

Follow the instruction on this page to start the training.: [Training mlagents](https://github.com/Unity-Technologies/ml-agents/blob/release_2_docs/docs/Training-ML-Agents.md)
