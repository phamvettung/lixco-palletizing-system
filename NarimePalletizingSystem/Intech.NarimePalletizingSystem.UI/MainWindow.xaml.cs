using Intech.NarimePalletizingSystem.BLL.Services;
using Intech.NarimePalletizingSystem.DAL.Entities;
using Intech.NarimePalletizingSystem.UI.Configurations;
using Intech.NarimePalletizingSystem.UI.Lixco;
using Intech.NarimePalletizingSystem.UI.Responses;
using Intech.NarimePalletizingSystem.UI.Models;
using Intech.NarimePalletizingSystem.UI.Systems;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Primitives;

namespace Intech.NarimePalletizingSystem.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ProductService _productService = new();
        PalletService _palletService = new();

        public static TeamLix TeamLixs { get; set; } = null;
        public static ShiftLix ShiftLixs { get; set; } = null;
        public static LineLix LineLixs { get; set; } = null;

        private TeamLixDTO selectedTeam = null;
        private ShiftLixDTO selectedShift = null;

        TSC _tsc = new();
        PLC _plc = new();

        Account account = AppConfig.GetAccount();

        System.Windows.Threading.DispatcherTimer dispatcherTimer = null;

        List<Product> products = null;

        public MainWindow()
        {
            new SplashWindow().ShowDialog();
            InitializeComponent();
            RegisterEvents();
            InitialTimers();
        }

        private void InitialTimers()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if(rdoManual.IsChecked != true)
            {
                txtDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (_plc.DataControl.reprint)
            {
                DisplayPrinter(lbPrinter, AppEnum.Reprint, AppEnum.Printer);
            }
            else if (_plc.DataControl.printError)
            {
                DisplayPrinter(lbPrinter, AppEnum.Printerror, AppEnum.Printer);
            }
            else
            {
                DisplayPrinter(lbPrinter, AppEnum.Idle, AppEnum.Printer);
            }
        }

        private void RegisterEvents()
        {
            this.Loaded += MainWindow_Loaded;

            btnAdd.Click += BtnAdd_Click;
            btnPrint.Click += BtnPrint_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnSearch.Click += BtnSearch_Click;

            cboTeamId.SelectionChanged += CboTeamId_SelectionChanged;
            cboShiftId.SelectionChanged += CboShiftId_SelectionChanged;

            rdoManual.Checked += RdoManual_Checked;
            rdoAuto.Checked += RdoAuto_Checked;

            txtModel.KeyUp += TxtModel_KeyUp;
            txtPacketWeight.KeyUp += TxtPacketWeight_KeyUp;
            txtNumPacketOnBin.KeyUp += TxtNumPacketOnBin_KeyUp;
            txtNumBinOnPallet.KeyUp += TxtNumBinOnPallet_KeyUp;

            PLC.OnModelChanged += PLC_OnModelChanged;
            PLC.OnOrderChanged += PLC_OnOrderChanged;
            PLC.OnLineChanged += PLC_OnLineChanged;
            PLC.OnPrintStarted += PLC_OnPrintStarted;
            PLC.OnRePrint += PLC_OnRePrint;
            PLC.OnPrintError += PLC_OnPrintError;
        }


        #region Form events
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDefault();
            LoadShiftTeamLix();
            ConnectToPLC();
            ShowStatusTSC();
        }

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.ShowDialog();
        }

        private void TxtNumBinOnPallet_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            double netWeight = CalculateNetweight();
            txtNetweight.Text = netWeight.ToString();
            e.Handled = true;
        }

        private void TxtNumPacketOnBin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            double netWeight = CalculateNetweight();
            txtNetweight.Text = netWeight.ToString();
            e.Handled = true;
        }

        private void TxtPacketWeight_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            double netWeight = CalculateNetweight();
            txtNetweight.Text = netWeight.ToString();
            e.Handled = true;
        }

        private void TxtModel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            bool modelRet = int.TryParse(txtModel.Text, out int model);
            if (modelRet == false)
            {
                MessageBox.Show("Model vừa nhập không hợp lệ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            LoadProduct(model);
            e.Handled = true;
        }

        private void RdoAuto_Checked(object sender, RoutedEventArgs e)
        {
            AutoLoadDefault();
        }

        private void RdoManual_Checked(object sender, RoutedEventArgs e)
        {
            ManualLoadDefault();
        }

        private void PLC_OnPrintStarted()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (rdoManual.IsChecked == false && rdoAuto.IsChecked == true)
                {

                    if(_plc.DataControl.reprint == false && cboCalculateQuantity.IsChecked == false)
                    {
                        Pallet pallet = GetPallet(false); 
                        Product product = GetProduct();
                        PrintLabel(pallet, product, false);
                        ShowPalletId(pallet.LineId.ToString(), txtNumOrderPlc.Text, pallet.NumOrder);
                    }
                    else if(_plc.DataControl.reprint == false && cboCalculateQuantity.IsChecked == true)
                    {
                        Pallet pallet = GetPallet(false); 
                        Product product = GetProduct();
                        PrintLabel(pallet, product, false);
                        ShowPalletId(pallet.LineId.ToString(), txtNumOrderPlc.Text, pallet.NumOrder);
                        SavePalletLix(pallet, product); //TINH SAN LUONG
                    }
                    else if(_plc.DataControl.reprint == true && cboCalculateQuantity.IsChecked == false)
                    {
                        Pallet pallet = GetPallet(true); //FALSE: TĂNG STT TRUE: KO TĂNG STT
                        Product product = GetProduct();
                        PrintLabel(pallet, product, false);
                        ShowPalletId(pallet.LineId.ToString(), txtNumOrderPlc.Text, pallet.NumOrder);                        
                    }
                    else if (_plc.DataControl.reprint == true && cboCalculateQuantity.IsChecked == true)
                    {
                        Pallet pallet = GetPallet(true); 
                        Product product = GetProduct();
                        PrintLabel(pallet, product, false);
                        ShowPalletId(pallet.LineId.ToString(), txtNumOrderPlc.Text, pallet.NumOrder);
                    }

                }
            }));
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (rdoManual.IsChecked == true && rdoAuto.IsChecked == false)
            {
                MessageBoxResult dr = MessageBox.Show("Bạn muốn In tem có tính STT Pallet?\n+ Yes: Có tính STT.\n+ No: Không tính STT.", "Confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.Yes)
                {
                    if(cboCalculateQuantity.IsChecked == true)
                    {
                        Pallet pallet = GetPallet(false);
                        Product product = GetProduct();
                        PrintLabel(pallet, product, true);
                        ShowPalletId(pallet.LineId.ToString(), txtNumOrderPlc.Text, pallet.NumOrder);
                        SavePalletLix(pallet, product); //TINH SAN LUONG
                    }
                    else // IN TEM KHONG TINH SAN LUONG
                    {
                        Pallet pallet = GetPallet(false);
                        Product product = GetProduct();
                        PrintLabel(pallet, product, true);
                        ShowPalletId(pallet.LineId.ToString(), txtNumOrderPlc.Text, pallet.NumOrder);
                    }
                }
                else if (dr == MessageBoxResult.No)
                {
                    Pallet pallet = GetPallet(true);
                    Product product = GetProduct();
                    PrintLabel(pallet, product, true);
                    ShowPalletId(pallet.LineId.ToString(), txtNumOrderPlc.Text, pallet.NumOrder);
                }
            }
        }

        private void PLC_OnPrintError()
        {

        }

        private void PLC_OnRePrint()
        {
            
        }

        private void PLC_OnLineChanged(int value)
        {
            txtLineId.Dispatcher.Invoke(new Action(() => 
            {
                txtLineId.Text = value.ToString();
            }));
        }

        private void PLC_OnOrderChanged(int value)
        {
            txtNumOrderPlc.Dispatcher.Invoke(new Action(() =>
            {
                txtNumOrderPlc.Text = value.ToString();
            }));
        }

        private void PLC_OnModelChanged(int value)
        {
            LoadProduct(value);
        }

        private void CboTeamId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = (TeamLixDTO)cboTeamId.SelectedItem;
        }

        private void CboShiftId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedShift = (ShiftLixDTO)cboShiftId.SelectedItem;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            CrudProduct(AppEnum.Search);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            CrudProduct(AppEnum.Delete);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            CrudProduct(AppEnum.Update);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            CrudProduct(AppEnum.Additional);
        }
        #endregion


        #region Methods

        private Account ConvertAccount(int line)
        {
            Account account = null;
            switch(line)
            {
                case 1:
                    account = new Account() { userName = "", passWord = "" };
                    break;
                case 2:
                    account = new Account() { userName = "", passWord = "" };
                    break;
                case 3:
                    account = new Account() { userName = "", passWord = "" };
                    break;
                case 4:
                    account = new Account() { userName = "", passWord = "" };
                    break;
                case 5:
                    account = new Account() { userName = "", passWord = "" };
                    break;
                case 6:
                    account = new Account() { userName = "", passWord = "" };
                    break;
                default:
                    break;

            }
            return account;
        }


        private async void SavePalletLix(Pallet palletToSave, Product productToSave)
        {
            PalletLixDTO palletLixDTO = new PalletLixDTO()
            {          
                productCode = palletToSave.LeverCode, //
                shiftId = palletToSave.ShiftId,
                teamId = palletToSave.TeamId,
                lineId = palletToSave.LineId, //
                reached = palletToSave.Reached,
                craft = palletToSave.Craft,
                numPallet = productToSave.NumBinsOnPallet, //
                numOrder = palletToSave.NumOrder,
                lot = palletToSave.Lot, //
                groupCode = palletToSave.GroupCode,
            };

            Authorization authorization = null;
            ResponseMessage response = null;

            try
            {
                Account newAccount = ConvertAccount(palletToSave.LineId);
                var taskLogin = LixcoAPI.Login(newAccount);
                response = await taskLogin;
                if (response != null)
                {
                    if (response.err == 0)
                    {
                        authorization = JsonConvert.DeserializeObject<Authorization>(response.dt.ToString());
                    }
                    else
                    {
                        DisplayReady(lbReady, string.Format("Login error! {0}", response.msg), AppEnum.Failured);
                        return;
                    }
                }

                //LineId API
                LineLixDTO lineLixDTO = LineLixs.lineDTOs.Where(o => o.num == palletToSave.LineId).FirstOrDefault();
                palletLixDTO.lineId = lineLixDTO.id;

                var taskSavePallet = LixcoAPI.SavePalletLix(authorization, palletLixDTO);
                response = await taskSavePallet;
                if (response != null)
                {
                    if (response.err == 0)
                    {
                        _palletService.SavePallet(palletToSave);
                        DisplayReady(lbReady, string.Format("{0} Lưu Pallet thành công! PalletId: {1} Model: {2}", DateTime.Now.ToString("HH:mm:ss"), txtPalletIdPC.Text, txtModel.Text), AppEnum.OK);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            txtOutput.Text = _palletService.CountPallet(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59")).ToString();
                        }));
                    }
                    else
                    {
                        DisplayReady(lbReady, string.Format("{0} Lưu Pallet thất bại, {1}. PalletId: {2} Model: {3}", DateTime.Now.ToString("HH:mm:ss"), response.msg, txtPalletIdPC.Text, txtModel.Text), AppEnum.NG);
                    }
                }

            }
            catch (Exception ex)
            {
                if (rdoManual.IsChecked == true)
                    MessageBox.Show(ex.Message, "SavePallet Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else 
                    DisplayReady(lbReady, string.Format("{0} {1}", "SavePallet Error", ex.Message), AppEnum.Failured);
            }

        }

        private double CalculateNetweight()
        {
            double netweight = 0.0;
            bool numPacketsOnBinRet = int.TryParse(txtNumPacketOnBin.Text, out int numPacketsOnBin);
            bool numBinsOnPalletRet = int.TryParse(txtNumBinOnPallet.Text, out int numBinsOnPallet);
            bool packetWeightRet = double.TryParse(txtPacketWeight.Text, out double packetWeight);
            if (!numPacketsOnBinRet)
            {
                MessageBox.Show("Sối túi trên 1 thùng không hợp lệ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNumBinOnPallet.Text = numPacketsOnBin.ToString();
                return netweight = (numPacketsOnBin * packetWeight * numBinsOnPallet) / 1000;
            }
            if (!numBinsOnPalletRet)
            {
                MessageBox.Show("Sối thùng trên 1 pallet không hợp lệ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNumBinOnPallet.Text = numBinsOnPallet.ToString();
                return netweight = (numPacketsOnBin * packetWeight * numBinsOnPallet) / 1000;
            }
            if (!packetWeightRet)
            {
                MessageBox.Show("Khối lượng của 1 túi không hợp lệ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPacketWeight.Text = packetWeight.ToString();
                return netweight = (numPacketsOnBin * packetWeight * numBinsOnPallet) / 1000;
            }
            return netweight = (numPacketsOnBin * packetWeight * numBinsOnPallet) / 1000;
        }

        /// <summary>
        /// Khi in ở chế độ Manual, phần mềm lấy DateTime từ textbox Datetime người dùng nhập vào
        /// Khi in ở chế độ Auto, phần mềm lấy thời gian thực
        /// </summary>
        /// <param name="palletToPrint"></param>
        /// <param name="productToPrint"></param>
        /// <param name="isManual">true: in ở chế độ manual, false: in ở chế độ auto</param>
        private void PrintLabel(Pallet palletToPrint, Product productToPrint, bool isManual)
        {
            try
            {
                int tsc_status = _tsc.GetStatus();
                if (tsc_status == 0)
                {
                    if (isManual)
                    {
                        DateTime dateTime = DateTime.Now; bool dateTimeIsParse = false;
                        dateTimeIsParse = DateTime.TryParse(txtDateTime.Text, out dateTime);
                        if (dateTimeIsParse)
                        {
                            _tsc.PrintLabel(palletToPrint, productToPrint, dateTime);
                        }
                        else
                        {
                            MessageBox.Show("Thời gian nhập không hợp lệ nhập vào không hợp lệ");
                        }
                    }
                    else
                    {
                        _tsc.PrintLabel(palletToPrint, productToPrint, DateTime.Now);
                    }

                }
                else if (tsc_status == 16)
                {
                    if (rdoManual.IsChecked == true)
                        MessageBox.Show(string.Format("TSC is pausing ({0})", tsc_status));
                    else
                        DisplayReady(lbReady, string.Format("TSC is pausing ({0})", tsc_status), AppEnum.Normally);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "PrintLabel Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetNumLot(int shiftId)
        {
            int _hour = Convert.ToInt32(DateTime.Now.Hour);
            string dateTime = DateTime.Now.ToString("ddMMyy");
            if (_hour < 6)
            {
                dateTime = DateTime.Now.AddDays(-1).ToString("ddMMyy");
            }
            return dateTime + shiftId.ToString() + "8";
        }

        private Pallet GetPallet(bool isReprint = false)
        {
            Pallet pallet = new Pallet();
            pallet.DateTime = DateTime.Now;
            pallet.ProductCode = txtProductCode.Text;
            pallet.LeverCode = txtLeverCode.Text;
            pallet.ShiftId = selectedShift == null?0: selectedShift.num;
            pallet.TeamId = selectedTeam == null?0: selectedTeam.id;
            bool lineRet = int.TryParse(txtLineId.Text, out int lineId);
            pallet.LineId = lineId;
            pallet.Reached = rdoReached.IsChecked.Value;
            pallet.Craft = false; //default
            pallet.NumOrder = GetNumOrder(txtLineId.Text, txtModel.Text, isReprint);
            pallet.Lot = GetNumLot(pallet.ShiftId);
            pallet.GroupCode = txtGroupCode.Text;
            pallet.FormName = txtFormName.Text;

            return pallet;
        }

        private Product GetProduct()
        {
            Product product = new Product();
            product.ProductCode = txtProductCode.Text;
            product.ProductName = new TextRange(txtProductName.Document.ContentStart, txtProductName.Document.ContentEnd).Text;
            bool netWeightRet = double.TryParse(txtNetweight.Text, out double netWeight);
            product.NetWeight = netWeight;
            bool numBinsOnPalletRet = int.TryParse(txtNumBinOnPallet.Text, out int numBinsOnPallet);
            product.NumBinsOnPallet = numBinsOnPallet;
            bool modelRet = int.TryParse(txtModel.Text, out int model);
            product.Model = model;
            bool packetWeightRet = double.TryParse(txtPacketWeight.Text, out double packettWeight);
            product.PacketWeight = packettWeight;
            bool numPacketsOnBinRet = int.TryParse(txtNumBinOnPallet.Text, out int numPacketsOnBin);
            product.NumPacketsOnBin = numPacketsOnBin;

            return product;
        }

        private void ManualLoadDefault()
        {
            txtPalletId.IsEnabled = true;
            txtPalletId.Background = Brushes.Azure;
            txtPalletIdPC.IsEnabled = false;

            txtLeverCode.IsEnabled = false;
            txtProductCode.IsEnabled = true;
            txtProductCode.Background = Brushes.Azure;
            txtProductName.IsEnabled = true;
            txtProductName.Background = Brushes.Azure;

            txtNumOrderPlc.IsEnabled = false;
            txtLineId.IsEnabled = true;
            txtLineId.Background = Brushes.Azure;
            txtModel.IsEnabled = true;
            txtModel.Background = Brushes.Azure;

            txtNetweight.IsEnabled = false;
            txtNumBinOnPallet.IsEnabled = true;
            txtNumBinOnPallet.Background = Brushes.Azure;
            txtNumBinOnPalletAPI.IsEnabled = false;
            txtPacketWeight.IsEnabled = true;
            txtPacketWeight.Background = Brushes.Azure;
            txtNumPacketOnBin.IsEnabled = true;
            txtNumPacketOnBin.Background = Brushes.Azure;

            txtDateTime.IsEnabled = true;
            txtDateTime.Background = Brushes.Azure;
            txtFormName.IsEnabled = true;
            txtFormName.Background = Brushes.Azure;
            txtGroupCode.IsEnabled = true;
            txtGroupCode.Background = Brushes.Azure;
            txtOutput.IsEnabled = false;

            btnPrint.IsEnabled = true;
            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnSearch.IsEnabled = true;

            _plc.EndReading();
        }

        private void AutoLoadDefault()
        {
            txtPalletId.IsEnabled = false;
            txtPalletId.Background = Brushes.Transparent;
            txtPalletIdPC.IsEnabled = false;

            txtLeverCode.IsEnabled = false;
            txtProductCode.IsEnabled = false;
            txtProductCode.Background = Brushes.Transparent;
            txtProductName.IsEnabled = false;
            txtProductName.Background = Brushes.Transparent;

            txtNumOrderPlc.IsEnabled = false;
            txtLineId.IsEnabled = false;
            txtLineId.Background = Brushes.Transparent;
            txtModel.IsEnabled = false;
            txtModel.Background = Brushes.Transparent;

            txtNetweight.IsEnabled = false;
            txtNumBinOnPallet.IsEnabled = false;
            txtNumBinOnPallet.Background = Brushes.Transparent;
            txtNumBinOnPalletAPI.IsEnabled = false;
            txtPacketWeight.IsEnabled = false;
            txtPacketWeight.Background = Brushes.Transparent;
            txtNumPacketOnBin.IsEnabled = false;
            txtNumPacketOnBin.Background = Brushes.Transparent;

            txtDateTime.IsEnabled = false;
            txtDateTime.Background = Brushes.Transparent;
            txtFormName.IsEnabled = false;
            txtFormName.Background = Brushes.Transparent;
            txtGroupCode.IsEnabled = false;
            txtGroupCode.Background = Brushes.Transparent;
            txtOutput.IsEnabled = false;

            btnPrint.IsEnabled = false;
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnSearch.IsEnabled = false;

            _plc.Status = PlcEnum.Ready;
            _plc.StartReading();
        }

        private void LoadDefault()
        {
            try
            {
                rdoManual.IsChecked = true;
                rdoReached.IsChecked = true;

                Product defaultProduct = new Product()
                {
                    NetWeight = 0.0,
                    NumBinsOnPallet = 0,
                    PacketWeight = 0.0,
                    NumPacketsOnBin = 0
                };

                txtNetweight.Text = defaultProduct.NetWeight.ToString();
                txtNumBinOnPallet.Text = defaultProduct.NumBinsOnPallet.ToString();
                txtPacketWeight.Text = defaultProduct.PacketWeight.ToString();
                txtNumPacketOnBin.Text = defaultProduct.NumPacketsOnBin.ToString();

                txtOutput.Text = _palletService.CountPallet(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59")).ToString();
                products = new List<Product>();
                products = _productService.GetAllProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void LoadShiftTeamLix()
        {         
            Authorization authorization = null;
            ResponseMessage response = null;

            try
            {
                var taskLogin = LixcoAPI.Login(account);
                response = await taskLogin;
                if (response != null)
                {
                    if (response.err == 0)
                    {
                        authorization = JsonConvert.DeserializeObject<Authorization>(response.dt.ToString());
                    }
                    else
                        MessageBox.Show(response.msg);
                }

                var taskGetTeam = LixcoAPI.GetTeamLix(authorization);
                response = await taskGetTeam;
                if (response != null)
                {
                    if (response.err == 0)
                    {
                        TeamLixs = JsonConvert.DeserializeObject<TeamLix>(response.dt.ToString());
                        cboTeamId.ItemsSource = TeamLixs.teamDTOs;
                        cboTeamId.DisplayMemberPath = "name";
                        cboTeamId.SelectedValuePath = "id";
                        cboTeamId.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show(response.msg);
                    }
                }

                var taskGetShift = LixcoAPI.GetShiftLix(authorization);
                response = await taskGetShift;
                if (response != null)
                {
                    if (response.err == 0)
                    {
                        ShiftLixs = JsonConvert.DeserializeObject<ShiftLix>(response.dt.ToString());
                        cboShiftId.ItemsSource = ShiftLixs.shiftDTOs;
                        cboShiftId.DisplayMemberPath = "name";
                        cboShiftId.SelectedValuePath = "num";
                        cboShiftId.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show(response.msg);
                    }
                }


                var taskGetLine = LixcoAPI.GetLineLix(authorization);
                response = await taskGetLine;
                if (response != null)
                {
                    if (response.err == 0)
                    {
                        LineLixs = JsonConvert.DeserializeObject<LineLix>(response.dt.ToString());
                    }
                    else
                    {
                        MessageBox.Show(response.msg);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LoadShiftTeamLix Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DisplayReady(lbReady, string.Format("{0} {1}", "LoadShiftTeamLix Error", ex.Message), AppEnum.Failured);
            }
        }

        private void ShowPalletId(string line, string orderPlc, int orderPc)
        {
            this.Dispatcher.Invoke(new Action(() => 
            { 
                txtPalletId.Text = string.Format("{0}_{1}", line, orderPlc);  
                txtPalletIdPC.Text = string.Format("{0}_{1}", line, orderPc);
            }));
        }

        private int GetNumOrder(string lineId, string model, bool isReptint)
        {
            int result = 0;
            bool lineRet = int.TryParse(lineId, out int numLine);
            bool numModelRet = int.TryParse(model, out int numModel);
            //int index = numModel - numLine * 100;
            int index = numModel;
            int numOrderVal = 0;
            switch (numLine)
            {
                case 1:
                    for (int i = 1; i < Properties.Settings.Default.Line1.Count; i++)
                    {
                        if (i == index)
                        {
                            numOrderVal = int.Parse(Properties.Settings.Default.Line1[index]);
                            if (!isReptint)
                            {
                                numOrderVal++;
                                result = numOrderVal;   
                            }
                            Properties.Settings.Default.Line1[index] = numOrderVal.ToString();
                            Properties.Settings.Default.Save();
                            return result;
                        }
                    }
                    break;
                case 2:
                    for (int i = 1; i < Properties.Settings.Default.Line2.Count; i++)
                    {
                        if (i == index)
                        {
                            numOrderVal = int.Parse(Properties.Settings.Default.Line2[index]);
                            if (!isReptint)
                            {
                                numOrderVal++;
                                result = numOrderVal;
                            }
                            Properties.Settings.Default.Line2[index] = numOrderVal.ToString();
                            Properties.Settings.Default.Save();
                            return result;
                        }
                    }
                    break;
                case 3:
                    for (int i = 1; i < Properties.Settings.Default.Line3.Count; i++)
                    {
                        if (i == index)
                        {
                            numOrderVal = int.Parse(Properties.Settings.Default.Line3[index]);
                            if (!isReptint)
                            {
                                numOrderVal++;
                                result = numOrderVal;
                            }
                            Properties.Settings.Default.Line3[index] = numOrderVal.ToString();
                            Properties.Settings.Default.Save();
                            return result;
                        }
                    }
                    break;
                case 4:
                    for (int i = 1; i < Properties.Settings.Default.Line4.Count; i++)
                    {
                        if (i == index)
                        {
                            numOrderVal = int.Parse(Properties.Settings.Default.Line4[index]);
                            if (!isReptint)
                            {
                                numOrderVal++;
                                result = numOrderVal;
                            }
                            Properties.Settings.Default.Line4[index] = numOrderVal.ToString();
                            Properties.Settings.Default.Save();
                            return result;
                        }
                    }
                    break;
                case 5:
                    for (int i = 1; i < Properties.Settings.Default.Line5.Count; i++)
                    {
                        if (i == index)
                        {
                            numOrderVal = int.Parse(Properties.Settings.Default.Line5[index]);
                            if (!isReptint)
                            {
                                numOrderVal++;
                                result = numOrderVal;
                            }
                            Properties.Settings.Default.Line5[index] = numOrderVal.ToString();
                            Properties.Settings.Default.Save();
                            return result;
                        }
                    }
                    break;
                case 6:
                    for (int i = 1; i < Properties.Settings.Default.Line6.Count; i++)
                    {
                        if (i == index)
                        {
                            numOrderVal = int.Parse(Properties.Settings.Default.Line6[index]);
                            if (!isReptint)
                            {
                                numOrderVal++;
                                result = numOrderVal;
                            }
                            Properties.Settings.Default.Line6[index] = numOrderVal.ToString();
                            Properties.Settings.Default.Save();
                            return result;
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// Load product info get from database and api
        /// </summary>
        /// <param name="model"></param>
        private async void LoadProduct(int model)
        {
            Authorization authorization = null;
            ResponseMessage response = null;

            try
            {
                txtModel.Dispatcher.Invoke(new Action(() =>
                {
                    txtModel.Text = model.ToString();
                }));

                Product product = _productService.GetProductByModel(model);
                if (product != null)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        txtProductCode.Text = product.ProductCode;
                        txtProductName.Document.Blocks.Clear();
                        txtProductName.Document.Blocks.Add(new Paragraph(new Run(product.ProductName)));
                        txtNumBinOnPallet.Text = product.NumBinsOnPallet.ToString();
                        txtPacketWeight.Text = product.PacketWeight.ToString();
                        txtNumPacketOnBin.Text = product.NumPacketsOnBin.ToString();
                        txtNetweight.Text = CalculateNetweight().ToString();
                    }));

                    var taskLogin = LixcoAPI.Login(account);
                    response = await taskLogin;
                    if (response != null)
                    {
                        if (response.err == 0)
                        {
                            authorization = JsonConvert.DeserializeObject<Authorization>(response.dt.ToString());
                        }
                        else
                        {
                            DisplayReady(lbReady, string.Format("Login error! {0}", response.msg), AppEnum.Failured);
                            return;
                        }
                    }

                    var taskGetProduct = LixcoAPI.GetProductLix(authorization, product.ProductCode);
                    response = await taskGetProduct;
                    if (response != null)
                    {
                        if (response.err == 0)
                        {
                            ProductLix productLix = JsonConvert.DeserializeObject<ProductLix>(response.dt.ToString());
                            Dispatcher.Invoke(new Action(() =>
                            {
                                txtNumBinOnPalletAPI.Text = productLix.productLixDTOs[0].numPallet.ToString();
                                txtLeverCode.Text = productLix.productLixDTOs[0].code;
                            }));
                        }
                        else
                        {
                            DisplayReady(lbReady, string.Format("Get product failed! Content: {0}", response.msg), AppEnum.Failured);
                            return;
                        }
                    }

                }
                else 
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        txtProductCode.Text = string.Empty;
                        txtLeverCode.Text = string.Empty;
                        txtProductName.Document.Blocks.Clear();
                        txtNumBinOnPallet.Text = "0";
                        txtPacketWeight.Text = "0.0";
                        txtNumPacketOnBin.Text = "0";
                        txtNetweight.Text = "0.0";
                        txtNumBinOnPalletAPI.Text = "0";
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion



        #region Connection methods

        private void ShowStatusTSC()
        {
            try
            {
                int status = _tsc.GetStatus();
                if(status == 0)
                {
                    DisplayStatus(pnPrinter, lbPrinter, AppEnum.Connected, AppEnum.Printer);
                }
                else if(status == 1)
                {
                    DisplayStatus(pnPrinter, lbPrinter, AppEnum.Pause, AppEnum.Printer);
                }
                else if(status == 16)
                {
                    DisplayStatus(pnPrinter, lbPrinter, AppEnum.HeadOpen, AppEnum.Printer);
                }
            }
            catch (Exception ex)
            {
                DisplayStatus(pnPrinter, lbPrinter, AppEnum.Failured, AppEnum.Printer);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ConnectToPLC()
        {
            try
            {
                _plc.Connect();
                if (_plc.Plc.IsConnected)
                {
                    DisplayStatus(pnController, lbController, AppEnum.Connected, AppEnum.Controller);
                }
                else
                {
                    DisplayStatus(pnController, lbController, AppEnum.Disconnected, AppEnum.Controller);
                }
            }
            catch (Exception ex)
            {
                DisplayStatus(pnController, lbController, AppEnum.Failured, AppEnum.Controller);
                MessageBox.Show(ex.Message, "ConnectToPLC Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayStatus(Ellipse pn, Label lb, AppEnum status, AppEnum deviceTypes)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (status == AppEnum.Connected)
                {
                    pn.Fill = Brushes.Lime;
                    lb.Content = string.Format("{0}: {1}", deviceTypes.ToString(), status.ToString());
                    lb.Foreground = Brushes.Green;
                }
                else if (status == AppEnum.Disconnected)
                {
                    pn.Fill = Brushes.LightGray;
                    lb.Content = string.Format("{0}: {1}", deviceTypes.ToString(), status.ToString());
                    lb.Foreground = Brushes.Black;
                }
                else if (status == AppEnum.Failured)
                {
                    pn.Fill = Brushes.Red;
                    lb.Content = string.Format("{0}: {1}", deviceTypes.ToString(), status.ToString());
                    lb.Foreground = Brushes.Red;
                }
            }));

        }

        private void DisplayPrinter(Label lb, AppEnum status, AppEnum deviceTypes)
        {
            Dispatcher.Invoke(new Action(() => 
            {
                if (status == AppEnum.Reprint)
                {
                    lb.Content = string.Format("{0}: {1}", deviceTypes.ToString(), status.ToString());
                    lb.Foreground = Brushes.Blue;
                }
                else if (status == AppEnum.Printerror)
                {
                    lb.Content = string.Format("{0}: {1}", deviceTypes.ToString(), status.ToString());
                    lb.Foreground = Brushes.Red;
                }
                else if (status == AppEnum.Idle)
                {
                    lb.Content = string.Format("{0}: {1}", deviceTypes.ToString(), AppEnum.Connected);
                    lb.Foreground = Brushes.Black;
                }
            }));

        }

        private void DisplayReady(Label lb, string message, AppEnum status)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (status == AppEnum.NG || status == AppEnum.Failured)
                {
                    lb.Content = string.Format("{0}", message);
                    lb.Foreground = Brushes.Red;
                }
                else if (status == AppEnum.Normally || status == AppEnum.Idle)
                {
                    lb.Content = string.Format("{0}", message);
                    lb.Foreground = Brushes.Black;
                }
                else if (status == AppEnum.OK)
                {
                    lb.Content = string.Format("{0}", message);
                    lb.Foreground = Brushes.Green;
                }
            }));
        }

        #endregion




        #region CRUD Methods
        private void CrudProduct(AppEnum options)
        {
            if (options == AppEnum.Additional || options == AppEnum.Update)
            {
                Product itemToAddOrUpdate = new Product();
                itemToAddOrUpdate.ProductCode = txtProductCode.Text;
                itemToAddOrUpdate.ProductName = new TextRange(txtProductName.Document.ContentStart, txtProductName.Document.ContentEnd).Text;
                bool netWeightRet = double.TryParse(txtNetweight.Text, out double netWeightVal);
                itemToAddOrUpdate.NetWeight = netWeightVal;
                bool packetWeightRet = double.TryParse(txtPacketWeight.Text, out double packetWeightVal);
                itemToAddOrUpdate.PacketWeight = packetWeightVal;
                bool numBinsOnPalletRet = int.TryParse(txtNumBinOnPallet.Text, out int numBinsOnPallet);
                itemToAddOrUpdate.NumBinsOnPallet = numBinsOnPallet;
                bool numPacketsOnBinRet = int.TryParse(txtNumPacketOnBin.Text, out int numPacketsOnBin);
                itemToAddOrUpdate.NumPacketsOnBin = numPacketsOnBin;
                bool modelRet = int.TryParse(txtModel.Text, out int model);
                itemToAddOrUpdate.Model = model;
                itemToAddOrUpdate.ProductImages = "";

                if (itemToAddOrUpdate.ProductCode == string.Empty)
                {
                    return;
                }
                if (netWeightRet == false)
                {
                    return;
                }
                if (packetWeightRet == false)
                {
                    return;
                }
                if (numBinsOnPalletRet == false)
                {
                    return;
                }
                if (numPacketsOnBinRet == false)
                {
                    return;
                }
                if (modelRet == false)
                {
                    return;
                }

                if (options == AppEnum.Additional)
                {
                    try
                    {
                        _productService.CreateProduct(itemToAddOrUpdate);
                        MessageBox.Show("Đã thêm mới sản phẩm.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "CrudProduct Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (options == AppEnum.Update)
                {
                    try
                    {
                        Product prod = _productService.UpdateProduct(itemToAddOrUpdate);
                        MessageBox.Show("Đã cập nhật sản phẩm: " + prod.ProductName.ToString(), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "CrudProduct Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else if (options == AppEnum.Delete || options == AppEnum.Search)
            {
                if (options == AppEnum.Delete)
                {
                    string productCodeToDelete = txtProductCode.Text;
                    try
                    {
                        Product prod = _productService.DeleteProduct(productCodeToDelete);
                        MessageBox.Show("Đã xóa sản phẩm: " + prod.ProductCode, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtProductCode.Text = string.Empty;
                        txtProductName.Document.Blocks.Clear();
                        txtNetweight.Text = string.Empty;
                        txtNumBinOnPallet.Text = string.Empty;
                        txtPacketWeight.Text = string.Empty;
                        txtNumPacketOnBin.Text = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "CrudProduct Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (options == AppEnum.Search)
                {
                    Product prod = products.Where(p => p.ProductCode == txtProductCode.Text.Trim()).FirstOrDefault();
                    if (prod != null)
                    {
                        txtProductName.Document.Blocks.Clear();
                        txtProductName.Document.Blocks.Add(new Paragraph(new Run(prod.ProductName)));
                        txtProductCode.Text = prod.ProductCode;
                        txtPacketWeight.Text = prod.PacketWeight.ToString();
                        txtNumPacketOnBin.Text = prod.NumPacketsOnBin.ToString();
                        txtNumBinOnPallet.Text = prod.NumBinsOnPallet.ToString();
                        txtNetweight.Text = CalculateNetweight().ToString();
                    }
                    else
                    {
                        txtProductName.Document.Blocks.Clear();
                        txtProductCode.Text = string.Empty;
                        txtPacketWeight.Text = "0";
                        txtNumPacketOnBin.Text = "0";
                        txtNumBinOnPallet.Text = "0";
                        txtNetweight.Text = "0";
                    }
                }
            }
        }
        #endregion

        private void MenuHistory_Click(object sender, RoutedEventArgs e)
        {
            HistoryForm frm = new HistoryForm();
            frm.Show();
        }
    }
}