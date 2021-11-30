using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Jobs;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Lesson02Task02 : MonoBehaviour, IDisposable
    {
        #region Fields

        [SerializeField] private int _legth;

        private List<IDisposable> _disposableList;
        private NativeArray<Vector3> _positions;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _finalPositions;

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
            Initialization();
            
            FinalPositionsJob finalPositionsJob = new FinalPositionsJob()
            {
                Positions = _positions,
                Velocities = _velocities,
                FinalPositions = _finalPositions
            };

            JobHandle handle = finalPositionsJob.Schedule(_legth, 0);
            handle.Complete();

            if (handle.IsCompleted)
            {
                foreach (var data in _finalPositions)
                {
                    Debug.Log(data);
                }
            }
        }

        #endregion


        #region Methods

        private void Initialization()
        {
            _disposableList = new List<IDisposable>();
            Vector3[] vector3Positions = new Vector3[_legth].ReturnNewRandomVector3MinMaxByte();
            _positions = new NativeArray<Vector3>(vector3Positions, Allocator.Persistent);
            _disposableList.Add(_positions);

            

            Vector3[] vector3Velocities = new Vector3[_legth].ReturnNewRandomVector3MinMaxByte();
            _velocities = new NativeArray<Vector3>(vector3Velocities, Allocator.Persistent);
            _disposableList.Add(_velocities);

            Vector3[] vector3FinalPositions = new Vector3[_legth];
            _finalPositions = new NativeArray<Vector3>(vector3FinalPositions, Allocator.Persistent);
            _disposableList.Add(_finalPositions);
        } 

        #endregion


        #region IDisposable

        public void Dispose()
        {
            foreach (var disposable in _disposableList)
            {
                disposable.Dispose();
            }
            _disposableList.Clear();
        } 

        #endregion
    }
}
