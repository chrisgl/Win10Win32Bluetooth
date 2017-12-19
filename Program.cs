using System;

using Windows.Devices.Bluetooth.Advertisement;

namespace Win10Win32Bluetooth
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start the program
            var program = new Program();

            // Close on key press
            Console.ReadLine();
        }

        public Program()
        {
            // Create Bluetooth Listener
            var watcher = new BluetoothLEAdvertisementWatcher();

            watcher.ScanningMode = BluetoothLEScanningMode.Active;

            // Only activate the watcher when we're recieving values >= -80
            watcher.SignalStrengthFilter.InRangeThresholdInDBm = -80;

            // Stop watching if the value drops below -90 (user walked away)
            watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -90;

            // Register callback for when we see an advertisements
            watcher.Received += OnAdvertisementReceived;

            // Wait 5 seconds to make sure the device is really out of range
            watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);

            // Starting watching for advertisements
            watcher.Start();
        }

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            // Tell the user we see an advertisement and print some properties
            Console.WriteLine("Advertisement:");
            Console.WriteLine($"  TIME:     {eventArgs.Timestamp}");
            Console.WriteLine($"  ADVTYPE:  {eventArgs.AdvertisementType}");
            Console.WriteLine($"  BT_ADDR:  {eventArgs.BluetoothAddress}");
            Console.WriteLine($"  FR_NAME:  {eventArgs.Advertisement.LocalName}");
            Console.WriteLine($"  STRENGTH: {eventArgs.RawSignalStrengthInDBm} DBm");
            foreach (var serviceGUID in eventArgs.Advertisement.ServiceUuids)
            {
                Console.WriteLine($"  S_GUID:  {serviceGUID}");
            }
            foreach (var data in eventArgs.Advertisement.ManufacturerData)
            {
                Console.WriteLine($"  COMP_ID:  {data.CompanyId}");
                Console.WriteLine($"  DATA_CAP: {data.Data.Capacity}");
                Console.WriteLine($"  DATA_LEN: {data.Data.Length}");
            }
        }
    }
}
