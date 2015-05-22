using System.Windows.Media;

namespace Client.Model
{
    /// <summary>
    /// The colour palette for User colours.
    /// </summary>
    public static class UserColours
    {
        /// <summary>
        /// The colour for a connected member.
        /// </summary>
        public static Brush ConnectedColour
        {
            get { return ColourPalette.GoodColour; }
        }

        /// <summary>
        /// The colour for a disconnected member.
        /// </summary>
        public static Brush DisconnectedColour
        {
            get { return ColourPalette.BadColour; }
        }

        /// <summary>
        /// The colour for a member of an unkown state (should never be displayed).
        /// </summary>
        public static Brush UnknownColour
        {
            get { return ColourPalette.UnknownColour; }
        }
    }
}