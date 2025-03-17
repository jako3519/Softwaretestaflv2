using System;

namespace MobileChargingStation.Lib.Models

{
    public class RfidReader : IRfidReader
    {
        public event EventHandler<RfidDetectedChangedEventArgs>? RfidChangedEvent;

        private int _rfidId = 123;

        public bool DetectRfid(int rfidID)
        {
            int attempts = 0;
            const int maxAttempts = 3;

            while (attempts < maxAttempts)
            {
                if (rfidID == _rfidId)
                {
                    OnRfidChanged(new RfidDetectedChangedEventArgs { RfidID = rfidID });
                    return true; 
                }
                else
                {
                    Console.WriteLine("RFID not recognized, try again");
                    attempts++;
                    if (attempts == maxAttempts)
                    {
                        Console.WriteLine("Maximum attempts reached. Access denied.");
                        return false; // Maximum attempts reached, RFID detection failed
                    }
                }
            }
            return false; 
        }

        protected virtual void OnRfidChanged(RfidDetectedChangedEventArgs e)
        {
            RfidChangedEvent?.Invoke(this, e);
        }
    }

    public class RfidDetectedChangedEventArgs : EventArgs
    {
        public int RfidID { get; set; }
    }

    public interface IRfidReader
    {
        event EventHandler<RfidDetectedChangedEventArgs> RfidChangedEvent;
        bool DetectRfid(int rfidID);
    }
}
