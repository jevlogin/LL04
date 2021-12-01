using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


namespace WORLDGAMEDEVELOPMENT
{
    public struct AdvancedJob : IJobParallelFor
    {
        public NativeArray<Vector3> ArrayVector3;

        public void Execute(int index)
        {
            ArrayVector3[index] = ArrayVector3[index].normalized;
        }
    }
}