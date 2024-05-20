using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace HecticFeedbackSharp
{
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

        [DllImport("libSwiftHaptics", EntryPoint = "haptic_trigger")]
        private static extern void InternalHapticTrigger(int type);

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
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

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
                Thread.Sleep((int)(minDelay - elapsed));
                InternalHapticTrigger((int)type);
            }
            else
            {
                InternalHapticTrigger((int)type);
            }

            lastEvent = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        // Perform Feedback Asynchronously
        #pragma warning disable CS1998
        public async Task PerformAsync(FeedbackType type) 
        {
            Perform(type);
        }
        #pragma warning restore CS1998

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

