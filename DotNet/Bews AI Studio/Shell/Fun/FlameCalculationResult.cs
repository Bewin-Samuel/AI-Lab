namespace Shell.Fun;

internal record EliminationStep(int Iteration, char EliminatedLetter, string RemainingLetters);

internal class FlameCalculationResult
{
    public required string Crush1 { get; init; }
    public required string Crush2 { get; init; }
    public required string NormalizedCrush1 { get; init; }
    public required string NormalizedCrush2 { get; init; }
    public required List<char> MatchedCharacters { get; init; }
    public required int RemainingCharCount { get; init; }
    public required List<EliminationStep> EliminationSteps { get; init; }
    public required string FinalResult { get; init; }
}
