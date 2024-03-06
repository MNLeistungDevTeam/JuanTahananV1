using DMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Enums
{
    public class ReferenceType
    {
        public enum Index
        {
            Company = 1,
            Vendor = 2,
            Customer = 3,
            Employee = 4
        }

        public List<DropDownModel> List { get; set; }

        private readonly string CONST_COMPANY = "Company";
        private readonly string CONST_VENDOR = "Vendor";
        private readonly string CONST_CUSTOMER = "Customer";
        private readonly string CONST_EMPLOYEE = "Employee";

        public ReferenceType()
        {
            List = new List<DropDownModel>()
            {
                new DropDownModel() { Id = (int)Index.Company, Description = CONST_COMPANY},
                new DropDownModel() { Id = (int)Index.Vendor, Description = CONST_VENDOR},
                new DropDownModel() { Id = (int)Index.Customer, Description = CONST_CUSTOMER},
                new DropDownModel() { Id = (int)Index.Employee, Description = CONST_EMPLOYEE}
            };
        }
    }
}
