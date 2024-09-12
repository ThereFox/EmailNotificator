using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.ConfigsInputObjects
{
    public record RedisConfig
    (
        string Host,
        int Port,
        string CommonPassword,
        string UserName,
        string UserPassword
    );
}
