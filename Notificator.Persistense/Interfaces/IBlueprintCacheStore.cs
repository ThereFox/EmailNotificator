using CSharpFunctionalExtensions;
using Persistense.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Notifications.EFCore.Interfaces
{
    public interface IBlueprintCacheStore
    {
        public Task<Result<NotificationBlueprintEntity>> GetBlueprint(Guid Id);
        public Task<Result> SaveBlueprint(NotificationBlueprintEntity blueprintEntity);
    }
}
