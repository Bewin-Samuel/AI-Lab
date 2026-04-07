using System.Text.RegularExpressions;

namespace Shell.Fun;

internal class FlameCalculator
{
    private const string Flame = "FLAME";

    public FlameCalculationResult Calculate(string crush1, string crush2)
    {
        var normalizedCrush1 = Normalize(crush1);
        var normalizedCrush2 = Normalize(crush2);

        var (remainingCount, matchedCharacters) = StrikeCommonLetters(normalizedCrush1, normalizedCrush2);
        var (eliminationSteps, finalResult) = EliminateFlameLetters(remainingCount);

        return new FlameCalculationResult
        {
            Crush1 = crush1,
            Crush2 = crush2,
            NormalizedCrush1 = normalizedCrush1,
            NormalizedCrush2 = normalizedCrush2,
            MatchedCharacters = matchedCharacters,
            RemainingCharCount = remainingCount,
            EliminationSteps = eliminationSteps,
            FinalResult = finalResult
        };
    }

    private static string Normalize(string name) =>
        Regex.Replace(name.ToUpper(), "[^A-Z]", "");

    private static (int RemainingCount, List<char> MatchedCharacters) StrikeCommonLetters(string crush1, string crush2)
    {
        char?[] arr1 = Array.ConvertAll(crush1.ToCharArray(), c => (char?)c);
        char?[] arr2 = Array.ConvertAll(crush2.ToCharArray(), c => (char?)c);
        List<char> matched = [];

        for (int i = 0; i < arr1.Length; i++)
        {
            for (int j = 0; j < arr2.Length; j++)
            {
                if (arr1[i] is not null && arr2[j] is not null && arr1[i] == arr2[j])
                {
                    matched.Add(arr1[i]!.Value);
                    arr1[i] = null;
                    arr2[j] = null;
                    break;
                }
            }
        }

        int remaining = arr1.Count(c => c is not null) + arr2.Count(c => c is not null);
        return (remaining, matched);
    }

    private static (List<EliminationStep> Steps, string FinalResult) EliminateFlameLetters(int count)
    {
        List<char> letters = [.. Flame];
        List<EliminationStep> steps = [];
        int pos = 0;
        int iteration = 1;

        while (letters.Count > 1)
        {
            int idx = (pos + count - 1) % letters.Count;
            char eliminated = letters[idx];
            letters.RemoveAt(idx);
            pos = idx % letters.Count;

            steps.Add(new EliminationStep(iteration++, eliminated, string.Join(", ", letters)));
        }

        return (steps, ResolveMeaning(letters[0]));
    }

    private static string ResolveMeaning(char letter) => letter switch
    {
        'F' => "Friendship 🤝",
        'L' => "Love ❤️",
        'A' => "Affection 🥰",
        'M' => "Marriage 💍",
        'E' => "Enemies ⚔️",
        _ => "Unknown"
    };
}
