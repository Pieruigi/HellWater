using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Interfaces
{
    public interface ISkippable
    {
        void Skip();

        bool CanBeSkipped();
    }

}
