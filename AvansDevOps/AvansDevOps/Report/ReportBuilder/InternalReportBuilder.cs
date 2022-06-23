using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Sprint;

namespace AvansDevOps.Report.ReportBuilder
{
    public class InternalReportBuilder: IReportBuilder
    {
        private readonly ReportModel _report;

        public InternalReportBuilder()
        {
            this._report = new ReportModel();
        }

        public void BuildFooter()
        {
            this._report.Footer = new Footer()
                { 
                    FooterColor = "#00AA11", 
                    FooterName = "Faculty", 
                    FooterWebsite = "https://avansfictief.nl/werknemer/" 
                };
        }

        public void BuildHeader(ISprint sprint, string reportVersion, DateTime date)
        {
            this._report.Header = new Header()
            {
                HeaderColor = "#00AA11",
                HeaderName = "Faculty",
                HeaderLogoURL = "https://avansfictief.nl/werknemer/logo.png",
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
