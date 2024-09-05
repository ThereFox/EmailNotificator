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
    public class CustomerStore : ICustomerStore
    {
            private readonly ApplicationDBContext _dbContext;

            public CustomerStore(ApplicationDBContext context)
            {
                _dbContext = context;
            }

            public async Task<Result<Customer>> Get(Guid id)
            {
                if (await _dbContext.Database.CanConnectAsync() == false)
                {
                    return Result.Failure<Customer>("database unavaliable");
                }
                try
                {
                    var customer = await _dbContext
                        .CustomerEntities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ex => ex.Id == id);

                    if (customer == default)
                    {
                        return Result.Failure<Customer>("dont contain blueprint with this Id");
                    }

                    var validateCustomer = customer.ToDomain();

                    if (validateCustomer.IsFailure)
                    {
                        return Result.Failure<Customer>($"blueprint by this id invalid: {validateCustomer.Error}");
                    }

                    return Result.Success(validateCustomer.Value);
                }
                catch (Exception ex)
                {
                    return Result.Failure<Customer>(ex.Message);
                }
            }
    }
}
