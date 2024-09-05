using Application.DTO;
using Application.Interfaces.Services;
using CSharpFunctionalExtensions;
using Infrastructure.Kafka.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Brocker.Kafka.Producer
{
    public class ReportProducer : IReportSender
    {
        private readonly KafkaProducer _producer;

        private readonly string _topicName;

        public ReportProducer(KafkaProducer producer, string topicName)
        {
            _producer = producer;
            _topicName = topicName;
        }

        public async Task<Result> SendReport(Guid initNotificationId, bool isError = false)
        {
            var message = new NotificationSendReport(initNotificationId, !isError);

            var selialisedData = JsonConvert.SerializeObject(message);

            return await _producer.SendDataToTopic(selialisedData, _topicName);
        }
    }
}
