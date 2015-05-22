using System;

namespace Shared.Domain
{
    /// <summary>
    /// Defines a <see cref="Task" />s category.
    /// </summary>
    [Serializable]
    public enum TaskCategory
    {
        /// <summary>
        /// The <see cref="Task" /> has work to do with Guitars.
        /// </summary>
        Guitar,

        /// <summary>
        /// The <see cref="Task" /> has work to do with Bass.
        /// </summary>
        Bass,

        /// <summary>
        /// The <see cref="Task" /> has work to do with Drums.
        /// </summary>
        Drums,

        /// <summary>
        /// The <see cref="Task" /> has work to do with Vocals.
        /// </summary>
        Vocals,

        /// <summary>
        /// The <see cref="Task" /> has work to do with Synths.
        /// </summary>
        Synth,

        /// <summary>
        /// The <see cref="Task" /> has work to do with Mixing.
        /// </summary>
        Mixing,

        /// <summary>
        /// The <see cref="Task" /> has work to do with the Project.
        /// </summary>
        Project,

        /// <summary>
        /// The <see cref="Task" /> has work to do with something else.
        /// </summary>
        Other
    }
}