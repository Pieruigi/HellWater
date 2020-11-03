using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class RendererActivatorController : MonoBehaviour
    {
       
        [SerializeField]
        List<RendererActivator> objects;

        // Object to activate inside
        [SerializeField]
        List<GameObject> activables;

        // Object to deactivade inside
        [SerializeField]
        List<GameObject> deactivables;

        bool fadeEnabled = false;


        private void Awake()
        {
              Deactivate();  
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(!fadeEnabled)
                fadeEnabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != PlayerController.Instance.gameObject)
                return;

            Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != PlayerController.Instance.gameObject)
                return;

            Deactivate();
        }

        void Activate()
        {
            StartCoroutine(CoroutineActivate(true));
        }

        void Deactivate()
        {
            StartCoroutine(CoroutineActivate(false));
        }

        IEnumerator CoroutineActivate(bool value)
        {
            bool fade = fadeEnabled;
            if (fade)
            {
                CameraFader.Instance.TryDisableAnimator();
                yield return CameraFader.Instance.FadeOutCoroutine();
            }
                

            foreach (RendererActivator o in objects)
            {
                o.Activate(value);
            }

            foreach(GameObject o in activables)
            {
                o.GetComponent<IActivable>().Activate(value);
            }

            foreach (GameObject o in deactivables)
            {
                o.GetComponent<IActivable>().Activate(!value);
            }

            if (fade)
            {
                CameraFader.Instance.TryEnableAnimator();
                yield return CameraFader.Instance.FadeInCoroutine();
            }
                
        }

        //IEnumerator CoroutineDeactivate()
        //{
        //    bool fade = fadeEnabled;

        //    if (fade)
        //    {
        //        CameraFader.Instance.TryDisableAnimator(); 
        //        yield return CameraFader.Instance.FadeOutCoroutine();
        //    }
                
        //    foreach (RendererActivator o in objects)
        //    {
        //        o.Activate(false);
        //    }

        //    if (fade)
        //    {
        //        CameraFader.Instance.TryEnableAnimator();
        //        yield return CameraFader.Instance.FadeInCoroutine();
        //    }
                
        //}

    }

}
