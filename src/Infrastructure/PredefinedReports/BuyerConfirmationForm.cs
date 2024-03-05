
using System.ComponentModel;

using Template.Domain.Dto.BenificiaryDto;


namespace Template.Infrastructure.PredefinedReports
{
    public partial class BuyerConfirmationForm : DevExpress.XtraReports.UI.XtraReport
    {

        public BuyerConfirmationForm()
        {
            InitializeComponent();
        }

        private void BuyerConfirmationForm_BeforePrint(object sender, CancelEventArgs e)
        {
            string webRootPath = this.Parameters[0].Value?.ToString() ?? "";
            string imagePath = Path.Combine(webRootPath, "images", "Documents", "BCF.jpg");
            var datasourceModel = new BenificiaryReportModel() { 
                BcfPicture = File.ReadAllBytes(imagePath),
            };
            var item = new List<BenificiaryReportModel>();
            item.Add(datasourceModel);
            this.DataSource = item;
        }

    }
}
