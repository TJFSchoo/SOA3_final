using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Report
{
    public class Header
    {
        public string HeaderName { get; set; }
        public string HeaderLogoURL { get; set; }
        public string HeaderColor { get; set; }
        public string SprintName { get; set; }
        public string ReportVersion { get; set; }
        public DateTime Date { get; set; }
    }
}
