using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Report
{
    public class Header
    {
        public string CompanyName { get; set; }
        public string CompanyLogoURL { get; set; }
        public string CompanyColor { get; set; }
        public string SprintName { get; set; }
        public string ReportVersion { get; set; }
        public DateTime Date { get; set; }
    }
}
