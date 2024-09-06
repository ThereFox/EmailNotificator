using CSharpFunctionalExtensions;
using Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObject
{
    public class BlueprintTemplate : CSharpFunctionalExtensions.ValueObject
    {
        private string _template { get; set; }

        public string FormatMessageForCustomer(Customer customer)
        {
            return _template;
        }

        public static Result<BlueprintTemplate> Create(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                return Result.Failure<BlueprintTemplate>("template must be not null and not empty");
            }

            return Result.Success(new BlueprintTemplate(template));
        }

        protected BlueprintTemplate(string Template)
        {
            _template = Template;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return _template;
        }
    }
}
