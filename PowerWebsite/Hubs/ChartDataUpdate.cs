﻿using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PowerWebsite.Controllers;
using PowerWebsite.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Web;

namespace PowerWebsite.Hubs
{
    
    public class ChartDataUpdate
    {
        // Singleton instance
        private readonly static Lazy<ChartDataUpdate> _instance = new Lazy<ChartDataUpdate>(() => new ChartDataUpdate());
        // Send Data every 1 seconds
        readonly int _updateInterval = 1000;
        //Timer Class
        private Timer _timer;
        private volatile bool _sendingChartData = false;
        private readonly object _chartUpateLock = new object();

        DateTime today = DateTime.Today.Date;
        static DateTime startYesterdayTime = DateTime.Today.AddDays(-1); //Today at 00:00:00
        static DateTime endYesterdayTime = DateTime.Today.AddTicks(-1); //Today at 23:59:59

        private static string kenh = "";

        private ChartDataUpdate()
        {

        }

        public static ChartDataUpdate Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        // Calling this method starts the Timer
        public void GetChartElectric1Data()
        {
            _timer = new Timer(ChartTimerCallBack, null, _updateInterval, _updateInterval);
        }
        private void ChartTimerCallBack(object state)
        {
            if (_sendingChartData)
            {
                return;
            }
            lock (_chartUpateLock)
            {
                if (!_sendingChartData)
                {
                    _sendingChartData = true;
                    SendChartElectric1Data();
                    // Cập nhập popup;
                    sendPopupHienThiData();
                    _sendingChartData = false;
                }
            }
        }

        private void SendChartElectric1Data()
        {
            var gasChart = new GasController().GetGasData().Data;
            var waterChart = new WaterController().GetWaterData().Data;
            var kenh1Chart = new HomeController().GetChartKenh1Data().Data;
            var kenh2Chart = new HomeController().GetChartKenh2Data().Data;
            var kenh3Chart = new HomeController().GetChartKenh3Data().Data;
            var kenh4Chart = new HomeController().GetChartKenh4Data().Data;
            var kenh5Chart = new HomeController().GetChartKenh5Data().Data;
            var kenh6Chart = new HomeController().GetChartKenh6Data().Data;
            GetAllClients().All.UpdateChartElectric1(gasChart, waterChart, kenh1Chart, kenh2Chart, kenh3Chart, kenh4Chart, kenh5Chart, kenh6Chart);
        }

        private void sendPopupHienThiData()
        {
            var dataHienthi = new HomeController().GetHienThiData(kenh).Data;
            GetAllClients().All.UpdatePopupHienThi(dataHienthi);
        }

        private static dynamic GetAllClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ChartHub>().Clients;
        }

        public string getKenhValue(string kenhValue)
        {
            return kenh = kenhValue;
        }

    }
}