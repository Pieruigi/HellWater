using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    public class Health : MonoBehaviour
    {
        
        [SerializeField]
        float maxHealth;

        //[SerializeField]
        float currentHealth;
        public float CurrentHealth
        {
            get { return currentHealth; }
        }

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Damage(float amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }

        public void Init(float amount)
        {
            currentHealth = amount;
        }
    }

}
