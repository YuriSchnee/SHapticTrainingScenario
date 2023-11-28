using UnityEngine;

namespace jp.co.jetman.sequentialTask.spawners.data
{
    [System.Serializable]
    public class RandomRangeY
    {
        [SerializeField, Range(0.0f, 10.0f)]
        public float FROM = 0.0f;
        [SerializeField, Range(0.0f, 10.0f)]
        public float TO = 2.0f;
    }
}