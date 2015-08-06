using System.Windows.Media;

namespace Client.Model
{
    /// <summary>
    /// The colour palette for the view/view model.
    /// </summary>
    public static class ColourPalette
    {
        /// <summary>
        /// The colour for somethng good.
        /// </summary>
        public static Brush GoodColour => Brushes.LimeGreen;

        /// <summary>
        /// The colour for something bad.
        /// </summary>
        public static Brush BadColour => Brushes.Red;

        /// <summary>
        /// The colour for a guitar.
        /// </summary>
        public static Brush GuitarColour => Brushes.PowderBlue;

        /// <summary>
        /// The colour for a bass.
        /// </summary>
        public static Brush BassColour => Brushes.PaleVioletRed;

        /// <summary>
        /// The colour for a drum.
        /// </summary>
        public static Brush DrumColour => Brushes.MediumSeaGreen;

        /// <summary>
        /// The colour for a vocal.
        /// </summary>
        public static Brush VocalColour => Brushes.LightSteelBlue;

        /// <summary>
        /// The colour for a synth.
        /// </summary>
        public static Brush SynthColour => Brushes.MediumPurple;

        /// <summary>
        /// The colour for mixing.
        /// </summary>
        public static Brush MixingColour => Brushes.IndianRed;

        /// <summary>
        /// The colour for other.
        /// </summary>
        public static Brush OtherColour => Brushes.CornflowerBlue;

        /// <summary>
        /// The colour for a project.
        /// </summary>
        public static Brush ProjectColour => Brushes.SandyBrown;

        /// <summary>
        /// The colour for an unknown state.
        /// </summary>
        public static Brush UnknownColour => Brushes.DeepPink;
    }
}