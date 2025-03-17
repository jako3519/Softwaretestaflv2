using System;

namespace MobileChargingStation.Lib.Models
{
    public class Door : IDoor
    {
        public event EventHandler<DoorChangedEventArgs>? DoorChangedEvent;

        private bool _doorOpen;
        
        private bool _doorUnlocked;
        private bool _doorAvailable;

        private bool _recharging;

        public void SetDoorStatus(bool doorOpen,  bool doorUnlocked, bool doorAvailable, bool recharging)
        {
            if (doorOpen != _doorOpen  || doorUnlocked != _doorUnlocked || doorAvailable != _doorAvailable || recharging != _recharging)
            {
                _doorOpen = doorOpen;
               
                _doorUnlocked = doorUnlocked;

                _doorAvailable = doorAvailable;

                _recharging = recharging;

                OnDoorChanged(new DoorChangedEventArgs { DoorOpen = _doorOpen,  DoorUnlocked = _doorUnlocked, DoorAvailable = _doorAvailable, recharging = _recharging });

               
            }
        }

        
        

        protected virtual void OnDoorChanged(DoorChangedEventArgs e)
        {
            DoorChangedEvent?.Invoke(this, e);
        }
    }

    public class DoorChangedEventArgs : EventArgs
    {
        public bool DoorOpen { get; set; }

        
        public bool DoorUnlocked { get; set; }
        public bool DoorAvailable { get; set; }

        public bool recharging { get; set; }
    }

    public interface IDoor
    {
        event EventHandler<DoorChangedEventArgs> DoorChangedEvent;
        void SetDoorStatus(bool doorOpen, bool doorUnlocked, bool doorAvailable, bool recharging);
    }
}
