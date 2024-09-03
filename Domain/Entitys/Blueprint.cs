using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Blueprint : Entity<Guid>
{
    public Guid Id { get; private set; }
    public string Content {  get; private set; }
    
    public NotificationChannel Channel { get; private set; }

    public static Result<Blueprint> Create(Guid id, string content, NotificationChannel channel)
    {
        return Result.Success<Blueprint>(new Blueprint(id, content, channel));
    }
    
    protected Blueprint(Guid id, string content, NotificationChannel channel)
    {
        Id = id;
        Content = content;
        Channel = channel;
    }
}