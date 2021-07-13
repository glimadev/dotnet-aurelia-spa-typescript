using FluentValidation;
using AureliaWithDotNet.Domain.Departments;
using AureliaWithDotNet.Domain.Interfaces;
using AureliaWithDotNet.Domain.Models;
using System;

namespace AureliaWithDotNet.Domain.Domains.Assets
{
    public class AssetValidation : AbstractValidator<Asset>
    {
        public AssetValidation(ICountryRepository countryRepository)
        {
            ICountryRepository _countryRepository = countryRepository;

            //AssetName – at least 5 Characters
            RuleFor(u => u.AssetName).MinimumLength(5).WithMessage(AssetMessage.AssetNameMinimumLength);
            //Department – must be a valid enumvalue
            RuleFor(u => u.Department).Must(EnumIsValid).WithMessage(AssetMessage.DepartmentIsInvalid);
            //CountryOfDepartment– must be a valid Country
            RuleFor(u => u.CountryOfDepartment).Must(_countryRepository.IsValid).WithMessage(AssetMessage.CountryIsInvalid);
            //PurchaseDate - must not be older then one year.
            RuleFor(u => u.PurchaseDate).GreaterThan(DateTime.Now.AddYears(-1)).WithMessage(AssetMessage.PurchaseDateGreaterThan);
            //EMailAdressOfDepartment – must be an valid email
            RuleFor(u => u.EMailAdressOfDepartment)
                .NotEmpty().WithMessage(AssetMessage.EMailAdressOfDepartmentIsValid)
                .EmailAddress().WithMessage(AssetMessage.EMailAdressOfDepartmentIsValid);
        }

        protected static bool EnumIsValid(int value)
        {
            DepartmentEnum enumVal = (DepartmentEnum)Enum.Parse(typeof(DepartmentEnum), value.ToString());

            return Enum.IsDefined(typeof(DepartmentEnum), enumVal);
        }
    }
}
