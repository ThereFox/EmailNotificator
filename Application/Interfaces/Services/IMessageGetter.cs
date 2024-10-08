using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entitys;
using CSharpFunctionalExtensions;
using Infrastructure.Kafka.Requests;

namespace Application.Interfaces.Services;

public interface IMessageGetter
{
    public Task<Result<SendNotificationInputObject>> GetNewMessage();
}
