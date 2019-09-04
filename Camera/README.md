# Camera

Components adding functionality the standard camera component.

## CameraFieldOfView.cs

Ensure the field of view field of the camera component is applied to the horizontal axis if that one is shorter that the vertical axis. This creates the behaviour that there is a minimum field of view at all times.

## CameraLowResRenderer.cs

Using the unity's scriptable render pipeline this component allows a camera to be rendered in a lower resolution, then up-scaled to a higher resolution afterwards using a render texture. This component is great if you want to add a low resolution look and feel to a game.