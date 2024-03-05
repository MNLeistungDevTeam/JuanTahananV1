﻿using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMS.Infrastructure.PredefinedReports
{
    public static class ReportsFactory
    {
        public static Dictionary<string, Func<XtraReport>> Reports = new Dictionary<string, Func<XtraReport>>()
        {
            ["TestReport"] = () => new TestReport(),
            ["BuyerConfirmationForm"] = () => new BuyerConfirmationForm(),
            ["HousingForm"] = () => new HousingForm(),
        };



    }
}