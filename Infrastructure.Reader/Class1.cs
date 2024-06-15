using Application.DTO;
using Application.Interfaces;
using CSharpFunctionalExtensions;

namespace Infrastructure.Reader;

public class KafkaMessageReader : IMessageReader

{
    public Task<Result<NotificationInputObject>> GetNewMessage()
    {
        throw new NotImplementedException();
    }
}
