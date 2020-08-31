using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class ObjectData : Data
    {
        int state;

        public ObjectData(int state)
        {
            this.state = state;
        }

        public override string Format()
        {
            return state.ToString();
        }

        public override void Parse(string data)
        {
            state = int.Parse(data);
        }
    }

}
