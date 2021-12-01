using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Burst;


namespace WORLDGAMEDEVELOPMENT
{
    [BurstCompile]
    public struct RotateAroundTransformJob : IJobParallelForTransform
    {
        public NativeArray<float> Angle;
        public NativeArray<Quaternion> OriginRotation;


        public void Execute(int index, TransformAccess transform)
        {
            Angle[index]++;
            var rotationY = Quaternion.AngleAxis(Angle[index], Vector3.up);
            transform.rotation = OriginRotation[index] * rotationY;
        }
    }
}