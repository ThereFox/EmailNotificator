using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Customer
{
    private List<Device> _devices;
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    
    public CustomerRole Role { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
       

    public static Result<Customer> Create(Guid id, string name, CustomerRole role, DateTime createdAt, List<Device> devices)
    {
        if (createdAt >= DateTime.Now)
        {
            return Result.Failure<Customer>("invalid creation time");
        }

        return Result.Success<Customer>(new Customer(id, name, role, createdAt, devices));

    }
    
    public Result<Device> PickDeviceForNotificationByChannel(NotificationChannel channel)
    {
        if(_devices == null)
        {
            throw new InvalidCastException("loaded not full info by customer");
        }
        if(_devices.Any(ex => ex.NotificationChannel == channel && ex.IsActive) == false)
        {
            return Result.Failure<Device>("User dont have any devices for this channel");
        }
        return _devices.First(ex => ex.NotificationChannel == channel && ex.IsActive);
    }

    protected Customer(Guid id, string name, CustomerRole role, DateTime createdAt, List<Device> devices)
    {
        Id = id;
        Name = name;
        Role = role;
        CreatedAt = createdAt;
        _devices = devices;
    }
}