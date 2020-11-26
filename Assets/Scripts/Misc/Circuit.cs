using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    /// <summary>
    /// Get some input and return some output.
    /// </summary>
    public class Circuit : MonoBehaviour
    {
        [SerializeField]
        List<string> inputs;

        [SerializeField]
        List<string> outputs;

        /// <summary>
        /// Look for the input string in the input list and return the corresponding output; if
        /// no input is found return null.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetOutput(string input)
        {
            // Empty input.
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                return null;

            // Look for output.
            int id = inputs.FindIndex(i => i.Equals(input));

            // No output ( wrong input ).
            if (id < 0)
                return null;

            return outputs[id];
        }
    }

}
