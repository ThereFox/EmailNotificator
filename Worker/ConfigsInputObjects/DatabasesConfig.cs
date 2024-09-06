using Infrastructure.Logging.InfluxDB;
using Notification.ConfigsInputObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.ConfigsInputObjects
{
    public record DatabasesConfig
    (
      DatabaseConfig Main,
      InfluxConfig Logs,
      CacheConfig Cache
    );
}
