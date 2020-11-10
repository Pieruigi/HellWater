using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class EnemyGroup : MonoBehaviour
    {
        enum State { Disabled = -1, Enabled = 0 }

        [SerializeField]
        List<GameObject> enemies;

        // The finite state machine attached to this object.
        FiniteStateMachine fsm;

        // Contains info about who is alive and who is dead.
        List<bool> deadList;
        public IList<bool> DeadList
        {
            get { return deadList.AsReadOnly(); }
        }

        private void Awake()
        {
            // We check if the dead list is empty before initializing it in order to avoid to 
            // override data loaded from cache.
            if(deadList == null)
            {
                deadList = new List<bool>();
                for (int i = 0; i < enemies.Count; i++)
                    deadList.Add(false);
            }

            // Get the finite state machine.
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            // Set the dead handle to each enemy.
            foreach (GameObject enemy in enemies)
                enemy.GetComponent<Enemy>().OnDead += HandleOnDead;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Initialization.
            // If the fsm state is disabled then disable all the enemies.
            if(fsm.CurrentStateId == (int)State.Disabled)
            {
                foreach (GameObject o in enemies)
                    o.SetActive(false);
            }
            else
            {

                // Fsm state is enabled, so we activate only enemies who are still alive.
                EnableEnemies();
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// The alive list contains a flag for each enemy in this group.
        /// True means the enemy is alive, false means is dead and so it will be destroyed.
        /// </summary>
        /// <param name="deadList">True it the corresponding enemy is dead, otherwise false.</param>
        public void Init(List<bool> deadList)
        {
            this.deadList = deadList;

            // The dead list empty means all the enemies are still alive.
            if(deadList.Count == 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                    deadList.Add(false);
            }
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            // If the attached finite state machine switch to the enable state then
            // enemies must be activated.
            if(fsm.CurrentStateId == (int)State.Enabled)
            {
                EnableEnemies();
            }
        }

        void EnableEnemies()
        {
            bool all = (deadList.Count == 0);
            for (int i = 0; i < enemies.Count; i++)
            {
                if (all || !deadList[i])
                    enemies[i].SetActive(true);
                else
                    enemies[i].SetActive(false);
            }
        }

        void HandleOnDead(Enemy enemy)
        {
            // Get the enemy index from the list.
            int id = enemies.IndexOf(enemy.gameObject);

            // Flag as dead.
            deadList[id] = true;
        }
    }

}
