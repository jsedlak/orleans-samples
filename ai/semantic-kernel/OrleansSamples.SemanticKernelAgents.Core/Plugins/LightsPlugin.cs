using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using OrleansSamples.SemanticKernelAgents.Model;
using System.ComponentModel;

namespace OrleansSamples.SemanticKernelAgents.Plugins;

public class LightsPlugin
{
    private readonly IEnumerable<LightModel> _lights;

    public LightsPlugin(IEnumerable<LightModel> lights)
    {
        _lights = lights;
    }

    [KernelFunction("get_lights")]
    [Description("Gets a list of lights and their current state")]
    public async Task<LightModel[]> GetLightsAsync()
    {
        return _lights.ToArray();
    }

    [KernelFunction("change_state")]
    [Description("Changes the state of the light")]
    public async Task<LightModel?> ChangeStateAsync(LightModel changeState)
    {
        // Find the light to change
        var light = _lights.FirstOrDefault(l => l.Id == changeState.Id);

        // If the light does not exist, return null
        if (light == null)
        {
            return null;
        }

        // Update the light state
        light.IsOn = changeState.IsOn;
        light.Brightness = changeState.Brightness;
        light.Color = changeState.Color;

        return light;
    }
}
