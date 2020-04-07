using System;
namespace NetCode.Util
{
    class InvalidSystemClock : Exception
    {
        public InvalidSystemClock(string message) : base(message) { }
    }
}
