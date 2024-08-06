using Common;

namespace Application.Interfaces
{
    public interface ILogger
    {
        public Task LogReadMessage();
        public Task LogSendMessage();
        public Task LogError(Error error);
    }
}
