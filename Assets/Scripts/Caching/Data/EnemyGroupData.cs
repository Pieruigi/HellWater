using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class EnemyGroupData : Data
    {
        int state;
        public int State
        {
            get { return state; }
            
        }
        List<bool> deadList;
        public IList<bool> DeadList
        {
            get { return deadList.AsReadOnly(); }
        }

        public EnemyGroupData() 
        {
            deadList = new List<bool>();
        }

        public EnemyGroupData(int state, List<bool> deadList) : base()
        {
            this.state = state;
            this.deadList = deadList;
        }

        public override string Format()
        {
            string ret = state.ToString();
            if(deadList != null)
            {
                for (int i = 0; i < deadList.Count; i++)
                {
                    ret += deadList[i] ? ",1" : ",0";
                }
            }
            
            return ret;
        }

        public override void Parse(string data)
        {
            // Get the fsm state.
            string[] s = data.Split(',');
            state = int.Parse(s[0]);

            // Parse the dead list.
            //deadList = new List<bool>();
            for(int i=1; i<s.Length; i++)
            {
                deadList.Add("0".Equals(s[i]) ? false : true);
            }
        }


    }

}
