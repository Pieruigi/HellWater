using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public enum HumanoidNodeName { None, RightHand, LeftHand, Head }

    public class HumanoidNodeCollection : MonoBehaviour
    {
        [SerializeField]
        Transform rightHandNode;

        [SerializeField]
        Transform leftHandNode;

        [SerializeField]
        Transform headNode;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Transform GetNode(HumanoidNodeName nodeName)
        {
            Transform ret = null;

            switch (nodeName)
            {
                case HumanoidNodeName.RightHand:
                    ret = rightHandNode;
                    break;
                case HumanoidNodeName.LeftHand:
                    ret = leftHandNode;
                    break;
                case HumanoidNodeName.Head:
                    ret = headNode;
                    break;
            }

            return ret;
        }
    }

}

