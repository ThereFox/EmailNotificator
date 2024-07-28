using Application.DTO;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    internal interface INotificationHandler
    {
        public Task<Result> Handl(NotificationInputObject notification); 
    }
}
