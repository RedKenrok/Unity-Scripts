# Interface

Components that deal with the interface's behaviour.

## Reorderable

Allows you to have a list of items that can be reordered.

## Selectable

Creates a bar of selectable items that can be pressed.

## NonDrawingGraphics.cs

Function like a graphic component that can be clicked and raycasted onto, but is never visible.

## SafeArea.cs

Scales the rectTransform element it is attached to so it boundaries always fall withing the screens safe area to ensure interface elements do not fall behind a mobile devices notch.

> Make sure in the rect transform to set the "anchorMin" to 0, 0 and the "anchorMax" to 1, 1 and the left, right, bottom, and top to 0.