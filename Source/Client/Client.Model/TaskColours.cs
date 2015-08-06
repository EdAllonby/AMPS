using System.Windows.Media;

namespace Client.Model
{
    /// <summary>
    /// The colour palette for User colours.
    /// </summary>
    public static class TaskColours
    {
        /// <summary>
        /// The colour for a connected member.
        /// </summary>
        public static Brush CompletedTaskColour => ColourPalette.GoodColour;

        /// <summary>
        /// The colour for a disconnected member.
        /// </summary>
        public static Brush UncompletedTaskColour => ColourPalette.BadColour;
    }
}