using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;
using Random = UnityEngine.Random;
using Unity.Jobs;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Galaxy : MonoBehaviour, IDisposable
    {
        #region Fields

        [SerializeField] private GameObject _celestialBodyPrefab;
        [SerializeField] private int _numberOfEntities;
        [SerializeField] private float _startDistance;
        [SerializeField] private float _startVelocity;
        [SerializeField] private float _startMass;
        [SerializeField] private float _gravitionModifier;

        private TransformAccessArray _transformAccessArray;
        private NativeArray<Vector3> _positions;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _accelerations;
        private NativeArray<float> _masses;

        private List<IDisposable> _disposables = new List<IDisposable>();

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
            InitializeJobs();

            Transform[] transforms = new Transform[_numberOfEntities];
            for (int i = 0; i < _numberOfEntities; i++)
            {
                _positions[i] = Random.insideUnitSphere * Random.Range(0, _startDistance);
                _velocities[i] = Random.insideUnitSphere * Random.Range(0, _startVelocity);
                _accelerations[i] = Vector3.zero;
                _masses[i] = Random.Range(1, _startMass);
                transforms[i] = Instantiate(_celestialBodyPrefab, _positions[i], Quaternion.identity).transform;
                transforms[i].localScale *= _masses[i];
            }

            _transformAccessArray = new TransformAccessArray(transforms);
            _disposables.Add(_transformAccessArray);
        }

        private void Update()
        {
            GravitationJob gravitationJob = new GravitationJob()
            {
                Positions = _positions,
                Velocities = _velocities,
                Accelerations = _accelerations,
                Masses = _masses,
                GravitionModifier = _gravitionModifier,
                DeltaTime = Time.deltaTime
            };
            JobHandle gravitationHandle = gravitationJob.Schedule(_numberOfEntities, 0);

            MoveJob moveJob = new MoveJob()
            {
                Positions = _positions,
                Velocities = _velocities,
                Accelerations = _accelerations,
                DeltaTime = Time.deltaTime
            };
            JobHandle moveHandle = moveJob.Schedule(_transformAccessArray, gravitationHandle);

            moveHandle.Complete();
        }

        #endregion


        #region Methods

        private void InitializeJobs()
        {
            _positions = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
            _disposables.Add(_positions);
            _velocities = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
            _disposables.Add(_velocities);
            _accelerations = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
            _disposables.Add(_accelerations);
            _masses = new NativeArray<float>(_numberOfEntities, Allocator.Persistent);
            _disposables.Add(_masses);
        }

        #endregion


        #region IDisposable

        public void Dispose()
        {
            foreach (var dispos in _disposables)
            {
                dispos.Dispose();
            }
        }

        #endregion
    }
}
