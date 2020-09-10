using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class EnemyPS : MonoBehaviour
    {
        [SerializeField]
        List<ParticleSystem> splatters;

        Enemy enemy;

        CapsuleCollider coll;
        float maxOffset;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            enemy.OnGotHit += HandleOnGotHit;
            coll = GetComponent<CapsuleCollider>();
            maxOffset = coll.height * 0.5f;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnGotHit(HitInfo hitInfo)
        {
            if (splatters.Count > 0)
            {
                ParticleSystem ps = Instantiate(splatters[Random.Range(0, splatters.Count)]);

                ps.transform.forward = hitInfo.Normal;
                Debug.Log("HitNormal:" + hitInfo.Normal);
                ps.transform.position = hitInfo.Position + Vector3.up * Random.Range(0f,maxOffset);
            }
            
        }
    }

}
