
using UnityEngine;

namespace jp.co.jetman.common.data
{
    [System.Serializable]
    public class DataResult
    {
        public string targetName = "";
        public float time = 0.0f;
        public float timeReaction = 0.0f;
        public float timeLife = 0.0f;
        public float distance = 0.0f;
        public Vector3 position = default(Vector3);
        public float startingToMove = 0.0f;             // added
    }
}