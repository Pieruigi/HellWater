using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using HW.Interfaces;

namespace HW.UI
{
    public class AutomaticNavigator : MonoBehaviour
    {
        [SerializeField]
        bool horizontal;

        [SerializeField]
        bool vertical;

        IActivable activable;

        List<Selectable> selectables;

        DateTime lastSelection;
        float selectionTime = 0.25f;

        GameObject lastSelectedObject;

        private void Awake()
        {
            activable = GetComponent<IActivable>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get all selectables
            selectables = new List<Selectable>(GetComponentsInChildren<Selectable>());
            Debug.Log("Selectables.Count:" + selectables.Count);
        }

        private void OnEnable()
        {
            selectables = new List<Selectable>(GetComponentsInChildren<Selectable>());
        }

        // Update is called once per frame
        void Update()
        {
            if (!activable.IsActive())
                return;

            CheckNavigation();

            // The event system loses focus on the selected object when you click the mouse 
            // button somewhere around in the screen. To avoid this we simply store the current 
            // selected object on each frame so that we will be able to replace it when 
            // focus gets lost.
            if (!EventSystem.current.currentSelectedGameObject)
                lastSelectedObject.GetComponent<Selectable>().Select();
            else
                lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        void CheckNavigation()
        {
            if ((DateTime.UtcNow - lastSelection).TotalSeconds < selectionTime)
                return;

            float inputH = PlayerInput.GetHorizontalAxisRaw();
            float inputV = PlayerInput.GetVerticalAxisRaw();
            if (inputH == 0 && inputV == 0)
                return;

            // Get selected button
            Debug.Log("EventSystem.Current:" + EventSystem.current.name);
            Selectable selected = selectables.Find(b => b.gameObject == EventSystem.current.currentSelectedGameObject);
            Debug.Log("Selected:" + selected);

            Vector3 v = Vector3.zero;
            if (horizontal)
                v += Vector3.right * inputH;

            if (vertical)
                v += Vector3.up * inputV;

            Selectable next = selected.FindSelectable(v);
            Debug.Log("Next:" + next);

            // The object we want to select must be contained in the selectable list because UI navigation also
            // takes into accout selectables beholding to other panels
            if (next && selectables.Contains(next))
                next.Select();
                

            lastSelection = DateTime.UtcNow;
        }
        
    }

}
