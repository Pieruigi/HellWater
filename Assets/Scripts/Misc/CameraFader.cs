﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace HW
{
    public class CameraFader : MonoBehaviour
    {
        [SerializeField]
        Image image;

        float speed = 2f;

        //float time = 0.5f;

        static CameraFader instance;
        public static CameraFader Instance
        {
            get { return instance; }
        }

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ForceBlackScreen()
        {
            Color c = Color.black;
            c.a = 1;
            image.color = c;
        }

        public void FadeIn(UnityAction callback = null)
        {
            StartCoroutine(FadeInCoroutine(speed, callback));
        }

        public void FadeIn(float speed, UnityAction callback = null)
        {
            StartCoroutine(FadeInCoroutine(speed, callback));
        }

        public void FadeOut(UnityAction callback = null)
        {
            StartCoroutine(FadeOutCoroutine(speed, callback));
        }

        public void FadeOutIn(float length, UnityAction callback)
        {
            StartCoroutine(FadeOutInCoroutine(speed, length, callback));
        }

        public void FadeOutIn(float speed, float length, UnityAction callback)
        {
            StartCoroutine(FadeOutInCoroutine(speed, length, callback));
        }

        public IEnumerator FadeInCoroutine(float speed, UnityAction callback = null)
        {
            Color c = Color.black;
            c.a = 0;
            float time = 1f / speed;
            LeanTween.color((RectTransform)image.transform, c, time);

            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        public IEnumerator FadeInCoroutine(UnityAction callback = null)
        {
            yield return FadeInCoroutine(speed, callback);
        }

        public IEnumerator FadeOutCoroutine(float speed, UnityAction callback = null)
        {
            Color c = Color.black;
            c.a = 1;
            float time = 1f / speed;
            LeanTween.color((RectTransform)image.transform, c, time);

            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        public IEnumerator FadeOutCoroutine(UnityAction callback = null)
        {
            yield return FadeOutCoroutine(speed, callback);
        }

        public IEnumerator FadeOutInCoroutine(float speed, float length, UnityAction callback)
        {
            yield return FadeOutCoroutine(speed);

            yield return new WaitForSeconds(length);

            yield return FadeInCoroutine(speed, callback);
        }

    }
}

