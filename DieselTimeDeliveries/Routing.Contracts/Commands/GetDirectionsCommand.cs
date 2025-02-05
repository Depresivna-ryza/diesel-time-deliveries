namespace Routing.Contracts.Commands;

public record GetDirectionsCommand(string Origin, string Destination)
{
    public record Result(List<string> StepInstructions);
}
