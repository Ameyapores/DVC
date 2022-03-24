# DVC
This is the codebase for colonoscopy navigation using end-to-end deep visuomotor control. This repository contains the Unity scene used to implement autonomous control for colonoscopy. 

<img src="images/video.gif" width="300"> 

## Prerequisites
- mlagents 0.16.1
- tensorflow 2.3.0
- tensorboard 2.7.0
- SofaAPAPI-Unity3D Plugin v1.1 (for deformable behaviour of the colon)

## How to run

To run this demo you need to have Unity-64bit (has been tested on version 2019.4.13f1) installed on your system.
Afterwards, you have to:
1. Clone or download this repo.
2. Start Unity.
3. Select 'Open Project' and select the root folder that you have just cloned.
4. Press 'Play' button to run a sample trajectory.

## How to train
The endoscopic agents can be trained in two ways. First, an external python script containing the DRL agent can be interfaced with unity using [gym_unity](https://github.com/Unity-Technologies/ml-agents/blob/main/gym-unity/README.md). We present an implementation of PPO based on stable_baseline3 python package.
```
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
from gym_unity.envs import UnityToGymWrapper
from gym.wrappers import Monitor
import os
from stable_baselines3 import PPO

def main():
    monitor_dump_dir = os.path.join(os.path.dirname(__file__), os.pardir, 'gym_monitor')
    channel = EngineConfigurationChannel()
    unity_env = UnityEnvironment('built_scene/colonoscopy', side_channels=[channel])
    channel.set_configuration_parameters(time_scale = 4)
    env = UnityToGymWrapper(unity_env, uint8_visual=True)
    env = Monitor(env, monitor_dump_dir, allow_early_resets=True)

    model = PPO("CnnPolicy", env, verbose=1)
    model.learn(total_timesteps=1000000)
    model.save("unity_model")
    
    model = PPO.load("unity_model")

    obs = env.reset()
    while True:
        action, _states = model.predict(obs)
        obs, rewards, dones, info = env.step(action)
        env.render()

if __name__ == '__main__':
    main()
```


Second, [MlAgents toolkit](https://github.com/Unity-Technologies/ml-agents) can be used provide Unity technologies. We present the second way here:

Follow the instruction on this page to start the training.: [Training mlagents](https://github.com/Unity-Technologies/ml-agents/blob/release_2_docs/docs/Training-ML-Agents.md). Following are the steps:
```
git clone https://github.com/Unity-Technologies/ml-agents/tree/release_2
cd ml-agents-release_2
mlagents-learn config/trainer_config.yaml --run-id training
```
Press 'Play' button from the Unity editor to start training.
The training can be visualised using tensorboard
```
cd ml-agents-release_2
tensorboard --logdir summaries --port 6006
```
