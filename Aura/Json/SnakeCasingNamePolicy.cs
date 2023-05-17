using System.Text;
using System.Text.Json;

namespace Aura.Json;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        var result = new StringBuilder();
        var firstUnderscore = true;

        foreach (var character in name)
        {
            if (char.IsUpper(character))
            {
                if (!firstUnderscore)
                    result.Append('_');

                result.Append(char.ToLowerInvariant(character));
            }
            else
            {
                result.Append(character);
            }

            firstUnderscore = false;
        }

        return result.ToString();
    }
}