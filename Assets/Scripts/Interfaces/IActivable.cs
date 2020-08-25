using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Interfaces
{
    public interface IActivable
    {
        void Activate(bool value);
        bool IsActive();
    }

}
