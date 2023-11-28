using UnityEngine;

namespace jp.co.jetman.sequentialTask.spawners.data
{
    [System.Serializable]
    public class RandomRangeX
    {
        [SerializeField, Range(-180.0f, 180.0f)]
        public float FROM = -40.0f;
        [SerializeField, Range(-180.0f, 180.0f)]
        public float TO = 40.0f;
    }
}