using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class ObjectListData : Data
    {
        public class ObjectInfo
        {
            string code;
            public string Code
            {
                get { return code; }
            }
            int amount;
            public int Amount
            {
                get { return amount; }
            }

            public ObjectInfo(string code, int amount)
            {
                this.code = code;
                this.amount = amount;
            }
        }

        List<ObjectInfo> objects;
        public IList<ObjectInfo> Objects
        {
            get { return objects.AsReadOnly(); }
        }

        public ObjectListData()
        {
            objects = new List<ObjectInfo>();
        }

        public void AddObject(string code, int amount)
        {
            ObjectInfo oi = new ObjectInfo(code, amount);
            objects.Add(oi);
            
        }

        public override string Format()
        {
            string s = "";
            foreach(ObjectInfo oi in objects)
            {
                string tmp = oi.Code + "," + oi.Amount;
                if ("".Equals(s))
                    s = tmp;
                else
                    s += " " + tmp;
            }
            return s;
        }

        public override void Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;

            string[] sList = data.Split(' ');
            foreach(string s in sList)
            {
               
                string[] sp = s.Split(',');
                ObjectInfo oi = new ObjectInfo(sp[0], int.Parse(sp[1]));
                objects.Add(oi);
            }
        }
    }

}
