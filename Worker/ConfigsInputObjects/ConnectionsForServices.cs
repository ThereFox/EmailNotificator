using Worker.ConfigsInputObjects;

namespace Notification.ConfigsInputObjects
{
    public record ConnectionsForServices
    (
        DatabasesConfig Databases,
        BrockersConfig Brockers
    );
}
