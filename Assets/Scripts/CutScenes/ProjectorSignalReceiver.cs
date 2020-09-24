using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;

namespace HW.CutScene
{
    public class ProjectorSignalReceiver : MonoBehaviour
    {
        //[SerializeField]
        GameObject playableObject;

        [SerializeField]
        List<Sprite> slides;

        int currentId = 0;

        private void Awake()
        {
        
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
               

        public void NextSlide()
        {
            SlideProjector.Instance.ShowSlide(slides[currentId]);
            currentId++;
        }
        
        public void CloseProjector()
        {
            SlideProjector.Instance.Hide();
        }

    }

}
