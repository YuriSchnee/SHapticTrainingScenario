using UnityEngine;

namespace jp.co.jetman.sequentialTask.spawners.data
{
    [System.Serializable]
    public class RandomLocation
    {
        [SerializeField, Range(-180.0f, 180.0f)]
        public float X;
        [SerializeField, Range(0.0f, 10.0f)]
        public float Y;
        [SerializeField, Range(1.0f, 20.0f)]
        public float Z = 1.0f;
    }
}