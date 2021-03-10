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

    public class ChartGasUpdate
    {
        // Singleton instance
        private readonly static Lazy<ChartGasUpdate> _instance = new Lazy<ChartGasUpdate>(() => new ChartGasUpdate());
        // Send Data every 1 seconds
        readonly int _updateInterval = 1000;
        //Timer Class
        private Timer _timer;
        private volatile bool _sendingChartData = false;
        private readonly object _chartUpateLock = new object();

        private ChartGasUpdate()
        {

        }

        public static ChartGasUpdate Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        // Calling this method starts the Timer
        public void GetChartGasData()
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
                    SendChartGasData();
                    _sendingChartData = false;
                }
            }
        }

        private void SendChartGasData()
        {
            var gasChart = new GasController().GetGasData().Data;
            var gasCngChart = new GasController().GetGasCNGData().Data;
            GetAllClients().All.UpdateChartGas(gasChart, gasCngChart);
        }

        private static dynamic GetAllClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ChartHub>().Clients;
        }
    }
}