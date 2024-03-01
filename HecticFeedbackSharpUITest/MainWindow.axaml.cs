using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using HecticFeedbackSharp;

namespace HecticFeedbackSharpUITest;

public partial class MainWindow : Window
{
    FeedbackPerformer performer;
    public MainWindow()
    {
        InitializeComponent();
        performer = new FeedbackPerformer();
        performer.SetMinDelay(50);

        testSlider.ValueChanged += SliderChange;

        testButton.PointerEntered += PointerEnterEventGeneric;
        testButton2.PointerEntered += PointerEnterEventAlignment;
        testButton3.PointerEntered += PointerEnterEventLevelChange;
    }

    private void PointerEnterEventGeneric(object? sender, PointerEventArgs e)
    {
        Task.Run(() => performer.AsyncPerform(FeedbackType.Generic));
    }
    
    private void PointerEnterEventAlignment(object? sender, PointerEventArgs e)
    {
        Task.Run(() => performer.AsyncPerform(FeedbackType.Alignment));
    }

    private void PointerEnterEventLevelChange(object? sender, PointerEventArgs e)
    {
        Task.Run(() => performer.AsyncPerform(FeedbackType.LevelChange));
    }

    private void SliderChange(object? sender, RangeBaseValueChangedEventArgs e)
    {
        Task.Run(() => performer.AsyncPerform(FeedbackType.Alignment));
    }
}