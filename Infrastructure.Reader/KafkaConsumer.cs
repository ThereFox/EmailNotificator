using Application.Interfaces;
using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Infrastructure.Kafka.Requests;
using Infrastructure.Reader.Trying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Reader
{
    public class KafkaConsumer : IMessageReader
    {
        private const int TryDeadline = 200;
        private const int TrysCount = 3;

        private readonly IConsumer<Null, SendNotificationRequest> _consumer;

        public async Task<Result<SendNotificationRequest>> GetNewMessage()
        {
            var readResult = await _consumer.TryGetAsync(TrysCount, TryDeadline);

            if (readResult.IsFailure)
            {
                return Result.Failure<SendNotificationRequest>(readResult.Error);
            }

            return readResult;
        }

    }
}
