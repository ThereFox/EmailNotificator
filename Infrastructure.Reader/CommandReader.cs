using Application.Interfaces.Services;
using Common.Customs;
using CSharpFunctionalExtensions;
using Infrastructure.Kafka.Requests;
using Infrastructure.Reader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Brocker.Kafka.Consumer
{
    public class CommandReader : IMessageGetter
    {
        private readonly KafkaConsumer _consumer;

        private readonly string _topicName;

        public CommandReader(KafkaConsumer consumer, string topicName)
        {
            _consumer = consumer;
            _topicName = topicName;
        }

        public async Task<Result<SendNotificationInputObject>> GetNewMessage()
        {
            var getMessageResult = await _consumer.GetNewMessageFromTopic(_topicName);

            if (getMessageResult.IsFailure)
            {
                return Result.Failure<SendNotificationInputObject>(getMessageResult.Error);
            }

            var convertResult = ResultJsonDeserializer.DeserializeObject<SendNotificationInputObject>(getMessageResult.Value);
            
            if(convertResult.IsFailure)
            {
                return Result.Failure<SendNotificationInputObject>($"invalid value readed. Info: {convertResult.Error}");
            }

            return convertResult;

        }
    }
}
