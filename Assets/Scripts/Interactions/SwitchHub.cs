using HW.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    /// <summary>
    /// Sum the output given by circuits in the input state machine in order to set the output.
    /// Circuit output depends on the corresponding fsm state.
    /// </summary>
    public class SwitchHub : MonoBehaviour
    {
        // State machines in input.
        [SerializeField]
        List<FiniteStateMachine> inputs;

        // State machine to set.
        [SerializeField]
        List<FiniteStateMachine> outputs;

        [SerializeField]
        int sumBase = 2; // Binary ( 0 / 1 )

        // Each input has a circuit in order to set the output depending on its state.
        List<Circuit> circuits = new List<Circuit>();


        private void Awake()
        {
            


            foreach (FiniteStateMachine fsm in inputs)
            {
                // Get the circuite that defines the output on each given finite state machine.
                circuits.Add(fsm.GetComponent<Circuit>());

                // Set the handle.
                fsm.OnStateChange += HandleOnStateChange;

            }
            
           
        }

        // Start is called before the first frame update
        void Start()
        {
            ComputeOutput();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            
            ComputeOutput();
        }

        void ComputeOutput()
        {
            // Get the output for each finite state machine we have in input.
            // Each circuit output will be sent as input to the operator.
            string[] outArray = new string[inputs.Count];
            for(int i=0; i< outArray.Length; i++)
            {
                // Get the next state machine output.
                outArray[i] = circuits[i].GetOutput(inputs[i].CurrentStateId.ToString());
            }

            // Compute operation.
            string output = CircuitSum.Instance.Compute(outArray);
            Debug.Log("Output:" + output);
            if (!string.IsNullOrEmpty(output))
            {
                string[] outS = output.Split(' ');
                for(int i=0; i<outS.Length; i++)
                {
                    outputs[i].ForceState(int.Parse(outS[i]), true, false);
                }
            }
            

            //int numOfElements = outArray[0].Split(' ').Length;
            //// Split element by element in output.
            //for(int i=0; i<numOfElements; i++)
            //{
            //    // Sum of element i-th.
            //    int sum = 0;
            //    foreach(string output in outArray)
            //    {
            //        int s = int.Parse(output.Split(' ')[i]);

            //        sum += s;
            //        sum = sum % sumBase; 
            //    }

            //    outputs[i].ForceState(sum, true, false);

        
            //}

        }

        
    }

}
