using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AureliaWithDotNet.Web.ViewModels
{
    public class ResultServiceVM
    {
        public ResultServiceVM(ValidationResult fluentResult)
        {
            Messages = fluentResult.Errors.Select(x => x.ErrorMessage).ToList();
        }

        public ResultServiceVM()
        {
            Messages = new List<string>();
        }

        public bool Success
        {
            get
            {
                return Messages.Count == 0;
            }
        }

        public List<string> Messages { get; set; }

        public void AddMessages(ResultServiceVM resultService)
        {
            if (resultService.Messages.Count > 0) Messages.AddRange(resultService.Messages);
        }

        public string GetMessages()
        {
            return string.Join(',', Messages);
        }

        public ActionResult<ResultServiceVM> GetResult()
        {
            if (Success) return new OkResult();

            return new BadRequestObjectResult(this);
        }
    }
}
