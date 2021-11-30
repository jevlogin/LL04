using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public struct OtherJob : IJobParallelFor
    {
        [WriteOnly]
        public NativeArray<Vector3> Positions;
        [ReadOnly]
        public NativeArray<Vector3> NewPositions;

        public void Execute(int index)
        {
            Positions[index] = NewPositions[index];
        }
    }
}