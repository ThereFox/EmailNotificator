using Common;
using Domain.Entitys;

namespace Application.Interfaces.Services
{
    public interface ILogger
    {
        public Task LogRequest(Guid bluepringId, Guid customerId);
        public Task LogSendMessage(Notification message);
        public Task LogError(Error error);
        public Task LogAddToQueue(Notification message);
    }
}
