using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


namespace WORLDGAMEDEVELOPMENT
{
    public struct SomeJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> Positions;
        [WriteOnly]
        public NativeArray<Vector3> NewPositions;

        public void Execute(int index)
        {
            NewPositions[index] = Positions[0];
        }
    }
}