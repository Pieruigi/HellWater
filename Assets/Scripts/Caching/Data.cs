using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    // Implement this class to let the caching system to read/write game objects from/to file system
    public abstract class Data
    {
        // Parse a gameobject data 
        public abstract void Parse(string data);

        // Format data to string 
        public abstract string Format();
    }

}
