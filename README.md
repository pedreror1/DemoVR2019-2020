# DemoVR2019-2020
 Unity VR Sample  2019-2020
 
 Interactions:
 * Shoot Bats from your gun using the Index Trigger on your left Hand
 * Kill Incoming Ghosts with your sword on the Right Hand
 *Toggle the VR Console with the Right Hand A button
 *Clear the VR Console Messages with  the Right Hand B Button

 Descriptions
 *The Custom Collision Detection uses my own Implementation of a Quad Tree to avoid the massive overhead of Unity Physics system when using hundreds of Collision
  with this The collision is only calculated for the Elements close to the Pawn, which are the objects connected to the slimes using yellow lines

  *The sword is calculated by checking the start and end position of the sword in each LateUpdate and then using a Catmull rom spline system the vertex positions are calculated, then with the help of an alpha texture I could achieve the sword slash effect


  *The Pool System is a generic pool system that can be used for any kind of object, as you have to create a new class for each pool and objectType I decided to automize this by adding a custom button to the GameObject Menu

  *The vertex shaders are just simple implementations of shaders using the shader graph


  *Debugging in VR is bothersome as you need to take off the headset to look in the editor log so I created a simple VR Console that displays all the logs so that you can easily realize if there is a problem or a message being dispatched while continue in the headset

  
 
Sample Project With some Interesting Demos for VR using UNITY HDRP
![Imgur Image](http://i.imgur.com/7Oelk0B.gif)



Mesh Vertex Displacment Shader to give Animation to an static mesh (Bat Flying Animation)
<br/>	<img src="/Gifs/batshaderSmall.gif?raw=true" width="200px">


Scanlines and Distort Shader Using Vertex Displacment
![Imgur Image](https://imgur.com/7Oelk0B.gif)

Inverse Kinematics System to control a Virtual Character with full body Interactions(Hands and Head Tracking)
<br/>	<img src="/Gifs/IKsmall.gif?raw=true" width="200px">


Pool System Generator Custom Editor Actions to generate Pool System Classes on Runtime
![Imgur Image](https://imgur.com/Xrbqo1m.gif)


Quad Tree Implementation For Custom Collision Detection
![Imgur Image](https://imgur.com/7Oelk0B.gif)


Sword Slash Effect Run Time Generation of Mesh to create a more vivid Sword Slash Effect
<br/>	<img src="/Gifs/swordslashsmall.gif?raw=true" width="400px">


VRConsole  In Game VR console to make the Debugging in VR Easier
<br/>	<img src="/Gifs/vrconsole.gif?raw=true" width="200px">
