using Intech.NarimePalletizingSystem.DAL.Entities;
using Intech.NarimePalletizingSystem.UI.Configurations;
using Intech.NarimePalletizingSystem.UI.Logging;
using Intech.NarimePalletizingSystem.UI.Models;
using Intech.NarimePalletizingSystem.UI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Intech.NarimePalletizingSystem.UI.Systems
{
    public class TSC
    {
        TscConfig config = null;
        public TSC()
        {
            config = AppConfig.GetTscConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pallet"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public int PrintLabel(Pallet pallet, Product product, DateTime dateTime)
        {
            int status = -1;
            try
            {
                status = GetStatus();
                if(status == 0)
                {
                    var team = MainWindow.TeamLixs.teamDTOs.Where(p => p.id == pallet.TeamId).FirstOrDefault();

                    byte[] form_name = System.Text.Encoding.GetEncoding("utf-16").GetBytes(pallet.FormName);
                    byte[] pallet_shift = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("PALLET: {0}_{1}  CA: {2}", pallet.LineId, pallet.NumOrder, pallet.ShiftId));
                    byte[] product_code = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("MÃ SẢN PHẨM: {0}", pallet.ProductCode));
                    byte[] product_name = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("TÊN SẢN PHẨM:  {0}", product.ProductName));

                    byte[] date, time;
                    if (dateTime == null)
                    {
                        date = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("NGÀY:  {0}", DateTime.Now.ToString("dd/MM/yyyy")));
                        time = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("GIỜ:  {0}", DateTime.Now.ToString("HH:mm")));
                    }
                    else
                    {
                        date = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("NGÀY:  {0}", dateTime.ToString("dd/MM/yyyy")));
                        time = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("GIỜ:  {0}", dateTime.ToString("HH:mm")));
                    }

                    byte[] team_id = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("TỔ SẢN XUẤT: {0}", team == null ? string.Empty : team.name));
                    byte[] net_weight = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("KL NETWEIGHT: {0}  Kg", product.NetWeight));
                    byte[] num_bins_on_pallet = System.Text.Encoding.GetEncoding("utf-16").GetBytes(string.Format("SỐ THÙNG TRÊN PALLET:  {0}", product.NumBinsOnPallet));

                    //TSCLIB_DLL.openport("TSC PEX-1231");
                    //TSCLIB_DLL.sendcommand("SIZE 100 mm, 150 mm");
                    //TSCLIB_DLL.sendcommand("SPEED 1");
                    //TSCLIB_DLL.sendcommand("DENSITY 12");
                    //TSCLIB_DLL.sendcommand("DIRECTION 1");
                    //TSCLIB_DLL.sendcommand("SET TEAR ON");
                    //TSCLIB_DLL.sendcommand("CODEPAGE UTF-8");
                    //TSCLIB_DLL.clearbuffer();

                    //TSCLIB_DLL.openport(config.Port);
                    //TSCLIB_DLL.sendcommand(config.Size);
                    //TSCLIB_DLL.sendcommand(config.Speed);
                    //TSCLIB_DLL.sendcommand(config.Density);
                    //TSCLIB_DLL.sendcommand(config.Direction);
                    //TSCLIB_DLL.sendcommand(config.Tear);
                    //TSCLIB_DLL.sendcommand(config.Codepage);
                    //TSCLIB_DLL.clearbuffer();

                    byte status1 = TSCLIB_DLL.usbportqueryprinter();
                    TSCLIB_DLL.openport("usb");
                    TSCLIB_DLL.sendcommand("SIZE 100 mm, 150 mm");
                    TSCLIB_DLL.sendcommand("SPEED 4");
                    TSCLIB_DLL.sendcommand("DENSITY 12");
                    TSCLIB_DLL.sendcommand("DIRECTION 1");
                    TSCLIB_DLL.sendcommand("SET TEAR ON");
                    TSCLIB_DLL.sendcommand("CODEPAGE UTF-8");
                    TSCLIB_DLL.clearbuffer();

                    //TSCLIB_DLL.windowsfontUnicode(50, 550, 48, 90, 2, 0, "Arial", form_name);

                    //TSCLIB_DLL.windowsfontUnicode(150, 1650, 48 * 3, 90, 2, 0, "Arial", pallet_shift);
                    //TSCLIB_DLL.windowsfontUnicode(370, 1450, 48 * 2, 90, 2, 0, "Arial", product_code);
                    //TSCLIB_DLL.windowsfontUnicode(550, 1750, 48, 90, 2, 0, "Arial", product_name);
                    //TSCLIB_DLL.windowsfontUnicode(650, 1750, 48, 90, 2, 0, "Arial", date);
                    //TSCLIB_DLL.windowsfontUnicode(750, 1750, 48, 90, 2, 0, "Arial", time);
                    //TSCLIB_DLL.windowsfontUnicode(850, 1750, 48, 90, 2, 0, "Arial", team_id);
                    //TSCLIB_DLL.windowsfontUnicode(950, 1750, 48, 90, 2, 0, "Arial", net_weight);
                    //TSCLIB_DLL.windowsfontUnicode(1050, 1750, 48, 90, 2, 0, "Arial", num_bins_on_pallet);

                    //TSCLIB_DLL.windowsfontUnicode(150, 1650, 48 * 3, 90, 0, 0, "Arial", pallet_shift);
                    //TSCLIB_DLL.windowsfontUnicode(370, 1450, 48 * 2, 90, 0, 0, "Arial", product_code);
                    //TSCLIB_DLL.windowsfontUnicode(550, 1750, 48, 90, 0, 0, "Arial", product_name);
                    //TSCLIB_DLL.windowsfontUnicode(650, 1750, 48, 90, 0, 0, "Arial", date);
                    //TSCLIB_DLL.windowsfontUnicode(750, 1750, 48, 90, 0, 0, "Arial", time);
                    //TSCLIB_DLL.windowsfontUnicode(850, 1750, 48, 90, 0, 0, "Arial", team_id);
                    //TSCLIB_DLL.windowsfontUnicode(950, 1750, 48, 90, 0, 0, "Arial", net_weight);
                    //TSCLIB_DLL.windowsfontUnicode(1050, 1750, 48, 90, 0, 0, "Arial", num_bins_on_pallet);


                    //OK
                    //TSCLIB_DLL.windowsfontUnicode(150, 1450, 48 * 3, 90, 0, 0, "Arial", pallet_shift);
                    //TSCLIB_DLL.windowsfontUnicode(370, 1150, 48 * 2, 90, 0, 0, "Arial", product_code);
                    //TSCLIB_DLL.windowsfontUnicode(550, 1550, 48, 90, 0, 0, "Arial", product_name);
                    //TSCLIB_DLL.windowsfontUnicode(650, 1550, 48, 90, 0, 0, "Arial", date);
                    //TSCLIB_DLL.windowsfontUnicode(750, 1550, 48, 90, 0, 0, "Arial", time);
                    //TSCLIB_DLL.windowsfontUnicode(850, 1550, 48, 90, 0, 0, "Arial", team_id);
                    //TSCLIB_DLL.windowsfontUnicode(950, 1550, 48, 90, 0, 0, "Arial", net_weight);
                    //TSCLIB_DLL.windowsfontUnicode(1050, 1550, 48, 90, 0, 0, "Arial", num_bins_on_pallet);

                    TSCLIB_DLL.windowsfontUnicode(150, 1550, 48 * 3, 90, 2, 0, "Arial", pallet_shift);
                    TSCLIB_DLL.windowsfontUnicode(370, 1450, 48 * 2, 90, 2, 0, "Arial", product_code);
                    TSCLIB_DLL.windowsfontUnicode(550, 1550, 48, 90, 2, 0, "Arial", product_name);
                    TSCLIB_DLL.windowsfontUnicode(650, 1550, 48, 90, 2, 0, "Arial", date);
                    TSCLIB_DLL.windowsfontUnicode(750, 1550, 48, 90, 2, 0, "Arial", time);
                    TSCLIB_DLL.windowsfontUnicode(850, 1550, 48, 90, 2, 0, "Arial", team_id);
                    TSCLIB_DLL.windowsfontUnicode(950, 1550, 48, 90, 2, 0, "Arial", net_weight);
                    TSCLIB_DLL.windowsfontUnicode(1050, 1550, 48, 90, 2, 0, "Arial", num_bins_on_pallet);

                    TSCLIB_DLL.printlabel("1", "1");
                    TSCLIB_DLL.closeport();

                    Logger.WriteLog(string.Format("Pallet Infor: {0}. Profuct Infor: {1}", pallet.ToString(), product.ToString()), Microsoft.Extensions.Logging.LogLevel.Information, LogAction.PRINTING);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Printing error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {

            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>0 = idle, 1 = head open, 16 = pause</returns>
        public int GetStatus()
        {
            byte status = TSCLIB_DLL.usbportqueryprinter();
            return status;
        }
    }
}
