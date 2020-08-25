using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class MeleeChargingEffect : MonoBehaviour
    {
        Material[] materials;
        string emissionColorParam = "_EmissionColor";

        int dir = 0;
        float speed = 30;

        private void Awake()
        {
            materials = GetComponent<MeshRenderer>().materials;
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
            playerController.OnAttackCharged += HandleOnAttackCharged;

        }

        // Update is called once per frame
        void Update()
        {
            if (dir == 0)
                return;


            Color newColor;
            Vector4 desiredColor;
            if (dir > 0)
            {
                Vector4 currentColor = materials[0].GetColor(emissionColorParam);
                desiredColor = Color.red;
                newColor = Vector4.MoveTowards(currentColor, desiredColor, speed * Time.deltaTime);
                
            }
            else
            {
                Vector4 currentColor = materials[0].GetColor(emissionColorParam);
                desiredColor = Color.black;
                newColor = Vector4.MoveTowards(currentColor, desiredColor, speed * Time.deltaTime);
            }

            foreach (Material material in materials)
                material.SetColor(emissionColorParam, newColor);

            if (newColor == (Color)desiredColor)
                dir = 0;
        }

        void HandleOnAttackCharged(bool value)
        {
            foreach (Material material in materials)
                material.SetColor(emissionColorParam, value ? Color.red : Color.black);

        }
    }

}
