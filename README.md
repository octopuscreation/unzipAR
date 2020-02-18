# unzipAR_bundleGenerator

### Website : http://octopuscreation.com/augmented-reality.html

## Unity version: 2019.2.18

This project was made to generate compatible bundles for unzipAR application

All the configuration will be done with the 2 configurations files 'BundleConfig' and 'CharacterConfig'.

# Settings

## BundleConfiguration file

Location: Resources/BundleConfig.asset

![alt text](https://github.com/octopuscreation/unzipAR_bundleGenerator/blob/master/git-images/bundleConfig.png)

This file is used to know:

1. <b>Bundle Name</b>: The name of the generated bundle 'Name_android.unity3d' or 'Name_ios.unity3d' example : skeleton (don't need to add anything else)

2. <b>Target Model Folder</b>: Your model folder placed in 'Characters' folder

3. <b>Bundle Platform</b>: Target platform to build your bundle (only android or ios)

It will generate the bundle .unity3d at this path 'Characters_bundles/your_bundle_name/your_bundle_name.android'

## CharacterConfig file

Location: Resources/CharacterConfig.asset

This configuration file is used only to generate the json file for the character configuration (example -> 'Characters/Knight/knight_config.json)

/!\ This file need to exist and to be added in your bundle folder in order to have a 'valid' bundle to be used in the unzipAR application.

![alt text](https://github.com/octopuscreation/unzipAR_bundleGenerator/blob/master/git-images/character_config_skeleton_example.png)

1. <b>Prefab</b>: Your model prefab

2. <b>Animations Infos</b>:\
  a. <b>Name</b>: Animation name (name in your animator controller).\
  b. <b>Sprite</b>: Animation image in order to display them to select your animation in game. (if no image given, it will display his name)\
  c. <b>AudioClip</b>: Sound that will be played when the animation with 'Name' is playing.\
  

# Generate them all

Now that we have our configuration files ready and our model folder in 'Characters' folder.

![alt text](https://github.com/octopuscreation/unzipAR_bundleGenerator/blob/master/git-images/assetBundle_dropdown.png)

A. <b>Generate configuration file</b>:
  Normally, it will be added in your character folder, otherwise put it in.

B. <b>Generate bundle</b>:
This will generate the bundle.unity3d file in 'Characters_bundles/' folder

C. <b>Check bundle</b>:
It will tell you if your bundle is valid for unzipAR application.

## Test

There is a script 'BundleLoader' in the scene 'bundleGenerator'. When you hit 'Play' it will load your generated bundle in the scene. (bundle target in the 'BundleConfig' file in 'Characters_bundles/your_bundle_name/' folder.

## IMPORTANT

On iOS, Sometimes your bundles won't charge because iOS has a limit on the memory usage of your phone. 
I therefore advise you to make bundles with a maximum size of 100-200 MB depending on the memory of your phone. This could happen only if you try to load the 3D model from your phone, The other technique is to put your bundle on a server and download it from a url. Memory usage is much lower
