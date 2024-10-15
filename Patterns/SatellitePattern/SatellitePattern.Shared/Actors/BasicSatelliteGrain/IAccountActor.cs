using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors.BasicSatelliteGrain;

/// <summary>
/// Represents a user's account in the online system
/// </summary>
public interface IAccountActor : IGrainWithGuidKey
{
    /// <summary>
    /// Sets the status of the user to what they are currently doing and marks them as online
    /// </summary>
    /// <param name="status">The message containing what the user is doing</param>
    /// <returns>True if successful</returns>
    ValueTask<bool> SignIn(string status);

    /// <summary>
    /// Signs the user out of the system, clearing their status
    /// </summary>
    /// <returns>True if successful</returns>
    ValueTask<bool> SignOut();

    /// <summary>
    /// Sends a Message to the user
    /// </summary>
    /// <param name="senderAccountId">The targets's account identifier</param>
    /// <param name="message">The message being submitted</param>
    Task SendMessage(Guid senderAccountId, string message);

    /// <summary>
    /// Retrieves all messages sent to the user
    /// </summary>
    Task<DirectMessage[]> GetMessages();
}
