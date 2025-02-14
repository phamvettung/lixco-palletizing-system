using Intech.NarimePalletizingSystem.BLL.Services;
using Intech.NarimePalletizingSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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

namespace Intech.NarimePalletizingSystem.UI
{
    /// <summary>
    /// Interaction logic for HistoryForm.xaml
    /// </summary>
    public partial class HistoryForm : Window
    {
        PalletService _palletService = new();

        List<Pallet> pallets = new List<Pallet>();

        public HistoryForm()
        {
            InitializeComponent();
            btnSearch.Click += BtnSearch_Click;
            Loaded += HistoryForm_Loaded;
        }

        private void HistoryForm_Loaded(object sender, RoutedEventArgs e)
        {
            startTime.Text = "00:00:00";
            endTime.Text = "00:00:00";
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedStartDate = startDate.SelectedDate;
            DateTime? selectedEndDate = endDate.SelectedDate;
            string startDateStr = string.Empty, endDateStr = string.Empty;
            if (selectedStartDate.HasValue && selectedEndDate.HasValue)
            {
                startDateStr = selectedStartDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                endDateStr = selectedEndDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime startDateVal = DateTime.Parse(startDateStr + " " + startTime.Text);
                DateTime endDateVal = DateTime.Parse(endDateStr + " " + endTime.Text);
                pallets = _palletService.GetPalletByDate(startDateVal, endDateVal);
                LoadDataGrid();
            }
        }

        private void PalletDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void LoadDataGrid()
        {
            PalletDataGrid.ItemsSource = null;
            PalletDataGrid.ItemsSource = pallets;
        }

    }
}
