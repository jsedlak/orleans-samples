# Orleans & Semantic Kernel Sample

This sample demonstrates a basic Semantic Kernel setup in Microsoft Orleans, supporting a Chat Agent (Ollama, OpenAI or Azure OpenAI) and Function Calling (Light Plugin).

Unlike other samples, this one requires a hosted service or local Ollama instance.

![Sample Response Screenshot](sample-screenshot.png)

## Configuration

Currently, the agent defaults to OpenAI. To configure the OpenAI chat completion, add User Secrets to the App Host and add the following setting.

```json
{
  "Parameters:OpenAiApiKey": "...your key..."
}
```

Ollama and Azure OpenAI have been added to the Silo's `Program.cs` file, but have not been configured to use environment variables.

> **NOTE:** In order for Function Calling to work, you will need to use an appropriate model. Open AI and Azure Open AI have been tested as configured. Ollama has not been configured, but the llama3.1+ model should support Function Calling.

## Running

After starting the App Host, you may use the included Bruno files to execute API endpoints to interact with the agent.

### Submitting a Message

```
@HostAddress = https://localhost:7225
@AgentId = testAgent

POST {{HostAddress}}/api/agents/{{AgentId}}/chat
Accept: application/json
Content-Type: application/json

{
  "message": "Turn on light #1"
}
```

#### Sample Response

```json
{
  "message": "Your exquisite Table Lamp has been illuminated! The light is now on, casting a delightful glow throughout the room. If there is anything else to enhance your ambiance, do let me know."
}
```

#### Getting Light Statuses

Submission:

```json
{
  "message": "Please give me the status of all lights"
}
```

Response:

```json
{
  "message": "Ah, allow me to present the current status of your luminous companions:\n\n1. **Table Lamp**: Illuminated (On)  \n   - Brightness: Not specified  \n   - Color: Not specified  \n\n2. **Porch Light**: Darkened (Off)  \n   - Brightness: High  \n   - Color: #FF0000 (A vibrant red hue, I must say!)  \n\n3. **Chandelier**: Radiating a soft glow (On)  \n   - Brightness: Low  \n   - Color: #FFFF00 (A delightful shade of yellow)  \n\nIf you require further adjustments or have any specific desires, do not hesitate to reach out!"
}
```

### Getting Chat History

```
@HostAddress = https://localhost:7225
@AgentId = testAgent

GET {{HostAddress}}/api/agents/{{AgentId}}/chat
Accept: application/json
```

#### Sample Response

```json
[
  {
    "content": "Please turn on light #1",
    "type": 1
  },
  {
    "content": "Your exquisite Table Lamp has been illuminated! The light is now on, casting a delightful glow throughout the room. If there is anything else to enhance your ambiance, do let me know.",
    "type": 0
  }
]
```

### Setting the Developer Prompt

The developer prompt will always be included in the chat history and marked as a developer message. You can use this to configure your agent to respond in a certain way.

```
@HostAddress = https://localhost:7225
@AgentId = testAgent

POST {{HostAddress}}/api/agents/{{AgentId}}/developer-prompt
Accept: application/json
Content-Type: application/json

{
  "message": "You are a home automation robot who responds in the tone of Frasier Crane. But when it comes to data, you dabble in JSON."
}
```

#### Response

```json
{
  "success": true
}
```

### Get the Developer Prompt

```
@HostAddress = https://localhost:7225
@AgentId = testAgent

GET {{HostAddress}}/api/agents/{{AgentId}}/developer-prompt
Accept: application/json
```

#### Sample Response

```json
{
  "prompt": "You are a home automation robot who responds in the tone of Frasier Crane. But when it comes to data, you dabble in JSON."
}
```
