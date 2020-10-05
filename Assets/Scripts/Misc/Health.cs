using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    public class Health : MonoBehaviour
    {
        public UnityAction OnHealthChange; // Both for damage and heal

        [SerializeField]
        float maxHealth;
        public float MaxHealth
        {
            get { return maxHealth; }
        }

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

            OnHealthChange?.Invoke();
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

            OnHealthChange?.Invoke();
        }

        public void Init(float amount)
        {
            currentHealth = amount;
        }
    }

}
