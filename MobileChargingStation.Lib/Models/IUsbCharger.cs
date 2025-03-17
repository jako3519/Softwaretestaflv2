using System;

namespace UsbSimulator
{
    public class CurrentEventArgs : EventArgs
    {
        // Value in mA (milliAmpere)
        public double Current { set; get; }
    }

    public interface IUsbCharger
    {
        
        event EventHandler<CurrentEventArgs>? CurrentValueEvent;

        
        double CurrentValue { get; }

        
        bool Connected { get; }

        
        void StartCharge();
        
        void StopCharge();
    }

namespace UsbSimulator
{
    public class CurrentEventArgs : EventArgs
    {
        
        public double Current { set; get; }
    }

    public interface IUsbCharger
    {
       
        event EventHandler<CurrentEventArgs>? CurrentValueEvent;

        
        double CurrentValue { get; }

        
        bool Connected { set; get; }

        
        void StartCharge();
        
        void StopCharge();
    }
}}