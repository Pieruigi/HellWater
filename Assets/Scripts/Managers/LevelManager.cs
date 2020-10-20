using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HW
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        GroundType defaultGroundType = GroundType.Dirt;
        public GroundType DefaultGroundType
        {
            get { return defaultGroundType; }
        }

        [SerializeField]
        bool weaponNotAllowed = false; // Can you handle you weapons?

        public static LevelManager Instance { get; private set; }

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
            // Force holster if needed
            if (weaponNotAllowed)
                PlayerController.Instance.HolsterWeapon(true);
             
             
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int GetLevelId()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

       
    }

}
