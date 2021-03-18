﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PowerWebsite.Models
{
    [Table("Hienthi1")]
    public class Hienthi1
    {
        [Key]
        public string Kenh { get; set; }
        public string Vca { get; set; }
        public string Vbc { get; set; }
        public string Vab { get; set; }
        public string Vun { get; set; }
        public string VII { get; set; }
        public string Cac { get; set; }
        public string Cab { get; set; }
        public string Caa { get; set; }
        public string Civg { get; set; }
        public string Pc { get; set; }
        public string Pb { get; set; }
        public string Pa { get; set; }
        public string Ptotal { get; set; }
        public string Pkvar { get; set; }
        public string Pkva { get; set; }
        public string Van { get; set; }
        public string Vbn { get; set; }
        public string Vcn { get; set; }
        public string F { get; set; }
        public string PF { get; set; }
        public string Kwh { get; set; }
        public string Vin { get; set; }
        public int status { get; set; }
    }

    public class Hienthi1View
    {
        [Key]
        public string Kenh { get; set; }
        public string Vca { get; set; }
        public string Vbc { get; set; }
        public string Vab { get; set; }
        public string Vun { get; set; }
        public string VII { get; set; }
        public string Cac { get; set; }
        public string Cab { get; set; }
        public string Caa { get; set; }
        public string Civg { get; set; }
        public string Pc { get; set; }
        public string Pb { get; set; }
        public string Pa { get; set; }
        public string Ptotal { get; set; }
        public string Pkvar { get; set; }
        public string Pkva { get; set; }
        public string Van { get; set; }
        public string Vbn { get; set; }
        public string Vcn { get; set; }
        public string F { get; set; }
        public string PF { get; set; }
        public string Kwh { get; set; }
        public string Vin { get; set; }
        public int status { get; set; }
        public string KwhDay { get; set; }
    }
    public class Hienthi1OverView
    {
        [Key]
        public string Kenh { get; set; }
        public string Ptotal { get; set; }
        public string Kwh { get; set; }
        public string KWhDay { get; set; }
    }

}