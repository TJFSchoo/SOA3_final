using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Sprint;

namespace AvansDevOps.Report.ReportBuilder
{
    public class AvansReportBuilder : IReportBuilder
    {
        private readonly ReportModel _report;

        public AvansReportBuilder()
        {
            this._report = new ReportModel();
        }

        public void BuildFooter()
        {
            this._report.Footer = new Footer()
                {CompanyColor = "#fab207", CompanyName = "Avans", CompanyWebsite = "https://avans.nl"};
        }

        public void BuildHeader(ISprint sprint, string reportVersion, DateTime date)
        {
            this._report.Header = new Header()
            {
                CompanyColor = "#fab207",
                CompanyName = "Avans",
                CompanyLogoURL = "https://avans.nl/logo.png",
                Date = date,
                ReportVersion = reportVersion,
                SprintName = sprint.GetName()
            };
        }

        public void BuildContent(List<string> contents)
        {
            this._report.Contents = contents;
        }

        public ReportModel GetReport(EReportFormat format)
        {
            this._report.Format = format;
            return this._report;
        }
    }
}
