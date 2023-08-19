using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipDangerManagementSystem
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public class DangerEvent
        {
            public string RoomName { get; set; }
            public string DangerType { get; set; }
            public DateTime TimeStamp { get; set; }
            public bool IsDeleted { get; set; }

            public DangerEvent(string roomName, string dangerType, DateTime timeStamp, bool isDeleted = false)
            {
                RoomName = roomName;
                DangerType = dangerType;
                TimeStamp = timeStamp;
                IsDeleted = isDeleted;
            }
        }

    }
}
