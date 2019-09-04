# Player

Components that deal with the players behaviour.

## PlayerNavigation.cs

A navigation script that allows a player character to be controlled across a surface without needing physics, instead it uses a the navmesh surface to move across. This means jumping is not possible, but allows level geometry to be simplied as invisible borders to prevent a player character from falling of the level are not required.

> Recommended to be used in combination with Unity [NavMeshComponents](https://github.com/Unity-Technologies/NavMeshComponents)'s NavMeshSurface.