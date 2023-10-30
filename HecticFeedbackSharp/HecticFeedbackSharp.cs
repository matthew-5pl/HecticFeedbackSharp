using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace HecticFeedbackSharp
{
    public static class Internal
    {
        // You shouldn't call this method directly, please use FeedbackPerformer instead!
        [DllImport("libSwiftHaptics", EntryPoint = "haptic_trigger")]
        public static extern void InternalHapticTrigger(int type);
    }

    // Feedback type, pretty much equivalent to:
    // https://developer.apple.com/documentation/appkit/nshapticfeedbackmanager/feedbackpattern
    public enum FeedbackType: int 
    {
        Generic = 0,
        Alignment,
        LevelChange
    }

    public static class DebugLogger
    {
        public static void LogInfo(object x)
        {
            Console.WriteLine("HecticFeedback/Debug/Info: " + x.ToString());
        }

        public static void LogError(object x)
        {
            Console.WriteLine("HecticFeedback/Debug/Error: " + x.ToString());
        }
    }

    // A class that can programmatically perform haptic feedback events.

    public class FeedbackPerformer
    {
        // Minimum delay in milliseconds between feedback.
        // Can be set to not stress the haptic motors too much.
        private long minDelay;
        // Time in ms elapsed since last feedback event.
        private long elapsed;
        // Timestamp where last feedback event occured.
        private long lastEvent;

        // Set the minimum delay (in milliseconds) between feedback events.
        public void SetMinDelay(long value)
        {
            minDelay = value;
        }

        // Get the minimum delay (in milliseconds) between feedback events.
        public long GetMinDelay()
        {
            return minDelay;
        }

        // Get the amount of time (in milliseconds) elapsed since last feedback event.
        public long GetElapsed()
        {
            return elapsed;
        }

        // Perform feedback.
        // @param type Type of feedback to send, changes haptic feel
        public void Perform(FeedbackType type)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            elapsed = now - lastEvent;

#if DEBUG
            DebugLogger.LogInfo("Now: " + now.ToString());
            DebugLogger.LogInfo("LastEvent: " + lastEvent.ToString());
            DebugLogger.LogInfo("Elapsed: " + elapsed.ToString());
#endif

            if(elapsed < minDelay)
            {
#if DEBUG
                DebugLogger.LogInfo("Can't trigger: Minimum delay is  " + minDelay.ToString() + "ms but only " + elapsed.ToString() + "ms have passed. ");
#endif
            }
            else
            {
                Internal.InternalHapticTrigger((int)type);
            }

            lastEvent = DateTimeOffset.UtcNow.ToUnixTimeSeconds()-minDelay;
        }

        // Wait in this thread for the set delay time.
        public void Wait()
        {
            Thread.Sleep((int)minDelay+1);
        }

        // Wait in this thread for the set delay time, then perform feedback.
        // Equivalent to calling Wait() and Perform().
        public void WaitAndPerform(FeedbackType type)
        {
            Wait();
            Perform(type);
        }

        // Constructor.
        public FeedbackPerformer()
        {
            // Default delay = 100ms
            minDelay = 100;
            elapsed = 0;
            lastEvent = DateTimeOffset.UtcNow.ToUnixTimeSeconds()-minDelay;
        }
    }

}

