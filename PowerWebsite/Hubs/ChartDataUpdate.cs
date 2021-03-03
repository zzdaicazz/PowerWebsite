﻿using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PowerWebsite.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PowerWebsite.Hubs
{
    public class RandomNumberGenerator
    {
        static Random rnd1 = new Random();
        static public int randomScalingFactor()
        {

            return rnd1.Next(100);
        }
        static public int randomColorFactor()
        {

            return rnd1.Next(255);
        }
    }
    //The Line Chart Class
    public class LineChart
    {
        [JsonProperty("lineChartData")]
        private int[] lineChartData;
        [JsonProperty("colorString")]
        private string colorString;

        public void SetLineChartData()
        {
            lineChartData = new int[7];
            lineChartData[0] = RandomNumberGenerator.randomScalingFactor();
            lineChartData[1] = RandomNumberGenerator.randomScalingFactor();
            lineChartData[2] = RandomNumberGenerator.randomScalingFactor();
            lineChartData[3] = RandomNumberGenerator.randomScalingFactor();
            lineChartData[4] = RandomNumberGenerator.randomScalingFactor();
            lineChartData[5] = RandomNumberGenerator.randomScalingFactor();
            lineChartData[6] = RandomNumberGenerator.randomScalingFactor();

            colorString = "rgba(" + RandomNumberGenerator.randomColorFactor() + "," + RandomNumberGenerator.randomColorFactor() + "," + RandomNumberGenerator.randomColorFactor() + ",.3)";
        }
    }
    //The Pie Chart Class
    public class PieChart
    {
        [JsonProperty("value")]
        private int[] pieChartData;

        public void SetPieChartData()
        {
            pieChartData = new int[3];
            pieChartData[0] = RandomNumberGenerator.randomScalingFactor();
            pieChartData[1] = RandomNumberGenerator.randomScalingFactor();
            pieChartData[2] = RandomNumberGenerator.randomScalingFactor();

        }

    }
    public class ChartDataUpdate
    {
        // Singleton instance
        private readonly static Lazy<ChartDataUpdate> _instance = new Lazy<ChartDataUpdate>(() => new ChartDataUpdate());
        // Send Data every 5 seconds
        readonly int _updateInterval = 1000;
        //Timer Class
        private Timer _timer;
        private volatile bool _sendingChartData = false;
        private readonly object _chartUpateLock = new object();
        LineChart lineChart = new LineChart();
        PieChart pieChart = new PieChart();

        DateTime today = DateTime.Today.Date;
        DateTime startYesterdayTime = DateTime.Today.AddDays(-1); //Today at 00:00:00
        DateTime endYesterdayTime = DateTime.Today.AddTicks(-1); //Today at 23:59:59

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
        public void GetChartData()
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
                    SendChartData();
                    _sendingChartData = false;
                }
            }
        }

        private void SendChartData()
        {
            lineChart.SetLineChartData();
            pieChart.SetPieChartData();
            var gasChart = GetGasData();
            GetAllClients().All.UpdateChart(lineChart, pieChart, gasChart);

        }

        private static dynamic GetAllClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ChartHub>().Clients;
        }

        private IEnumerable<GasView> GetGasData()
        {
            using (DBModel db = new DBModel())
            {
                DateTime today = DateTime.Today.Date;
                var gas = db.gas.FirstOrDefault();
                var gas_recoder_begin = db.recoder_gas.Where(c => c.Thoigian >= startYesterdayTime && c.Thoigian <= endYesterdayTime).OrderByDescending(x => x.Thoigian)
                         .Take(1).ToList().FirstOrDefault();
                var gas_view = new GasView();
                if (gas != null)
                {
                    var gas_total_last = (gas_recoder_begin != null) ? gas_recoder_begin.luu_luong_tong : "0";
                    gas_view.luu_luong_hien_tai = ((float)Math.Round(float.Parse(gas.luu_luong_hien_tai) * 10f) / 10f).ToString();
                    gas_view.luu_luong_tong = ((float)Math.Round(float.Parse(gas.luu_luong_tong) * 10f) / 10f).ToString();
                    gas_view.luu_luong_tong_ngay = ((float)Math.Round((float.Parse(gas.luu_luong_tong) - float.Parse(gas_total_last)) * 10f) / 10f).ToString();
                    gas_view.status = gas.status;
                }
                yield return gas_view;
            }
        }

    }
}