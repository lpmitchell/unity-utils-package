# Unity helper utilities

## Installation

In the Unity Package Manager window, click the "+" button at the top-left and select 
"Add package from git URL"

Enter the following URL:

`https://github.com/lpmitchell/unity-utils-package.git`

## Documentation (WIP)

*Collection extensions:*
Adds methods to do common things to arrays or lists:
- `myGameObjectArray.AllSetActive(false);`
- `myGameObjectArray.AllDestroy()`
- `myBehaviourArray.AllSetEnabled(false);`
- `myArrayOrList.Swap(indexA, indexB)`
- `var randomElement = myArrayOrList.RandomElement()`
- `if(myArrayOrList.IsNullOrEmpty()){ ... }`

*Unity inspector:*
New attribute to add a single Layer selector as an int field, because Unity has nothing built-in to do that
`[Layer] public int MyLayerProperty`

*Camera:*
- Methods to find the Orthographic size or screen-space rectangle of a given `Bounds` object
- `myCamera.BoundsToScreenRect(bounds);`
- `myCamera.orthographicSize = myCamera.BoundsToOrthographicSize(bounds);`

*MonoBehaviour extensions:*
- Method to destroy the behaviour or it's enclosing GameObject in a set number of seconds:
- `myRigidbody.DestroyBehaviourInSeconds(5f);`
- `myRigidbody.DestroyGameObjectInSeconds(10f);`

*Coroutine helpers:*
- `WaitForContinue` class:
```cs
var hold = new WaitForContinue();
// continue can now be passed to something else, like a UI panel
yield return hold; // halt execution of this coroutine until we can continue
```

The above coroutine will continue when you call `hold.Continue();`

- `WaitForResult<T>` class:
  -  This is like WaitForContinue but you can set a return value to be passed back to the coroutine

*Patterns*
- `Boxed<T>` is a simple generic class to box a value-type, so you can do something like:
```cs
var myBoxedInt = new Boxed<int>(5);
otherThing.referencedInt = myBoxedInt;
// otherThing will be able to see this change also:
myBoxedInt.Value = 6;
// also you can listen for changes:
myBoxedInt.Changed += onIntChanged;
```

*Tween library*
I have a _really_ simple tween library (59 lines of code) in there also

*Logging*
There's a debug logging system that gets fully stripped on release builds and passes messages out to the browser when built for WebGL, but it's pretty specific really. It does do some nice things like automatically showing the caller of the logging, but I probably wouldn't use it until it's more fleshed out