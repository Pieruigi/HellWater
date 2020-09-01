using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class FiniteStateMachineData : Data
    {
        int stateId;
        public int StateId
        {
            get { return stateId; }
        }

        public FiniteStateMachineData() { }

        public FiniteStateMachineData(int stateId)
        {
            this.stateId = stateId;
        }

        public override string Format()
        {
            return stateId.ToString();
        }

        public override void Parse(string data)
        {
            stateId = int.Parse(data);
        }
    }

}
