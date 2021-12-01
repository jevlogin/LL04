using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;


namespace WORLDGAMEDEVELOPMENT
{
    [BurstCompile]
    public struct FinalPositionsJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> Positions;
        [ReadOnly] public NativeArray<Vector3> Velocities;
        [WriteOnly] public NativeArray<Vector3> FinalPositions;

        public void Execute(int index)
        {
            //FinalPositions[index].Sum(Positions[index], Velocities[index]);   // почему не работает метод расширения?!?!
            FinalPositions[index] = Sum(Positions[index], Velocities[index]);
        }

        public Vector3 Sum( Vector3 vectorFirst, Vector3 vectorSecond)
        {
            return new Vector3(vectorFirst.x + vectorSecond.x, vectorFirst.y + vectorSecond.y, vectorFirst.z + vectorSecond.z);
        }
    }
}