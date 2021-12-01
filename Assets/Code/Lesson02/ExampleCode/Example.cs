using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Example : MonoBehaviour
    {
        #region Fields

        [SerializeField] private SpriteRenderer _prefab;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private float _duration = 1.0f;
        [SerializeField] private int _numberOfObjects;

        private SpriteRenderer[] _spriteRenderers;
        private NativeArray<Color> _colorsA;
        private NativeArray<Color> _colorsB;
        private RepaintingJob _repaintngJob;
        private JobHandle _jobHandle;
        private NativeArray<Color> _outputColors;

        private float _currentTime = 0.0f;

        #endregion


        #region ClassLifeCycle

        private void OnDestroy()
        {
            _colorsA.Dispose();
            _colorsB.Dispose();
            _outputColors.Dispose();
        }

        #endregion


        #region UnityMethods

        private void Start()
        {
            _spriteRenderers = new SpriteRenderer[_numberOfObjects];
            _colorsA = new NativeArray<Color>(_numberOfObjects, Allocator.Persistent);
            _colorsB = new NativeArray<Color>(_numberOfObjects, Allocator.Persistent);
            _outputColors = new NativeArray<Color>(_numberOfObjects, Allocator.Persistent);
            _repaintngJob = new RepaintingJob();

            for (int i = 0; i < _numberOfObjects; i++)
            {
                var instance = Instantiate(_prefab);
                _spriteRenderers[i] = instance;
                instance.transform.position = Random.onUnitSphere * _spawnRadius;
                _colorsA[i] = Random.ColorHSV();
                _colorsB[i] = Random.ColorHSV();
            }
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _duration)
            { 
                _currentTime = 0.0f;
            }

            _repaintngJob = new RepaintingJob()
            {
                ColorsA = _colorsA,
                ColorsB = _colorsB,
                TimeDuration = _currentTime / _duration,
                OutputColors = _outputColors
            };
            _jobHandle = _repaintngJob.Schedule(_numberOfObjects, 0);

            _jobHandle.Complete();
            for (int i = 0; i < _numberOfObjects; i++)
            {
                _spriteRenderers[i].color = _outputColors[i];
            }
        }

        #endregion
    }
}