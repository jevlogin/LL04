using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Main : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Мы: проснулись");
            Debug.Log("Мы: потянулись");
            var coroutine =  StartCoroutine(HeatsWaterInTheKettle());
            //StopCoroutine(coroutine);
            Debug.Log("Мы: пошли умываться");
        }

        private IEnumerator HeatsWaterInTheKettle()
        {
            Debug.Log("Мы: включили чайник");
            yield return new WaitForSeconds(3);
            Debug.Log("Чайник: согрелся");
        }
    } 
}
