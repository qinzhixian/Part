using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogCenter
{
    public class LogModel
    {
        public string Content { get; set; }

        public Util.LogType LogType { get; set; }

        public DateTime AddTime { get; set; }
    }
}