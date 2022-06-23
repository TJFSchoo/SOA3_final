using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Sprint;

namespace AvansDevOps.Report.ReportBuilder
{
    public class PublicReportBuilder : IReportBuilder
    {
        private readonly ReportModel _report;

        public PublicReportBuilder()
        {
            this._report = new ReportModel();
        }

        public void BuildFooter()
        {
            this._report.Footer = new Footer()
                {
                    FooterColor = "#ffff00", 
                    FooterName = "Public", 
                    FooterWebsite = "https://avans.nl/public"};
        }

        public void BuildHeader(ISprint sprint, string reportVersion, DateTime date)
        {
            this._report.Header = new Header()
            {
                HeaderColor = "#ffff00",
                HeaderName = "Public",
                HeaderLogoURL = "https://avans.nl/public/logo.png",
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
