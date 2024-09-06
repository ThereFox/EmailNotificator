namespace Notification.ConfigsInputObjects
{
    public record KafkaProducerConfig
    (
        string BrockerUrl,
        string TopicName
    );
}
