using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;
using Random = UnityEngine.Random;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Lesson02Task01 : MonoBehaviour, IDisposable
    {
        #region Fields
        
        private NativeArray<int> _dataArray;
        private List<IDisposable> _disposableList;
        [SerializeField] private int _lengthArray = 100;

        #endregion


        #region ClassLifeCycles

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion


        #region UnityMethods

        private void Start()
        {
            Initialize();
            IntJob intJob = new IntJob()
            {
                Data = _dataArray
            };
            JobHandle handle = intJob.Schedule();
            handle.Complete();
            foreach (var data in _dataArray)
            {
                Debug.Log(data);
            }
            if (handle.IsCompleted)
            {
                _disposableList.Remove(_dataArray);
                _dataArray.Dispose();
            }
        } 

        #endregion


        #region Methods

        private void Initialize()
        {
            _disposableList = new List<IDisposable>();
            var array = new int[_lengthArray];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Random.Range(1, _lengthArray);
            }
            _dataArray = new NativeArray<int>(array, Allocator.TempJob);
            _disposableList.Add(_dataArray);
        }

        #endregion


        #region IDisposable
        
        public void Dispose()
        {
            foreach (var dispose in _disposableList)
            {
                dispose?.Dispose();
            }
        } 
        
        #endregion
    } 
}
