using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Sprint;

namespace AvansDevOps.Report
{
    public interface IReportBuilder
    {
        void BuildFooter();
        void BuildHeader(ISprint sprint, string reportVersion, DateTime date);
        void BuildContent(List<string> contents);
        ReportModel GetReport(EReportFormat format);
    }
}
