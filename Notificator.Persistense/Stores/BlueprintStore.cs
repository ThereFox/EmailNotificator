using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
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

        public BlueprintStore(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<Result<Blueprint>> Get(Guid id)
        {
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

                return Result.Success(validatedBlueprint.Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<Blueprint>(ex.Message);
            }
            }
    }
}
