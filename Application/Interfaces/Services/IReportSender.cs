using CSharpFunctionalExtensions;
using Domain.Entitys;
using Infrastructure.Kafka.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IReportSender
    {
        public Task<Result> SendReport(Guid initNotificationId, bool isError = false);
    }
}
