using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using CSharpFunctionalExtensions;

namespace Application.Interfaces;

public interface IMessageReader
{
    public Task<Result<bool>> HasMessages();
    public Task<Result<NotificationInputObject>> GetNewMessage();
}
