using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AureliaWithDotNet.Web.ViewModels
{
    public class ResultServiceDataVM<T> : ResultServiceVM
    {
        public ResultServiceDataVM()
            : base()
        {
        }

        public ResultServiceDataVM(T data)
            : base()
        {
            Data = data;
        }

        public ResultServiceDataVM(ValidationResult fluentResult)
            : base(fluentResult)
        {
        }

        public T Data { get; set; }

        public ActionResult<ResultServiceDataVM<T>> GetResultData()
        {
            if (Success) return new OkObjectResult(this);
            else return new NotFoundObjectResult(this);
        }
    }
}
