namespace Shared.Message
{
    /// <summary>
    /// Gives definitions for the types of <see cref="IMessage"/>, avoiding the introduction of magic numbers.
    /// </summary>
    public enum MessageIdentifier
    {
        UnrecognisedMessage = 0, // in place to safeguard against unassigned ints.
        ClientDisconnection,
        LoginResponse,
        LoginRequest,
        UserNotification,
        UserSnapshot,
        UserSnapshotRequest,
        ConnectionStatusNotification,
        JamRequest,
        JamSnapshotRequest,
        JamSnapshot,
        ParticipationNotification,
        ParticipationRequest,
        ParticipationSnapshotRequest,
        ParticipationSnapshot,
        JamNotification,
        BandRequest,
        BandNotification,
        BandSnapshot,
        BandSnapshotRequest,
        TaskRequest,
        TaskNotification,
        TaskSnapshotRequest,
        TaskSnapshot,
        TaskUpdateRequest
    }
}