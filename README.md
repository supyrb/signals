# Signals ❇

![Signals Editor Window Screenshot](Documentation~/signals-preview-screenshot.png)

### A typesafe, lightweight, tested messaging package for Unity.  
[![openupm](https://img.shields.io/npm/v/com.supyrb.signals?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.supyrb.signals/)  [![](https://img.shields.io/github/release-date/supyrb/signals.svg)](https://github.com/supyrb/signals/releases)  [![Unity 5.6 or later](https://img.shields.io/badge/unity-5.6%20or%20later-blue.svg?logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)  [![Tested up to Unity 2020.2](https://img.shields.io/badge/tested%20up%20to%20unity-2020.2-green.svg?logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)

## Installation

Install the package with [OpenUPM](https://openupm.com/)

```sh
$ openupm add com.supyrb.signals
```

or download the [Latest Unity Packages](../../releases/latest)

## Features

* Signal Hub as a global Registry for everyone to access
* Signal with up to three parameters
* Signal Listener Order
* Consuming Signals
* Pausing Signals

As well as
* An editor window
  * A filterable list of all signals in the project
  * Dispatch signals with custom parameters
  * A signal log for each signal
  * List of all subscribed listeners for a signal
* An editor console window
  * Similar to Unity's console, just for signals
  * Options to clear console on enter playmode, script recompile or after builds

* Easy integration with UPM
* Unit tests for runtime scripts
* Sample packages to get started fast
* XML comments for all public methods and properties
* GC Free dispatching and very small memory footprint

## Usage

[BasicExample](./Samples~/Basic/Scripts/BasicExampleSignalTest.cs)

### Get Signal

```c#
BasicExampleSignal exampleSignal;

// Get the signal by passing its variable
Signals.Get(out exampleSignal);
// or with a generic get
exampleSignal = Signals.Get<BasicExampleSignal>();
// or by passing the type
exampleSignal = Signals.Get(typeof(BasicExampleSignal));
```
### Subscribe to Signal

```c#
//if you didn't store the Signal in a variable you can use
Signals.Get<BasicExampleSignal>().AddListener(DefaultListener);

// Default subscription with order 0
exampleSignal.AddListener(DefaultListener);

// Subscribe with an earlier order to be called first
exampleSignal.AddListener(FirstListener, -100);
```
### Unsubscribe to Signal

```c#
//if you didn't store the Signal in a variable you can use
Signals.Get<BasicExampleSignal>().RemoveListener(DefaultListener);

// Unsubscribe does not care for the listening order
exampleSignal.RemoveListener(DefaultListener);

```

### Dispatch Signal

```c#
// Send the signal to all listeners (if not consumed or paused in between)
exampleSignal.Dispatch();
```
### Pause & Continue

```c#
// No more listeners will be called until the signal is continued
exampleSignal.Pause();

// Will continue after the listener that paused the signal
exampleSignal.Continue();
```
If you want to pause the further propagation of a signal (wait for a *user input*/*scene that needs to load*/*network package*) you can easily do that with `signal.Pause()` and `signal.Continue()`.

### Consume Signals

```c#
// No more listeners will receive this signal
exampleSignal.Consume();
```
Sometimes only one script should handle a signal or the signal should not reach others. Unity for example does this with keystrokes in the editor, you can decide in the script if the [event is used](https://docs.unity3d.com/ScriptReference/Event.Use.html). Similar to that, you can consume signals with `signal.Consume()`. Always be away of the order of your listeners. Listeners with a lower order value are called first and therefore decide before others if they should get the event as well.

## Editor Window

The editor window can be accessed through `Window->Signals->Singals`. On the first start and whenever you added a signal you want to debug, just hit the refresh button in the bottom right corner of the window

* In the top right corner there is a search field which you can use to filter your signals
* Click on a signal in the list, to see the details of that signal

### Detail View

* You can dispatch, consume, pause and continue the signal. For dispatching you will get the needed fields to enter your custom arguments. Most common types are supported, for non supported types you will see an information.
* The log shows a history of all dispatched of that signal with a timestamp of the real time all well as the game time of that dispatch
* The Listeners list shows all subscribed methods to that signal sorted with their order. Additionally the listeners are colored if the last dispatch is at a certain listener
  * Green: The signal is currently running at this listener
  * Yellow: The signal was paused at this listener
  * Red: The signal was consumed at this listener

## Caveats

* Each signal is only lazily instanced once (nice!). This results in reusing the same signal over and over. While in general this is great, you will have to remember that fact while using the signals. Consider the following case:
  Signal A has Listeners 1,2,3 and 4.
  Signal A gets dispatched.

  - 1 processes the event
  - 2 processes the event and decides to dispatch A again
  - 1 processes the event
  - 2 processes the event
  - 3 processes the event
  - 4 processes the event

  In this case 1 and 2 got called two times, but 3 and 4 only one time. This might be wanted, but might also be unexpected.

* When using the signal editor window, you will get a GC for every signal of 48bytes. This only happens if the window is open and won't happen in the build or if there is no window open.



## Contribute

Contributions to the repository are always welcome. There are several ways to contribute:  
* [Create an issue](../../issues) for a problem you found or an idea on how to improve the project
* Solve existing issues with PRs
* Write test cases to make sure everything is running the way it is supposed to run
* Create CI actions (e.g. run automated tests, trigger new version creation)
* Refactor / Cleanup code
* Document code functionality
* Write wiki entries
* Improve editor integration of signals
* Post your thoughts in the [Unity Forum](https://forum.unity.com/threads/open-source-signals-a-decoupled-typesafe-messaging-system.803487/)

### Code Contribution

#### Setup

1. Create a new Unity Project
2. Clone git repository in your assets folder `C:\UnityProject\Assets> git clone hhtps://github.com/supyrb/signals.git`
3. Get access to the samples
   1. Simple way: Copy folder `UnityProject/Assets/Signals/Samples~` to `UnityProject/Assets/SignalSamples` in order to see/use the examples.
   2. Hard way: Run a command prompt as admin and go to the Assets folder of the project, then run `mklink /D SignalSamples .\signals\Samples~`


#### Guidelines

* Use Tabs
* Use namespace `Supyrb`
* Use private fields with `[SerializeField]` when you need to expose fields in the editor
* Use [XML comments](https://docs.microsoft.com/en-us/dotnet/csharp/codedoc) for public methods and classes
* Follow the [Supyrb Guidelines](https://github.com/supyrb/SupyrbConventions) in your code.
* Use present tense git commits as described [here](https://github.com/supyrb/SupyrbConventions/tree/develop/git#commit-messages)
* Use reflection for the editor window to access data from the signals, do not change field visibility just for editor tools.

## Credits

* Built on the shoulders of [Signals](https://github.com/yankooliveira/signals) by [Yanko Oliveira](https://github.com/yankooliveira)
* Inspired by [JS-Signas](https://github.com/millermedeiros/js-signals) by [Miller Medeiros](https://github.com/millermedeiros)
* Thanks to [Benny Berger](https://github.com/Asorano) for introducing me to the pattern
* Thanks to [Bryan Keiren](https://github.com/bkeiren) for [SerializableDateTime](./Scripts/Runtime/Logging/Libraries/SerializableDateTime.cs)
* Developed by [Johannes Deml](https://github.com/JohannesDeml) – [public@deml.io](mailto:public@deml.io)

## License

* MIT - see [LICENSE](./LICENSE.md)

*![💥Supyrb](https://supyrb.com/data/supyrb-inline-logo.svg)*
