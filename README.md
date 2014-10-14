TalkTime
========

Unity3D script to animate a 2D GameObject based on real-time audio clip amplitude

Easy to use. Just attach the TalkTime script to a GameObject, and then add references to the GameObjects representing the mouth frames. The frames represent 0 to Max audio amplitude. I'm using four here in the demo, but it should work with as many frames as you'd like.

With some hacking, you could modify this to do anything, not just flip through frames. Go nuts. And send me any bug fixes or modifications. 

TODO/KNOWN ISSUES
=================
* Modify the TalkTime script to use a smaller "window" array instead of allocating all the samples. As it's current implemented, AudioClip.GetData is duplicating the entire clip in memory, when I only need a subset.  This is easy to do, but I'm done with this for the evening. ;)

DEMO
=================
Here's a demo of it in action: http://www.bytestemplar.com/misc/unity/talktime/

<img src="http://i.imgur.com/RmoRpXA.png"/>
