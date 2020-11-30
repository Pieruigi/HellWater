using HW.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CircuitSum : ICircuitOperation
    {

        int sumBase = 2; // Binary.

        static CircuitSum instance;
        public static CircuitSum Instance
        {
            get { if (instance == null) instance = new CircuitSum(); return instance; }
            private set { instance = value; }
        }
        
        public string Compute(string[] inputs)
        {
            string output = null;
            int numOfElements = inputs[0].Split(' ').Length;
            // Split element by element in output.
            for (int i = 0; i < numOfElements; i++)
            {
                // Sum of element i-th.
                int sum = 0;
                foreach (string input in inputs)
                {
                    int s = int.Parse(input.Split(' ')[i]);

                    sum += s;
                    sum = sum % sumBase;
                }

                //outputs[i].ForceState(sum, true, false);
                if (string.IsNullOrEmpty(output))
                    output = sum.ToString();
                else
                    output += " " + sum.ToString();

            }

            return output;
        }

    }

}
