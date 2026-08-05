using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SftpFileSync.Worker.Models
{
    public class ScheduleSettings
    {
        public string Cron { get; set; } = "* * * * *";
    }
}
