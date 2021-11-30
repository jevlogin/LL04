using System.Collections;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class OtherObject : MonoBehaviour
    {
        private NativeArray<Vector3> _array;
        private JobHandle _jobHandle;

        private void Start()
        {
            _array = new NativeArray<Vector3>(100, Allocator.TempJob);
            AdvancedJob job = new AdvancedJob();
            job.ArrayVector3 = _array;

            _jobHandle = job.Schedule(100, 5);
            _jobHandle.Complete();

            StartCoroutine(JobCoroutine());
        }

        private IEnumerator JobCoroutine()
        {
            while (_jobHandle.IsCompleted == false)
            {
                yield return new WaitForEndOfFrame();
            }
            foreach (var vector in _array)
            {
                print(vector);
            }
            _array.Dispose();
        }

    } 
}
