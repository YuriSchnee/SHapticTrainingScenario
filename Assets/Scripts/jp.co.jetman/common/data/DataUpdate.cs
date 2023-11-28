

using UnityEngine;

namespace jp.co.jetman.common.data
{
    [System.Serializable]
    public class DataUpdate
    {
        public float time = 0.0f;
        public double durationTime = 0.0f;                      // added
        public Vector2 reticlePosition = default(Vector2);
        public float reticlePositionDegree = 0.0f;              // added
        public Vector2 cameraRotation = default(Vector2);       // added
        public Vector2 mousepos = default(Vector2);             // added
        public Vector2 speedVector = default(Vector2);          // added
        public float speed = 0.0f;                              // added
        public int targetSpawned = 0;
        public int targetLeaved = 0;
        public int shot = 0;
        public int targetDestroyed = 0;
        public Vector3 spawnPosition = default(Vector3);
        public string hap1 = "";
        public string hap2 = "";
        public float rayDistance = 0.0f;                        // added
        public int startingToMove = 0;                          // added
        public int onTarget = 0;                                // added
    }
}