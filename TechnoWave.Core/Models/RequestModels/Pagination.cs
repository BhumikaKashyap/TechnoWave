using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoWave.Core.Models.RequestModels
{
    public class ColumnFilter
    {
        [DefaultValue("")]
        public string? Field { get; set; } = "";
        [DefaultValue("")]// e.g. "FirstName"
        //public string? Operator { get; set; } = "";
        //[DefaultValue("")]// e.g. "startsWith", "contains", "equals"
        public string? Value { get; set; } = "";        // e.g. "Ram"
    }
    public class Pagination
    {
        [DefaultValue("")]
        public string? TenantId { get; set; } = "";
        [DefaultValue("")]
        public string? SearchValue { get; set; } = "";
        [DefaultValue("")]
        public string? OrderStatus { get; set; } = "";
        [DefaultValue("")]
        public string? NotificationType { get; set; } = "";
        [DefaultValue(1)]
        public int PageNo { get; set; } = 1;
        [DefaultValue(10)]
        public int RecordPerPage { get; set; } = 10;

        [DefaultValue(null)]
        public DateTime? StartDate { get; set; } = null;

        [DefaultValue(null)]
        public DateTime? ExpiryDate { get; set; } = null;

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; } = null;

        [DefaultValue(null)]
        public DateTime? FormDate { get; set; } = null;

        [DefaultValue(2)]
        public int Status { get; set; } = 2;
        [DefaultValue("")]
        public string? Vendor { get; set; } = "";


        [DefaultValue(false)]
        public bool? IsOptionExpression { get; set; } = false;

        [DefaultValue(false)]
        public bool? IsPartnerEstimateInvoice { get; set; } = false;

        [DefaultValue(false)]
        public bool? IsCustomer { get; set; } = false;


        [DefaultValue("")]
        public string? CustomerId { get; set; } = "";

        [DefaultValue("")]
        public string? EstimationId { get; set; } = "";

        [DefaultValue("")]
        public string? OrderId { get; set; } = "";

        [DefaultValue("")]
        public string? ProductType { get; set; } = "";
        [DefaultValue("")]
        public string? JobStatus { get; set; } = "";
        [DefaultValue("")]
        public string? JobId { get; set; } = "";

        [DefaultValue("")]
        public string? Address { get; set; } = "";

        [DefaultValue("")]
        public string? Region { get; set; } = "";

        [DefaultValue("")]
        public string? SortColumn { get; set; } = "";  // E.g. "FirstName", "LastName"

        [DefaultValue("asc")]
        public string? SortDirection { get; set; } = "asc";  // "asc" or "desc"

        public List<ColumnFilter>? ColumnFilters { get; set; } = new(); // 🔍 Individual column filters

        //[DefaultValue(false)]
        public bool? Live { get; set; } = null;

        [DefaultValue("")]
        public string? EstimateCode { get; set; } = "";

        [DefaultValue("")]
        public string? OrderUniqueCode { get; set; } = "";
        public bool IsMasterData { get; set; } = false;
    }
}
