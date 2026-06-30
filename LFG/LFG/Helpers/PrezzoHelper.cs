using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LFG.Helpers;

public static class PrezzoHelper
{
    /// <summary>
    /// Parses a canonical price string (dot decimal, no thousands separator).
    /// Returns 0 on null/empty. Never uses thread culture.
    /// </summary>
    public static decimal Parse(string? prezzo)
    {
        if (string.IsNullOrWhiteSpace(prezzo))
            return 0m;

        if (decimal.TryParse(prezzo, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
            return result;

        return 0m;
    }

    /// <summary>
    /// Formats a decimal to canonical string: dot decimal, two places, no thousands.
    /// Example: 1200m -> "1200.00"
    /// </summary>
    public static string ToCanonico(decimal valore) =>
        valore.ToString("F2", CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts a raw, potentially ambiguous price string to canonical format.
    /// Disambiguation rule: the decimal separator is the LAST '.' or ',' in the string,
    /// but ONLY if it is followed by exactly 1 or 2 digits. Everything before it has
    /// its separators stripped (they are thousands separators).
    /// If no valid decimal separator is found the whole string is treated as an integer.
    /// Idempotent: a string already in canonical form is returned unchanged.
    /// </summary>
    public static string NormalizzaRaw(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return "0.00";

        raw = raw.Trim();

        // Find last '.' or ',' followed by exactly 1 or 2 digits at end of string.
        var match = Regex.Match(raw, @"[.,](\d{1,2})$");

        if (match.Success)
        {
            var decPart = match.Groups[1].Value.PadRight(2, '0'); // "9" -> "90"
            var intRaw  = raw[..match.Index];                      // everything before sep
            var intPart = Regex.Replace(intRaw, @"[.,]", "");     // strip thousands seps
            var canonical = $"{intPart}.{decPart}";

            if (decimal.TryParse(canonical, NumberStyles.AllowDecimalPoint,
                                  CultureInfo.InvariantCulture, out var val))
                return ToCanonico(val);
        }
        else
        {
            // No decimal separator (or sep followed by 3+ digits = thousands only)
            var digits = Regex.Replace(raw, @"[.,]", "");
            if (decimal.TryParse(digits, NumberStyles.None,
                                  CultureInfo.InvariantCulture, out var intVal))
                return ToCanonico(intVal);
        }

        // Unreachable for well-formed strings; return raw to surface the anomaly in logs.
        return raw;
    }
}
