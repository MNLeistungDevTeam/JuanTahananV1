using DevExpress.Compatibility.System.Web;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using Template.Domain.Dto.BenificiaryDto;
using Template.Domain.Dto.HousingForm;
using Template.Domain.Entities;

namespace Template.Infrastructure.PredefinedReports
{
    public partial class HousingForm : DevExpress.XtraReports.UI.XtraReport
    {

        public HousingForm()
        {
            InitializeComponent();
        }
        private void HousingForm_BeforePrint(object sender, CancelEventArgs e)
        {
            string webRootPath = this.Parameters[0].Value?.ToString() ?? "";
            var dataSource = JsonConvert.DeserializeObject<HousingFormModel>(this.Parameters[1].Value?.ToString() ?? "") ?? new HousingFormModel();
            if (dataSource != null)
            {
                string imagePath1 = Path.Combine(webRootPath, "images", "Documents", "form1.jpg");
                string imagePath2 = Path.Combine(webRootPath, "images", "Documents", "form2.jpg");
                dataSource.form1 = File.ReadAllBytes(imagePath1);
                dataSource.form2 = File.ReadAllBytes(imagePath2);
                var housingChars = dataSource.ApplicantsPersonalInformation?.HousingAccountNumber.ToString() ?? "0";
                var pagibig = dataSource.ApplicantsPersonalInformation?.PagibigNumber.ToString() ?? "0";

                for (int i = 0; i < housingChars.Length; ++i)
                {
                    foreach (Band band in Bands)
                    {
                        foreach (XRControl control in band.Controls)
                        {
                            if (control is XRLabel label)
                            {
                                if (control.Name == $"H{i}")
                                {
                                    control.Text = housingChars[i].ToString();
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(pagibig))
                {
                    for (int i = 0; i < pagibig.Length; ++i)
                    {
                        foreach (Band band in Bands)
                        {
                            foreach (XRControl control in band.Controls)
                            {
                                if (control is XRLabel label)
                                {
                                    if (control.Name == $"Pag{i + 1}")
                                    {
                                        control.Text = pagibig[i].ToString();
                                    }
                                }
                            }
                        }
                    }
                }

                var item = new List<HousingFormModel>();
                item.Add(dataSource);
                XRCheckBox purposeloan = this.FindControl<XRCheckBox>($"purpose{dataSource.LoanParticularsInformation?.PurposeOfLoanId}");
                XRCheckBox repricing = this.FindControl<XRCheckBox>($"Dis{dataSource.LoanParticularsInformation?.RepricingPeriod}");
                XRCheckBox modeofPayment = this.FindControl<XRCheckBox>($"mod{dataSource.LoanParticularsInformation?.ModeOfPaymentId}");
                XRCheckBox BarrowIndustry = this.FindControl<XRCheckBox>($"bri{dataSource.BarrowersInformationModel?.IndustryName}");
                XRCheckBox SpouseIndustry = this.FindControl<XRCheckBox>($"si{dataSource.SpouseModel?.IndustryId}");
                if (purposeloan != null )
                {
                    purposeloan.Checked = true;
                }
                if (SpouseIndustry != null)
                {
                    SpouseIndustry.Checked = true;
                }
                if (repricing != null)
                {
                    repricing.Checked = true;
                }
                if (modeofPayment != null)
                {
                    modeofPayment.Checked = true;
                }
                if (BarrowIndustry != null)
                {
                    BarrowIndustry.Checked = true;
                }
                XRCheckBox propertyType = this.FindControl<XRCheckBox>($"ty{dataSource.CollateralInformation?.PropertyTypeId}");
                if (propertyType != null)
                {
                    propertyType.Checked = true;
                }
                XRCheckBox MaritalStatus = this.FindControlTag<XRCheckBox>(dataSource?.BarrowersInformationModel?.MaritalStatus ?? "");
                if (MaritalStatus != null)
                {
                    MaritalStatus.Checked = true;
                }


                BarrowersPermanentUnitName.Text = dataSource?.BarrowersInformationModel?.PermanentUnitName;
                xrLabel17.Text = dataSource?.BarrowersInformationModel?.PermanentStreetName;
                xrLabel16.Text = dataSource?.BarrowersInformationModel?.PermanentLotName;
                xrLabel15.Text = dataSource?.BarrowersInformationModel?.PermanentBuildingName;


                xrLabel21.Text = dataSource?.BarrowersInformationModel?.PermanentSubdivisionName;
                xrLabel20.Text = dataSource?.BarrowersInformationModel?.PermanentBaranggayName;
                xrLabel19.Text = dataSource?.BarrowersInformationModel?.PermanentMunicipalityName;
                xrLabel22.Text = dataSource?.BarrowersInformationModel?.PermanentZipCode;
                xrLabel18.Text = dataSource?.BarrowersInformationModel?.PermanentProvinceName;

                xrLabel26.Text = dataSource?.BarrowersInformationModel?.PresentUnitName;
                xrLabel23.Text = dataSource?.BarrowersInformationModel?.PresentStreetName;
                xrLabel24.Text = dataSource?.BarrowersInformationModel?.PresentLotName;
                xrLabel25.Text = dataSource?.BarrowersInformationModel?.PresentBuildingName;


                xrLabel31.Text = dataSource?.BarrowersInformationModel?.PresentSubdivisionName;
                xrLabel30.Text = dataSource?.BarrowersInformationModel?.PresentBaranggayName;
                xrLabel29.Text = dataSource?.BarrowersInformationModel?.PresentMunicipalityName;
                xrLabel27.Text = dataSource?.BarrowersInformationModel?.PresentZipCode;
                xrLabel28.Text = dataSource?.BarrowersInformationModel?.PresentProvinceName;

                xrLabel35.Text = dataSource?.BarrowersInformationModel?.BusinessUnitName;
                xrLabel36.Text = dataSource?.BarrowersInformationModel?.BusinessBuildingName;
                xrLabel37.Text = dataSource?.BarrowersInformationModel?.BusinessLotName;
                xrLabel38.Text = dataSource?.BarrowersInformationModel?.BusinessStreetName;


                xrLabel48.Text = dataSource?.BarrowersInformationModel?.BusinessSubdivisionName;
                xrLabel47.Text = dataSource?.BarrowersInformationModel?.BusinessBaranggayName;
                xrLabel46.Text = dataSource?.BarrowersInformationModel?.BusinessMunicipalityName;
                xrLabel45.Text = dataSource?.BarrowersInformationModel?.BusinessProvinceName;
                xrLabel44.Text = dataSource?.BarrowersInformationModel?.BusinessZipCode;
                this.DataSource = item;
            }


        }
        public T FindControl<T>(string controlName) where T : XRControl
        {
            var control = FindControlByName(controlName);
            if (control != null && control is T typedControl)
            {
                return typedControl;
            }
            return null;
        }

        private XRControl FindControlByName(string controlName)
        {
            foreach (Band band in Bands)
            {
                var control = band.Controls.Cast<XRControl>().FirstOrDefault(c => c.Name == controlName);
                if (control != null)
                {
                    return control;
                }
            }
            return null;
        }

        public T FindControlTag<T>(string Tag) where T : XRControl
        {
            var control = FindControlByTag(Tag);
            if (control != null && control is T typedControl)
            {
                return typedControl;
            }
            return null;
        }

        private XRControl FindControlByTag(string Tag)
        {
            foreach (Band band in Bands)
            {
                var control = band.Controls.Cast<XRControl>().FirstOrDefault(c => (c.Tag ?? "")?.ToString()?.ToLower() == Tag.ToLower());
                if (control != null)
                {
                    return control;
                }
            }
            return null;
        }
    }
}
