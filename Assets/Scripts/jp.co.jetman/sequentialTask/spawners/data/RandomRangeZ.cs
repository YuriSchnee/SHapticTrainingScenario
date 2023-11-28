using UnityEngine;

namespace jp.co.jetman.sequentialTask.spawners.data
{
    [System.Serializable]
    public class RandomRangeZ
    {
        [SerializeField, Range(1.0f, 20.0f)]
        public float FROM = 5.0f;
        [SerializeField, Range(1.0f, 20.0f)]
        public float TO = 15.0f;
    }
}