namespace BuildingBlocks.Contracts.Events;

public record StreamRequestEvent(Guid ProcessId, int[] Data);
public record StreamResponseEvent(Guid ProcessId, int[] Result);
