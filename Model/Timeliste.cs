﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Jobbplan.Model
{
    public class Timeliste
    {
        public int BrukerId { get; set; }
        public decimal Timer { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public DateTime PeriodeStart { get; set; }
        public DateTime PeriodeSlutt { get; set; }
        public decimal Sum { get; set; }
        public int ProsjektId { get; set;}

    }

}