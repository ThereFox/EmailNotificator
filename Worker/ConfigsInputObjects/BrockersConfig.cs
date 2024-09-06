using Notification.ConfigsInputObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.ConfigsInputObjects
{
    public record BrockersConfig
    (
        KafkaConsumerConfig ConsumerInfo,
        KafkaProducerConfig ProducerInfo
    );

}
