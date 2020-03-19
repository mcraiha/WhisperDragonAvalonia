using System;
using Avalonia.Data.Converters;
using System.Globalization;

namespace WhisperDragonAvalonia
{
	public class BoolToSecureIconStringConverter : IValueConverter
	{
		private static readonly string secureIcon = "🔐";
		private static readonly string unsecureIcon = "❌";

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() == typeof(bool))
			{
				if ((bool)value)
				{
					return secureIcon;
				}
				else
				{
					return unsecureIcon;
				}
			}

			throw new InvalidOperationException("BoolToSecereIconStringConverter can only convert bool value types");
		}
	
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string)
            {
                string stringValue = (string)value;
                if (stringValue == secureIcon)
                {
                    return true;
                }
                else if (stringValue == unsecureIcon)
                {
                    return false;
                }
            }

            return null;
		}
	}
}