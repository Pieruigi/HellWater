using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HW.UI
{
    public class HealthBar : MonoBehaviour
    {

        [SerializeField]
        Image bar;

        Health health;

        float currentHealth;
        float desiredHealth;

        float fillTime = 1f;
        float fillSpeed;

        float dangerValue = 0.3f;
        float dangerSpeed = 4f;
        int dangerDir = 0;
        Color desiredColor;


        // Start is called before the first frame update
        void Start()
        {
            health = PlayerController.Instance.GetComponent<Health>();
            health.OnHealthChange += HandleOnHealthChange;

            // Set fill speed
            fillSpeed = health.MaxHealth / fillTime;

            // Set health
            desiredHealth = health.CurrentHealth;
            currentHealth = 0;
            FillBar();
        }

        // Update is called once per frame
        void Update()
        {
            // We don't want the bar to teleport between values, instead we want it to fill until the player health
            // is reached.
            FillBar();

            // Check color
            Colorize();
        }

        void HandleOnHealthChange()
        {
            desiredHealth = health.CurrentHealth;
        }

        void FillBar()
        {
            if (currentHealth != desiredHealth)
            {
                currentHealth = Mathf.MoveTowards(currentHealth, desiredHealth, fillSpeed * Time.deltaTime);
                bar.fillAmount = currentHealth / health.MaxHealth;

            }
            
        }

        void Colorize()
        {
            if(currentHealth > health.MaxHealth * dangerValue)
            {
                dangerDir = 0;
                desiredColor = Color.white;
                bar.color = Vector4.MoveTowards(bar.color, desiredColor, dangerSpeed * Time.deltaTime);
            }
            else
            {
                // Init direction and choose color
                if (dangerDir == 0)
                {
                    dangerDir = 1;
                    desiredColor = Color.red;
                }

                // Update bar color
                bar.color = Vector4.MoveTowards(bar.color, desiredColor, dangerSpeed * Time.deltaTime);
                
                // Switch color when needed
                if(bar.color == desiredColor)
                {
                    if (dangerDir > 0)
                        desiredColor = Color.red;
                    else
                        desiredColor = Color.white;

                    dangerDir *= -1;
                }
            }
        }
    }

}
