using Application.Interfaces;
using CSharpFunctionalExtensions;
using Domain.Entitys;

namespace Infrastructure.Reader;

public class KafkaMessageReader : IMessageReader
{
    public Task<Result<Notification>> GetNewMessage()
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> HasMessages()
    {
        throw new NotImplementedException();
    }
}
