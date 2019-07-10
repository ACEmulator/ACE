using System;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class HouseCallback
    {
        public House House;
        public Action<House> Callback;

        public HouseCallback(House house, Action<House> callback)
        {
            House = house;
            Callback = callback;
        }

        public void Run()
        {
            Callback(House);
        }
    }
}
