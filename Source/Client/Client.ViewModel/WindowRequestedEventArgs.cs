using System;
using Shared.Domain;

namespace Client.ViewModel
{
    /// <summary>
    /// Packages objects needed for when a new Window is requested in the View Model.
    /// </summary>
    public sealed class WindowRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Packages a <see cref="Task" /> which is needed for the context of a new Window.
        /// </summary>
        /// <param name="task">The <see cref="Task" /> to package in the <see cref="EventArgs" />.</param>
        public WindowRequestedEventArgs(Task task)
        {
            Task = task;
        }

        /// <summary>
        /// Packages a <see cref="Band" /> which is needed for the context of a new Window.
        /// </summary>
        /// <param name="band">The <see cref="Band" /> to package in the <see cref="EventArgs" />.</param>
        public WindowRequestedEventArgs(Band band)
        {
            Band = band;
        }

        /// <summary>
        /// The <see cref="Band" /> packaged.
        /// </summary>
        public Band Band { get; private set; }

        /// <summary>
        /// The <see cref="Task" /> packaged.
        /// </summary>
        public Task Task { get; private set; }
    }
}