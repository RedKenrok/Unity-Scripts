# Animation

A simple animation system for when the build-in animation is to much for what you want to achieve and when a much simpler system works too.

## SimpleAnimator.cs

The core of the animation system. This object takes all the animations (added as sister components) and animates the given target object when the "Play" function is called.

> Do note this component uses the [UniRx](https://github.com/neuecc/UniRx) library, however the component could be changed to use the standard Unity co-routines.

## ISimpleAnimation.cs

A simple interface used by all animations, this is the interface the "SimpleAnimator" is looking for.

## Animation[...].cs

The animations themself, each inheriting the ISimpleAnimation interface. Examples of animations included animating the position, rotation, or scale of object, and the transparency or color of a graphics component.