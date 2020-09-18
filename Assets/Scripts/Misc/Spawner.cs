using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class Spawner: MonoBehaviour
    {
        [SerializeField]
        Transform target;
        public Transform Target
        {
            get { return target; }
        }

        [SerializeField]
        List<Transform> spawnPoints;

        int spawnPointId = 0; // 0 is the player starting point id for a new game
        public int SpawnPointId
        {
            get { return spawnPointId; }
            set { spawnPointId = value; }
        }

        
        // Start is called before the first frame update
        void Start()
        {
            // Ask the level manager for the spawn point in the world
            Transform sp = spawnPoints[spawnPointId];
            Debug.Log("SpawnPoint:" + sp);

            // Set position and rotation
            target.position = sp.position;
            target.rotation = sp.rotation;
        }

        // Update is called once per frame
        void Update()
        {

        }

        
    }

}
