using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [BurstCompile(CompileSynchronously = true)]
    public struct RepaintingJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Color> ColorsA;
        [ReadOnly]
        public NativeArray<Color> ColorsB;
        [WriteOnly]
        public NativeArray<Color> OutputColors;

        public float TimeDuration;

        public void Execute(int index)
        {
            OutputColors[index] = Color.Lerp(ColorsA[index], ColorsB[index], TimeDuration);
        }
    }
}