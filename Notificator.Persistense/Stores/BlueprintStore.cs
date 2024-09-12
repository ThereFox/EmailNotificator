using App.Stores;
using Application.Interfaces.Services;
using Common;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Persistense.Notifications.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Notifications.EFCore.Stores
{
    public class BlueprintStore : IBlueprintStore
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IBlueprintCacheStore _cache;
        private readonly ILogger _logger;

        public BlueprintStore(ApplicationDBContext context, IBlueprintCacheStore cache, ILogger logger)
        {
            _dbContext = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<Blueprint>> Get(Guid id)
        {
            var getFromCacheResult = await _cache.GetBlueprint(id);

            if (getFromCacheResult.IsSuccess)
            {
                return getFromCacheResult.Value.ToDomain();
            }
            else
            {
                await _logger.LogError(getFromCacheResult.AsError());
            }

            if(await _dbContext.Database.CanConnectAsync() == false)
            {
                return Result.Failure<Blueprint>("database unavaliable");
            }
            try
            {
                var blueprint = await _dbContext
                    .Blueprints
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ex => ex.Id == id);

                if(blueprint == default)
                {
                    return Result.Failure<Blueprint>("dont contain blueprint with this Id");
                }

                var validatedBlueprint = blueprint.ToDomain();

                if (validatedBlueprint.IsFailure)
                {
                    return Result.Failure<Blueprint>($"blueprint by this id invalid: {validatedBlueprint.Error}");
                }

                var saveToCacheResult = await _cache.SaveBlueprint(blueprint);

                if (saveToCacheResult.IsFailure)
                {
                    await _logger.LogError(saveToCacheResult.AsError());
                }

                return Result.Success(validatedBlueprint.Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<Blueprint>(ex.Message);
            }
            }
    }
}
