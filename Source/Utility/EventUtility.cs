using System;
using System.ComponentModel;

namespace Utility
{
    /// <summary>
    /// Utility methods for Events, such as firing safely.
    /// </summary>
    public static class EventUtility
    {
        /// <summary>
        /// Safely fires an event with no <see cref="EventArgs" />.
        /// </summary>
        /// <param name="unsafEventHandler">The event handler to safely fire.</param>
        /// <param name="sender">The class that sent the event.</param>
        public static void SafeFireEvent(EventHandler unsafEventHandler, object sender)
        {
            EventHandler eventCopy = unsafEventHandler;

            eventCopy?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Safely fires an event with type <see cref="EventArgs" />.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="EventArgs" /> to fire.</typeparam>
        /// <param name="unsafeEventHandler">The event to safely fire.</param>
        /// <param name="sender">The class that called the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> the event has.</param>
        public static void SafeFireEvent<T>(EventHandler<T> unsafeEventHandler, object sender, T eventArgs) where T : EventArgs
        {
            EventHandler<T> eventCopy = unsafeEventHandler;

            eventCopy?.Invoke(sender, eventArgs);
        }

        public static void SafeFireEvent(PropertyChangedEventHandler unsafEventHandler, object sender, string propertyName)
        {
            PropertyChangedEventHandler eventCopy = unsafEventHandler;

            eventCopy?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}