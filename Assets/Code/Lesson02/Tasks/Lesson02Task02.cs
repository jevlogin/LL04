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
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _angleRotation;
        [SerializeField] private float _speedRotation;

        private List<IDisposable> _disposableList;
        private NativeArray<Vector3> _positions;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _finalPositions;
        private TransformAccessArray _transformAccessArray;

        /*****/
        private NativeArray<float> _angle;
        private NativeArray<Quaternion> _originRotation;
        private Transform[] _transforms;

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

            _transforms = new Transform[_legth];

            for (int i = 0; i < _legth; i++)
            {
                _transforms[i] = Instantiate(_prefab).transform;
            }

            _transformAccessArray = new TransformAccessArray(_transforms);
            _disposableList.Add(_transformAccessArray);

            var arrayAngle = new float[_legth];
            for (int i = 0; i < _legth; i++)
            {
                arrayAngle[i] = i;
            }
            _angle = new NativeArray<float>(arrayAngle, Allocator.Persistent);
            _disposableList.Add(_angle);

            var rotations = new Quaternion[_transforms.Length];
            for (int i = 0; i < rotations.Length; i++)
            {
                rotations[i] = _transforms[i].rotation;
            }

            _originRotation = new NativeArray<Quaternion>(rotations, Allocator.Persistent);
            _disposableList.Add(_originRotation);
        }


        private void Update()
        {
            RotateAroundTransformJob rotateAround = new RotateAroundTransformJob()
            {
                Angle = _angle,
                OriginRotation = _originRotation,
            };
            var handleRotation = rotateAround.Schedule(_transformAccessArray);
            handleRotation.Complete();
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
