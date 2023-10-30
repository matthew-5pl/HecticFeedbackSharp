# HecticFeedbackSharp
A C# port of [HecticFeedback](https://github.com/matthew-5pl/HecticFeedback).

Allows you to trigger Mac trackpads' haptic feedback programmatically!

# Basic example
```cs
using HecticFeedbackSharp;

FeedbackPerformer performer = new FeedbackPerformer();
performer.WaitAndPerform(FeedbackType.Generic);
```