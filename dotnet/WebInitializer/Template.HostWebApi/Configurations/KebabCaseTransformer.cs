using System.Text.RegularExpressions;

namespace Template.HostWebApi.Configurations;

public partial class KebabCaseTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        string? valueString = value?.ToString();
        return string.IsNullOrEmpty(valueString)
            ? valueString
            : KebabCaseRegex().Replace(valueString, "$1-$2").ToLowerInvariant();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex KebabCaseRegex();
}
