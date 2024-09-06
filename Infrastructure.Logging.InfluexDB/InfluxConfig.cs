using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging.InfluxDB
{
    public record InfluxConfig
    (
        string Host,
        string Token,
        string Organisation,
        string Bucket
    );
}
