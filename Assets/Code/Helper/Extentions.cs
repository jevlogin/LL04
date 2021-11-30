using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public static class Extentions
    {
        public static Vector3 Sum(this Vector3 vector3, Vector3 vectorFirst, Vector3 vectorSecond)
        {
            vector3 = new Vector3(vectorFirst.x + vectorSecond.x, vectorFirst.y + vectorSecond.y, vectorFirst.z + vectorSecond.z);
            return vector3;
        }

        public static Vector3[] ReturnNewRandomVector3MinMaxByte(this Vector3[] vector3s)
        {
            for (int i = 0; i < vector3s.Length; i++)
            {
                vector3s[i] = Random.insideUnitSphere * Random.Range(byte.MinValue, byte.MaxValue);
            }
            return vector3s;
        }
    }
}
