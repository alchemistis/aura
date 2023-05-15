namespace Aura;

public static class DotEnv
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(".env file not found.");

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(
                '=',
                2,
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}