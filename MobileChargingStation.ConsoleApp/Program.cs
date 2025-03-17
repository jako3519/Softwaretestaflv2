using System;
using MobileChargingStation.Lib.Controller;  // Import the Controller namespace
using MobileChargingStation.Lib.Models;     // Import the Models namespace
using UsbSimulator;                         // Import the UsbSimulator namespace

namespace Ladeskab
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the required objects
            IDoor door = new Door(); // Assumes the Door class implements IDoor
            IUsbCharger usbCharger = new UsbChargerSimulator(); // Assumes the UsbChargerSimulator implements IUsbCharger
            RfidReader rfidReader = new RfidReader(); // Assumes RfidReader class implements IRfidReader

            // Create the StationControl instance
            StationControl stationControl = new StationControl(door, usbCharger, rfidReader);

            // Set RFID ID for the user
            stationControl.SetRfidId(123); // Example RFID ID

            // Simulate door status changes and RFID interactions
            Console.WriteLine("Starting station control simulation...");
            
           
           

           // door.SetDoorStatus(false, true, true, false);

            stationControl.DetectRfid(123);

            // Simulate door status change (open and unlocked)
            door.SetDoorStatus(true, true, true, false); // Door opened, unlocked, available for charging

            

            // Wait for user to press a key before exit
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
