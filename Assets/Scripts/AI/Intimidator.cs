using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class Intimidator : MonoBehaviour, IFighter
    {
        [SerializeField]
        float attackRange = 10f;

        [SerializeField]
        GameOverType gameOverType = GameOverType.Capture;
      

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool Fight(Transform target)
        {
            // There is no fight at all.
            target.GetComponent<PlayerController>().Surrender((int)gameOverType);

            return true;
        }

        public float GetFightingRange()
        {
            return attackRange;
        }

        public bool AttackAvailable()
        {
            return true;
        }

        public bool CallFightByAnimation()
        {
            return false;
        }
    }

}
