using UnityEngine;

namespace jp.co.jetman.sequentialTask.spawners
{
    [System.Serializable]
    public class SequentialSpawnerParameters
    {
        [SerializeField, Range(0.0f, 5.0f)]
        public float DURATION_RESPAWN = 0.0f;
    }
}