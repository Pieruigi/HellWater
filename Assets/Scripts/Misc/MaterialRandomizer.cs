using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class MaterialRandomizer : MonoBehaviour
    {
        [SerializeField]
        List<Material> materials;

        Renderer rend;

        private void Awake()
        {
            rend = GetComponent<Renderer>();

            int r = Random.Range(0, materials.Count);
            
            rend.material = materials[r];
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
