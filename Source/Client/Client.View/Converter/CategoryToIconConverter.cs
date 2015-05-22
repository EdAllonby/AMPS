using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Shared.Domain;

namespace Client.View.Converter
{
    /// <summary>
    /// Converts a <see cref="TaskCategory" /> to an icon.
    /// </summary>
    internal sealed class CategoryToIconConverter : IValueConverter
    {
        /// <summary>
        /// Coverts a <see cref="TaskCategory"/> into a static resource icon.
        /// </summary>
        /// <param name="value">The Category to convert.</param>
        /// <param name="targetType">No specified target type.</param>
        /// <param name="parameter">No parameter.</param>
        /// <param name="culture">No culture.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TaskCategory taskCategory = (TaskCategory) value;

            if (taskCategory.Equals(TaskCategory.Guitar))
            {
                return Application.Current.Resources["GuitarIcon"];
            }
            if (taskCategory.Equals(TaskCategory.Bass))
            {
                return Application.Current.Resources["BassIcon"];
            }
            if (taskCategory.Equals(TaskCategory.Drums))
            {
                return Application.Current.Resources["DrumsIcon"];
            }
            if (taskCategory.Equals(TaskCategory.Vocals))
            {
                return Application.Current.Resources["MicrophoneIcon"];
            }
            if (taskCategory.Equals(TaskCategory.Synth))
            {
                return Application.Current.Resources["SynthIcon"];
            }
            if (taskCategory.Equals(TaskCategory.Mixing))
            {
                return Application.Current.Resources["KnobIcon"];
            }
            if (taskCategory.Equals(TaskCategory.Other))
            {
                return Application.Current.Resources["MusicIcon"];
            }
            if (taskCategory.Equals(TaskCategory.Project))
            {
                return Application.Current.Resources["ProjectIcon"];
            }

            return Application.Current.Resources["MusicIcon"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}