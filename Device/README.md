# Device

Static and globally accessable classes that deal with the device.

## Resolution.cs

A static and globally accessable class that keep track of the screens resolution and has an event that can be listened to for when it changes.

> Do note this component uses the [UniRx](https://github.com/neuecc/UniRx) library, however this script could be changed to use the standard Unity co-routines. If rewritten to use Unity's standard co-routines then it would also become a component, and not a static and globally accessable class.