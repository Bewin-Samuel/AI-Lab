using System.Text;

namespace Shell.Fun;

internal static class FlameResultFormatter
{
    public static string Format(FlameCalculationResult result)
    {
        var sb = new StringBuilder();

        sb.AppendLine("=== Step 1: Inputs ===");
        sb.AppendLine($"Crush 1: {result.Crush1}");
        sb.AppendLine($"Crush 2: {result.Crush2}");
        sb.AppendLine();

        sb.AppendLine("=== Step 2: Strike Matching Characters ===");
        sb.AppendLine($"{result.NormalizedCrush1}  vs  {result.NormalizedCrush2}");

        if (result.MatchedCharacters.Count > 0)
            sb.AppendLine($"Struck Characters: {string.Join(", ", result.MatchedCharacters)}");
        else
            sb.AppendLine("No matching characters found.");

        sb.AppendLine($"Remaining Character Count: {result.RemainingCharCount}");
        sb.AppendLine();

        sb.AppendLine("=== Step 3: FLAME Elimination ===");

        foreach (var step in result.EliminationSteps)
        {
            sb.AppendLine($"Iteration {step.Iteration} → Strike '{step.EliminatedLetter}' → [{step.RemainingLetters}]");
        }

        sb.AppendLine();
        sb.AppendLine($"🔥 Result:");
        sb.Append($"{result.Crush1} shares a {result.FinalResult} with {result.Crush2}");

        return sb.ToString();
    }
}