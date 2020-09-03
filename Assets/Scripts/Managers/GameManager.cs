using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class GameManager : MonoBehaviour
    {
        private Language language = Language.Italian;
        public Language Language
        {
            get { return language; }
        }

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
