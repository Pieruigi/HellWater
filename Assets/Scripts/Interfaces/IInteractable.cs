using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Interfaces
{
    public interface IInteractable
    {
        void Interact();

        bool IsAvailable();
    }

}
