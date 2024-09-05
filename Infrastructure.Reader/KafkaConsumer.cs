using Application.Interfaces.Services;
using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Infrastructure.Kafka.Requests;
using Infrastructure.Reader.Trying;

namespace Infrastructure.Reader
{
    public class KafkaConsumer
    {
        private const int TryDeadline = 200;
        private const int TrysCount = 3;

        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaConsumer(IConsumer<Ignore, string> consumer)
        {
            _consumer = consumer;
        }

        public async Task<Result<string>> GetNewMessageFromTopic(string topic)
        {
            _consumer.Subscribe(topic);

            var readResult = await _consumer.TryGetAsync(TrysCount, TryDeadline);

            _consumer.Unsubscribe();

            if (readResult.IsFailure)
            {
                return Result.Failure<string>(readResult.Error);
            }

            return readResult;
        }
    }
}
