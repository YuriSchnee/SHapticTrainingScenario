
using UnityEngine;

using jp.co.jetman.common;
using jp.co.jetman.common.spawners;
using jp.co.jetman.common.gameInstances;

namespace jp.co.jetman.sequentialTask.spawners
{
    public class SequentialSpawner : SpawnerAbstract
    {
        private SequentialSpawnerParameters parameters;

        private int arrayLength = 0;
        private int currentArrayIndex = 0;

        private TargetBehaviour prevTarget = null;

        #region Constructor
        public SequentialSpawner() {}
        #endregion

        #region Private Methods
        private Vector3 makeSequentialPosition()
        {
            var location = SpawnPositionLoader.positions[currentArrayIndex % arrayLength];

            var r = location.z;
            var y = location.y;
            var angle = location.x;
            var rad = (angle + OFFSET_Y_AXIS_DEGREE) * Mathf.Deg2Rad;
            var x = r * Mathf.Cos(rad);
            var z = r * Mathf.Sin(rad);

            currentArrayIndex++;
            currentArrayIndex = currentArrayIndex < arrayLength ? currentArrayIndex : 0;

            return new Vector3(x, y, z);
        }
        #endregion

        #region SpawnerAbstract
        override protected void makeTargets()
        {
            currentArrayIndex = 0;
            prevTarget = null;

            var n = SequentialTaskBehaviour.instance.MAX_TARGETS_AT_A_TIME;
            for (var i = 0; i < n; i++)
            {
                currentId = i + 1;
                makeTarget(currentId, makeSequentialPosition());
            }
        }
        override protected void makeTarget(int _id, Vector3 _position)
        {
            prevTarget?.TargetOn();
            if (prevTarget != null)
            {
                if (MainBehaviour.hapticsMode == HapticsMode.Feedback)
                    HapticsConnectorBehaviour.instance.OnTargetAvailable();
            }
            prevTarget = null;

            if (currentId <= ((NKillsGameParameters)AimAppSettings.instance.GetGameParams()).TOTAL_NUMBER_OF_ENEMIES_TO_KILL)
            {
                var g = GameObject.Instantiate(getPrefab(), Vector3.zero, Quaternion.identity);

                var comp = g.GetComponent<TargetBehaviour>();
                comp.id = _id;
                comp.parent = context?.gameObject.transform;
                comp.position = _position;
                comp.TargetOff();

                prevTarget = comp;
            }
        }
        override protected void makeTargetObjects()
        {
            throw new System.NotImplementedException();
        }
        override protected void makeTargetObject(int _id, Vector3 _position)
        {
            throw new System.NotImplementedException();
        }
        override protected void makeTargetHumanoids()
        {
            throw new System.NotImplementedException();
        }
        override protected void makeTargetHumanoid(int _id, Vector3 _position)
        {
            throw new System.NotImplementedException();
        }
        override protected void respawn(int _id, Vector3 _position)
        {
            Debug.Log("respawn");
            currentId++;

            makeTarget(currentId, makeSequentialPosition());
        }

        override public void Init(MonoBehaviour _context)
        {
            base.Init(_context);

            parameters = (SequentialSpawnerParameters) SequentialTaskBehaviour.instance.GetSpawnerParams();
            arrayLength = SpawnPositionLoader.positions.Count;
        }
        // override public void Reset()
        // {
            // base.Reset();
        // }
        // override public void Spawn()
        // {
            // base.Spawn();
        // }
        override public void Respawn(TargetBehaviour _other)
        {
            var target = _other.gameObject.transform;
            var position = target.position;
            GameObject.Destroy(_other.gameObject, 0.0f);

            context.StartCoroutine(delayMethod(parameters.DURATION_RESPAWN, () => {
                respawn(_other.id, position);
            }));
        }
        #endregion
    }
}