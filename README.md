# CS4700 Game Development Final Project
Please update your availabilities for the next meeting once a week here:

https://huangruoqi.github.io/better_doodle/
## Current Progress:
* Have a cube with legs that can jump once and move on the platform freely
* Main Camera follows all character movement
* Add GameManager to change levels (currently if you jump you change level)
* Make sure to create a custom level/scene when testing out stuff, don't mess with existing scene if someone else is working on it because it will cause a merge conflict. A level/scene should only be manipulated by one branch, and don't work on the wrong branch :(
    * To create custom scene: 
        * choose a scene -> <ctrl+d> or <command+d>
        * rename the duplicated scene to `level#`
        * double click on the new scene
        * use menu **File->Build Settings** and add your scenes to the list (click **Add Open Scenes** for each scene)
        * change the highest level in GameManager.cs


