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
            Debug.Log("��: ����������");
            Debug.Log("��: ����������");
            var coroutine =  StartCoroutine(HeatsWaterInTheKettle());
            //StopCoroutine(coroutine);
            Debug.Log("��: ����� ���������");
        }

        private IEnumerator HeatsWaterInTheKettle()
        {
            Debug.Log("��: �������� ������");
            yield return new WaitForSeconds(3);
            Debug.Log("������: ��������");
        }
    } 
}
