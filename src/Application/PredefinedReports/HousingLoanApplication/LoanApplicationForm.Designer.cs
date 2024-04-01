namespace DMS.Application.PredefinedReports.HousingLoanApplication
{
    partial class LoanApplicationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.subReportFormPage2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.subReportFormPage1 = new DevExpress.XtraReports.UI.XRSubreport();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0.8749962F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.subReportFormPage2,
            this.subReportFormPage1});
            this.Detail.HeightF = 46F;
            this.Detail.Name = "Detail";
            // 
            // subReportFormPage2
            // 
            this.subReportFormPage2.GenerateOwnPages = true;
            this.subReportFormPage2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23F);
            this.subReportFormPage2.Name = "subReportFormPage2";
            this.subReportFormPage2.ReportSource = new DMS.Application.PredefinedReports.HousingLoanApplication.HLF_4PH_FORM2();
            this.subReportFormPage2.SizeF = new System.Drawing.SizeF(850F, 23F);
            // 
            // subReportFormPage1
            // 
            this.subReportFormPage1.GenerateOwnPages = true;
            this.subReportFormPage1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.subReportFormPage1.Name = "subReportFormPage1";
            this.subReportFormPage1.ReportSource = new DMS.Application.PredefinedReports.HousingLoanApplication.HLF_4PH_FORM1();
            this.subReportFormPage1.SizeF = new System.Drawing.SizeF(850F, 23F);
            // 
            // LoanApplicationForm
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(0F, 0F, 0F, 0.8749962F);
            this.PageHeight = 1400;
            this.PaperKind = System.Drawing.Printing.PaperKind.Legal;
            this.Version = "22.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRSubreport subReportFormPage2;
        private DevExpress.XtraReports.UI.XRSubreport subReportFormPage1;
    }
}
