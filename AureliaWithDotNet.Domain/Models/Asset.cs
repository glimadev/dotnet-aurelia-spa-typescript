using System;

namespace AureliaWithDotNet.Domain.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public int Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EMailAdressOfDepartment { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool? Broken { get; set; }
    }
}
