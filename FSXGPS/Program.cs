using System;
using System.Windows.Forms;
using FsuipcSdk;
using FSXGPS.Services;

namespace FSXGPS
{
    static class Program
    {
        private static MainForm mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new MainForm();

            IGpsBroadcastService gpsBroadcastService = new GpsBroadcastService();
            using (IFlightSimulatorDataService flightSimulatorDataService = new FlightSimulatorDataService(new Fsuipc()))
            {
                using (var timer = new Timer { Interval = 1000, Enabled = true })
                {
                    timer.Tick += (sender, eventArgs) => Update(gpsBroadcastService, flightSimulatorDataService);

                    Application.Run(mainForm);
                }
            }
        }

        private static void Update(IGpsBroadcastService gpsBroadcastService, IFlightSimulatorDataService flightSimulatorDataService)
        {
            flightSimulatorDataService.Update();
            gpsBroadcastService.BroadcastData(flightSimulatorDataService);
            mainForm.lblStatus.Text = string.Format("Status: {0}", flightSimulatorDataService.Connected ? "Connected" : "Not connected");
        }
    }
}
