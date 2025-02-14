using Intech.NarimePalletizingSystem.UI.Configurations;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Intech.NarimePalletizingSystem.UI.Systems
{
    public enum PlcEnum
    {
        Ready = 0,
        End = 1,
        ModelChanged = 2,
        LineChanged = 3,
        OrderChanged = 4,
        PrintChanged = 5
    }
    public class PLC
    {

        #region Fields
        private PlcConfig configuration = AppConfig.GetPlcConfig();
        Thread readingDataThread = null;
        private DataControl _dataControlActived;
        #endregion

        #region Properties
        public DataControl DataControl { get; set; }
        public Plc Plc { get; set; }
        public PlcEnum Status { get; set; }
        #endregion

        #region Delegates
        public delegate void ModelChanged(int value);
        public static event ModelChanged OnModelChanged;
        public delegate void OrderChanged(int value);
        public static event OrderChanged OnOrderChanged;
        public delegate void LineChanged(int value);
        public static event LineChanged OnLineChanged;
        public delegate void PrintStarted();
        public static event PrintStarted OnPrintStarted;
        public delegate void Reprint();
        public static event Reprint OnRePrint;
        public delegate void PrintError();
        public static event Reprint OnPrintError;
        #endregion

        public PLC()
        {
            DataControl = new DataControl();
            _dataControlActived = new DataControl();
            Status = PlcEnum.End;

            this.Plc = new Plc(configuration.CpuType, configuration.IpAddress, configuration.Rack, configuration.Slot);
        }

        public void Connect()
        {
            try
            {
                if (!Plc.IsConnected)
                {
                    Plc.Open();
                    if (Plc.IsConnected)
                    {
                        Status = PlcEnum.Ready;
                    }
                }
            }
            catch (PlcException e)
            {

                throw e;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (Plc.IsConnected)
                {
                    Plc.Close();
                    Status = PlcEnum.End;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void StartReading()
        {
            if (Plc.IsConnected && Status == PlcEnum.Ready)
            {
                readingDataThread = new Thread(ReadData);
                readingDataThread.IsBackground = true;
                readingDataThread.Start();
            }
        }

        public void EndReading()
        {
            Status = PlcEnum.End;
        }


        public void ReadData()
        {
            try
            {
                while (Plc.IsConnected && Status != PlcEnum.End)
                {
                    Plc.ReadClass(DataControl, 113);
                    if(DataControl.model != _dataControlActived.model)
                    {
                        _dataControlActived.model = DataControl.model;
                        OnModelChanged(DataControl.model);
                    }

                    if (DataControl.order != _dataControlActived.order)
                    {
                        _dataControlActived.order = DataControl.order;
                        OnOrderChanged(DataControl.order);
                    }

                    if (DataControl.lineId != _dataControlActived.lineId)
                    {
                        _dataControlActived.lineId = DataControl.lineId;
                        OnLineChanged(DataControl.lineId);
                    }

                    if (DataControl.print == true && _dataControlActived.print == false)
                    {
                        _dataControlActived.print = true;
                        OnPrintStarted();
                    }
                    else if(DataControl.print == false)
                    {
                        _dataControlActived.print = false;
                    }

                    if (DataControl.printError == true && _dataControlActived.printError == false)
                    {
                        _dataControlActived.printError = true;
                        OnPrintError();
                    }
                    else if (DataControl.printError == false)
                    {
                        _dataControlActived.printError = false;
                    }

                    if (DataControl.reprint == true && _dataControlActived.reprint == false)
                    {
                        _dataControlActived.reprint = true;
                        OnRePrint();
                    }
                    else if (DataControl.printError == false)
                    {
                        _dataControlActived.reprint = false;
                    }

                    Thread.Sleep(100);
                }
            }
            catch (PlcException e)
            {
                Disconnect();
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
