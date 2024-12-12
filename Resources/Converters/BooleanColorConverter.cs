using System.Globalization;

namespace QuizzingApp341.Resources.Converters;
public class BooleanColorConverter : IValueConverter {
    public Color? FalseColor { get; set; }
    public Color? TrueColor { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (FalseColor == null && TrueColor == null && parameter is string param) {
            string[] colorStrings = param.Replace(" ", "").Split(',');
            if (colorStrings.Length == 2) {
                bool first = Color.TryParse(colorStrings[0], out var trueColor);
                bool second = Color.TryParse(colorStrings[1], out var falseColor);
                if (first && second) {
                    return (value as bool?) ?? false ? trueColor : falseColor;
                }
            }
        }
        return (value as bool?) ?? false ? TrueColor : FalseColor;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value == TrueColor;
    }
}
