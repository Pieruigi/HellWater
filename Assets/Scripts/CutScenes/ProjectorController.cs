using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;

namespace HW.Cinema
{
    public class ProjectorController : MonoBehaviour
    {
        //[SerializeField]
        GameObject playableObject;

        [SerializeField]
        List<Sprite> slides;

        int currentId = 0;

        private void Awake()
        {
            playableObject = transform.parent.gameObject;
            playableObject.GetComponent<CutSceneController>().OnStart += HandleOnStart;
            playableObject.GetComponent<CutSceneController>().OnStop += HandleOnStop;
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
        
        void HandleOnStart(CutSceneController playable)
        {
            currentId = 0;
        }

        void HandleOnStop(CutSceneController playable)
        {
            SlideProjector.Instance.Hide();
        }
    }

}
