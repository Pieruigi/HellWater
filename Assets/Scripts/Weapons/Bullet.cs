using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class Bullet : MonoBehaviour
    {
        float speed = 60f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }

}
