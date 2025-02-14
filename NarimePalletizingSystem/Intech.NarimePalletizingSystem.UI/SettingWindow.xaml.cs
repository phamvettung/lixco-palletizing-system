using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Intech.NarimePalletizingSystem.UI
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            Loaded += SettingWindow_Loaded;
            btnReadModel.Click += BtnReadModel_Click;
            btnSetModel.Click += BtnSetModel_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnSetModelSpace.Click += BtnSetModelSpace_Click;
            btnSetFormName.Click += BtnSetFormName_Click;
            btnResetNumOrder.Click += BtnResetNumOrder_Click;

        }


        #region Events
        private void SettingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFormName.Text = Properties.Settings.Default.FormName.ToString();
                txtModelSpace.Text = Properties.Settings.Default.ModelSpace.ToString();
                DefaultSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnResetNumOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("Bạn muốn xóa toàn bộ STT đã lưu của LINE và MODEL đã tính.", "Confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if(mr == MessageBoxResult.Yes)
            {
                ResetNumOrder();
            }
        }

        private void BtnSetFormName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.FormName = txtFormName.Text;
                Properties.Settings.Default.Save();
                MessageBox.Show("Hoàn thành!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {   
                MessageBox.Show(ex.Message, "BtnSetFormName_Click", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void BtnSetModelSpace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool modelSpaceRet = int.TryParse(txtModelSpace.Text, out int modelSpace);
                Properties.Settings.Default.ModelSpace = modelSpace;
                Properties.Settings.Default.Save();
                MessageBox.Show("Hoàn thành!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BtnSetModelSpace_Click", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DefaultSettings();
        }

        private void BtnSetModel_Click(object sender, RoutedEventArgs e)
        {
            bool lineRet = int.TryParse(cboLine.Text, out int line);
            bool modelRet = int.TryParse(cboModel.Text, out int model);
            bool numOrderRet = int.TryParse(txtNumOrder.Text, out int numOrder);
            if(lineRet && modelRet && numOrderRet)
            {
                SetNumOrder(line, model, numOrder.ToString());
            }
            else
            {
                MessageBox.Show("Thông tin vừa nhập không chính xác. Vui lòng kiểm tra lại", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnReadModel_Click(object sender, RoutedEventArgs e)
        {
            bool lineRet = int.TryParse(cboLine.Text, out int line);
            bool modelRet = int.TryParse(cboModel.Text, out int model);
            if (lineRet && modelRet)
            {
                ReadNumOrder(line, model);
            }
            else
            {
                MessageBox.Show("Thông tin vừa nhập không chính xác. Vui lòng kiểm tra lại", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #endregion

        #region Methods

        private void ReadNumOrder(int line, int model)
        {
            try
            {
                string numOrderVal = string.Empty;
                switch (line)
                {
                    case 1:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                                numOrderVal = Properties.Settings.Default.Line1[i];
                        break;
                    case 2:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                                numOrderVal = Properties.Settings.Default.Line2[i];
                        break;
                    case 3:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                                numOrderVal = Properties.Settings.Default.Line3[i];

                        break;
                    case 4:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                                numOrderVal = Properties.Settings.Default.Line4[i];
                        break;
                    case 5:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                                numOrderVal = Properties.Settings.Default.Line5[i];
                        break;
                    case 6:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                                numOrderVal = Properties.Settings.Default.Line6[i];
                        break;
                    default:
                        break;
                }
                txtNumOrder.Text = numOrderVal;
                DefaultSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadNumOrder", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetNumOrder(int line, int model, string numOrderVal)
        {
            try
            {
                switch (line)
                {
                    case 1:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                            {
                                Properties.Settings.Default.Line1[i] = numOrderVal;
                                Properties.Settings.Default.Save();
                            }
                        break;
                    case 2:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                            {
                                Properties.Settings.Default.Line2[i] = numOrderVal;
                                Properties.Settings.Default.Save();
                            }
                        break;
                    case 3:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                            {
                                Properties.Settings.Default.Line3[i] = numOrderVal;
                                Properties.Settings.Default.Save();
                            }
                        break;
                    case 4:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                            {
                                Properties.Settings.Default.Line4[i] = numOrderVal;
                                Properties.Settings.Default.Save();
                            }
                        break;
                    case 5:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                            {
                                Properties.Settings.Default.Line5[i] = numOrderVal;
                                Properties.Settings.Default.Save();
                            }
                        break;
                    case 6:
                        for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                            if (i == model)
                            {
                                Properties.Settings.Default.Line6[i] = numOrderVal;
                                Properties.Settings.Default.Save();
                            }
                        break;
                    default:
                        break;
                }
                DefaultSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SetNumOrder", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DefaultSettings()
        {
            try
            {
                cboModel.Items.Clear();
                cboLine.Items.Clear();
                for (int i = 1; i <= Properties.Settings.Default.ModelSpace; i++)
                {
                    cboModel.Items.Add(i.ToString());
                    if (i <= 6)
                    {
                        cboLine.Items.Add(i.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DefaultSettings", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetNumOrder()
        {
            try
            {
                Properties.Settings.Default.Line1.Clear();
                Properties.Settings.Default.Line2.Clear();
                Properties.Settings.Default.Line3.Clear();
                Properties.Settings.Default.Line4.Clear();
                Properties.Settings.Default.Line5.Clear();
                Properties.Settings.Default.Line6.Clear();
                for (int i = 1; i < Properties.Settings.Default.ModelSpace; i++)
                {
                    Properties.Settings.Default.Line1.Add("0");
                    Properties.Settings.Default.Line2.Add("0");
                    Properties.Settings.Default.Line3.Add("0");
                    Properties.Settings.Default.Line4.Add("0");
                    Properties.Settings.Default.Line5.Add("0");
                    Properties.Settings.Default.Line6.Add("0");
                }

                MessageBox.Show("Hoàn thành!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ResetNumOrder", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
