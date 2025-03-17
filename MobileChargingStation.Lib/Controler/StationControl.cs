using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MobileChargingStation.Lib.Models;
using UsbSimulator;

namespace MobileChargingStation.Lib.Controller

{
    public class StationControl
    {
        private readonly IDoor _door;
        private readonly ChargeControl _chargeControl;
        private readonly RfidReader _rfidReader;
        private int _currentRfidId;

        public StationControl(IDoor door, IUsbCharger usbCharger, RfidReader rfidReader)
        {
            _door = door;
            _chargeControl = new ChargeControl(usbCharger);
            _rfidReader = rfidReader;

            _door.DoorChangedEvent += HandleDoorChanged;
        }

        private void HandleDoorChanged(object? sender, DoorChangedEventArgs e)
        {
            //Console.WriteLine($"Door status changed - Open: {e.DoorOpen}, Unlocked: {e.DoorUnlocked}, Available: {e.DoorAvailable}");
            HandleDoorStatus(e.DoorOpen, e.DoorUnlocked, e.DoorAvailable, e.recharging);
        }

        private void HandleDoorStatus(bool doorOpen, bool doorUnlocked, bool doorAvailable, bool recharging)
        {
            switch ((doorOpen, doorUnlocked, doorAvailable, recharging))
            {
                case (false, true, true, false):
                    Console.WriteLine("Door is closed, but unlocked, and available.");
                    break;

                case (true, true, true, false):
                    Console.WriteLine("tislut svinet");
                    if (_chargeControl.IsConnected())
                    {
                        LockDoor();
                    }
                    else
                    {
                        Console.WriteLine("No device connected");
                    }
                    break;

                case (false, false, false, false):
                    Console.WriteLine("Door is closed and locked.");
                    Console.WriteLine("Scan your RFID");
                    bool rfidDetected = _rfidReader.DetectRfid(_currentRfidId);

                    if (rfidDetected)
                    {
                        _chargeControl.StartCharging();
                        ChargingDoor();
                    }
                    else
                    {
                        Console.WriteLine("RFID detection failed. Access denied.");
                    }
                    break;

                case (false, false, false, true):
                    Console.WriteLine("Scan your RFID");
                    bool rfidDetectedOut = _rfidReader.DetectRfid(_currentRfidId);
                    if (rfidDetectedOut)
                    {
                        _chargeControl.StopCharging();
                        UnlockDoor();
                    }
                    else
                    {
                        Console.WriteLine("RFID detection failed. Access denied.");
                    }
                    break;

                default:
                    Console.WriteLine("Unexpected door status.");
                    break;
            }
        }

        public void LockDoor()
        {
            Console.WriteLine("Locking the door...");
            _door.SetDoorStatus(false, false, false, false);
        }

        public void UnlockDoor()
        {
            Console.WriteLine("Unlocking the door...");
            _door.SetDoorStatus(false, true, true, false);
        }

        public void OpenDoor()
        {
            Console.WriteLine("Opening the door...");
            _door.SetDoorStatus(true, false, false, false);
        }

        public void ChargingDoor()
        {
            Console.WriteLine("Charging in progress...");
            _door.SetDoorStatus(false, false, false, true);
        }

        public void SetRfidId(int rfidID)
        {
            _currentRfidId = rfidID;
        }

        public void DetectRfid(int rfidID)
        {
            _rfidReader.DetectRfid(rfidID);
        }
    }

    public class ChargeControl
    {
        private readonly IUsbCharger _usbCharger;

        public ChargeControl(IUsbCharger usbCharger)
        {
            _usbCharger = usbCharger;
            _usbCharger.CurrentValueEvent += HandleCurrentValueChanged;
        }

        private void HandleCurrentValueChanged(object? sender, CurrentEventArgs e)
        {
            Console.WriteLine($"USB Current changed: {e.Current} mA");
        }

        public void StartCharging()
        {
            if (_usbCharger.Connected)
            {
                Console.WriteLine("Starting USB charging...");
                _usbCharger.StartCharge();
            }
            else
            {
                Console.WriteLine("No device connected, cannot start charging.");
            }
        }

        public void StopCharging()
        {
            Console.WriteLine("Stopping USB charging...");
            _usbCharger.StopCharge();
        }

        public bool IsConnected()
        {
            return _usbCharger.Connected;
        }
    }
}