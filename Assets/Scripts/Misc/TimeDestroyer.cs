using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class TimeDestroyer : MonoBehaviour
    {
        [SerializeField]
        float time = 1;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator CoroutineDestroy()
        {
            yield return new WaitForSeconds(time);

            Destroy(gameObject);
        }
    }

}
