namespace Infrastructure.Kafka.Requests;

public record SendNotificationInputObject
(
    Guid Id,
    Guid BlueprintId,
    Guid CustomerId
);