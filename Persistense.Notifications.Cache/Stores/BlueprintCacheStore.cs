using Common.Customs;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Persistense.Entitys;
using Persistense.Notifications.EFCore.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Notifications.Cache.Stores
{
    public class BlueprintCacheStore : IBlueprintCacheStore
    {
        private readonly IDatabase _database;

        public BlueprintCacheStore(IDatabase database)
        {
            _database = database;
        }

        public async Task<Result<NotificationBlueprintEntity>> GetBlueprint(Guid Id)
        {
            try
            {
                var getResult = await _database.StringGetAsync(Id.ToString());

                if(getResult.HasValue == false)
                {
                    return Result.Failure<NotificationBlueprintEntity>($"dont contain value with id {Id}");
                }

                var parseResult = ResultJsonDeserializer.DeserializeObject<NotificationBlueprintEntity>(getResult.ToString());

                if (parseResult.IsFailure)
                {
                    return Result.Failure<NotificationBlueprintEntity>($"invalid value with id {Id}");
                }

                return parseResult;

            }
            catch (Exception ex)
            {

                return Result.Failure<NotificationBlueprintEntity>(ex.Message);
            }
        }

        public async Task<Result> SaveBlueprint(NotificationBlueprintEntity blueprint)
        {
            try
            {
                var SavedContent = getContentForSave(blueprint);

                var options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                var saveResult = await _database.StringSetAsync(blueprint.Id.ToString(), SavedContent, TimeSpan.FromMinutes(5));

                return Result.SuccessIf(saveResult, "inner error");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        private string getContentForSave(NotificationBlueprintEntity blueprint)
        {
            if (blueprint.UsedIn == null)
            {
                return JsonConvert.SerializeObject(blueprint);
            }

            var copyForNulling = new NotificationBlueprintEntity()
            {
                Channel = blueprint.Channel,
                CreatedAt = blueprint.CreatedAt,
                Id = blueprint.Id
            };

            return JsonConvert.SerializeObject(copyForNulling);
        }

    }
}
