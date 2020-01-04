# Signals ❇

![UPM Screenshot](https://repository-images.githubusercontent.com/196998874/5b81aa00-0794-11ea-804b-4acc77a1ce2e)

### A typesafe, lightweight, tested messaging package for Unity.
---

## Installation

### Simple Download

[Latest Unity Packages](../../releases/latest)

### Unity Package Manager (UPM)

> You will need to have git installed and set in your system PATH.

Find `Packages/manifest.json` in your project and add the following:
```json
{
  "dependencies": {
    "com.supyrb.signals": "https://github.com/supyrb/signals.git#0.3.0",
    "...": "..."
  }
}
```

## Features

* Signal Hub as a global Registry for everyone to access
* Signal with up to three parameters
* Signal Listener Order
* Consuming Signals
* Pausing Signals
  
* Easy integration with UPM
* Unit tests to assure quality
* Sample packages to get started fast
* XML comments for all public methods and properties

## Usage

[BasicExample](./Samples~/Basic/Scripts/BasicExampleSignalTest.cs)

### Get Signal

```c#
exampleSignal = Signals.Get<BasicExampleSignal>();
```
or
```c#
Signals.Get(out exampleSignal);
```
or
```c#
exampleSignal = Signals.Get(typeof(BasicExampleSignal));
```

### Subscribe to Signal

```c#
// Default subscription with order 0
exampleSignal.AddListener(DefaultListener);
// Subscribe with an earlier order to be called first
exampleSignal.AddListener(FirstListener, -100);
```

### Dispatch Signal

```c#
exampleSignal.Dispatch();
```
### Pause & Continue

```c#
exampleSignal.Pause();
exampleSignal.Continue();
```
If you want to pause the further propagation of a signal (wait for a *user input*/*scene that needs to laod*/*network package*) you can easily do that with `signal.Pause()` and `signal.Continue()`.

### Consume Signals

```c#
exampleSignal.Consume();
```
Sometimes only one script should handle a signal or the signal should not reach others. Unity for example does this with keystrokes in the editor, you can decide in the script if the [event is used](https://docs.unity3d.com/ScriptReference/Event.Use.html). Similar to that, you can consume signals with `signal.Consume()`. Always be away of the order of your listeners. Listeners with a lower order value are called first and therefore decide before others if they should get the event as well.

## Contribute

Contributions to the repository are always welcome. There are several ways to contribute:  
* Creating issues with problems you found or ideas you have how to improve the project
* Solving existing issues with PRs
* Writing test cases to make sure everything is running the way it is supposed to run
* Create CI actions (e.g. run automated tests, trigger new version creation)
* Refactor / Cleanup code
* Document code functionality
* Write wiki entries
* Improve editor integration of signals

### Code Contribution

#### Setup

1. Create a new Unity Project
2. Clone git repository in your assets folder `C:\UnityProject\Assets> git clone hhtps://github.com/supyrb/signals.git`
3. Copy folder `UnityProject\Assets\Signals\Samples~` to `UnityProject\Assets\SignalSamples` in order to see/use the examples

#### Guidelines

* Use Tabs
* Use namespace `Supyrb`
* Use private fields with `[SerializeField]` when you need to expose fields in the editor
* Use [XML comments](https://docs.microsoft.com/en-us/dotnet/csharp/codedoc) for public methods and classes
* Follow the [Supyrb Guidelines](https://github.com/supyrb/SupyrbConventions) in your code.
* Use present tense git commits as described [here](https://github.com/supyrb/SupyrbConventions/tree/develop/git#commit-messages)

## Credits

* Built on the shoulders of [Signals](https://github.com/yankooliveira/signals) by [Yanko Oliveira](https://github.com/yankooliveira)
* Inspired by [JS-Signas](https://github.com/millermedeiros/js-signals) by [Miller Medeiros](https://github.com/millermedeiros)
* Developed by [Johannes Deml](https://github.com/JohannesDeml) – [public@deml.io](mailto:public@deml.io)

## License

* MIT - see [LICENSE](./LICENSE.md)

*![💥Supyrb](https://supyrb.com/data/supyrb-inline-logo.svg)*
