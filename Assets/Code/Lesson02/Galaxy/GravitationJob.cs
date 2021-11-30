using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;


namespace WORLDGAMEDEVELOPMENT
{
    [BurstCompile]
    public struct GravitationJob : IJobParallelFor
    {
        public NativeArray<Vector3> Accelerations;
        [ReadOnly]
        public NativeArray<Vector3> Positions;
        [ReadOnly]
        public NativeArray<Vector3> Velocities;
        [ReadOnly]
        public NativeArray<float> Masses;
        [ReadOnly]
        public float GravitionModifier;
        [ReadOnly]
        public float DeltaTime;

        public void Execute(int index)
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                if (i == index)
                {
                    continue;
                }
                float distance = Vector3.Distance(Positions[i], Positions[index]);
                if (distance > 2.0f)
                {
                    break;
                }
                var direction = Positions[i] - Positions[index];
                Vector3 gravitation = (direction * Masses[i] * GravitionModifier) / (Masses[index] * Mathf.Pow(distance, 2));
                Accelerations[index] += gravitation * DeltaTime;
            }
        }
    }
}