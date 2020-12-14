using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class Referer : MonoBehaviour
    {
        object reference;

        public object GetReference()
        {
            return reference;
        }

        public void SetReference(object reference)
        {
            this.reference = reference;
        }
    }

}
