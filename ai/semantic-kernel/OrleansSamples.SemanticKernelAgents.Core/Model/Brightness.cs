using System.Text.Json.Serialization;

namespace OrleansSamples.SemanticKernelAgents.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Brightness
{
    Low,
    Medium,
    High
}
