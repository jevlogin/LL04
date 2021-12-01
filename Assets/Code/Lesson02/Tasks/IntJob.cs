using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;


namespace WORLDGAMEDEVELOPMENT
{
    [BurstCompile]
    public struct IntJob : IJob
    {
        public NativeArray<int> Data;

        public void Execute()
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i] > 10)
                {
                    Data[i] = 0;
                }
            }
        }
    }
}