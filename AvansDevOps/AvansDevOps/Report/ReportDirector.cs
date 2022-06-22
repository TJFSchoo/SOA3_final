using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Report.ReportBuilder;
using AvansDevOps.Sprint;

namespace AvansDevOps.Report
{
    public static class ReportDirector
    {
        public static ReportModel BuildAvansReport(ISprint sprint, List<string> contents, string reportVersion, DateTime date, EReportFormat format)
        {
            IReportBuilder builder = new AvansReportBuilder();
            builder.BuildContent(contents);
            builder.BuildFooter();
            builder.BuildHeader(sprint, reportVersion, date);
            return builder.GetReport(format);
        }

        public static ReportModel BuildAvansPlusReport(ISprint sprint, List<string> contents, string reportVersion, DateTime date, EReportFormat format)
        {
            IReportBuilder builder = new AvansPlusReportBuilder();
            builder.BuildContent(contents);
            builder.BuildFooter();
            builder.BuildHeader(sprint, reportVersion, date);
            return builder.GetReport(format);
        }
    }
}
