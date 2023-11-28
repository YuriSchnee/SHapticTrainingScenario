using UnityEngine;

namespace jp.co.jetman.sequentialTask.spawners.data
{
    [System.Serializable]
    public class RandomRange
    {
        [SerializeField]
        public RandomRangeX X;
        [SerializeField]
        public RandomRangeY Y;
        [SerializeField]
        public RandomRangeZ Z;
    }
}