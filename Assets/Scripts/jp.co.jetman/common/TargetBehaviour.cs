using System.Collections.Generic;

using UnityEngine;

using jp.co.jetman.common.data;
using jp.co.jetman.sequentialTask;

namespace jp.co.jetman.common
{
    public class TargetBehaviour : MonoBehaviour
    {
        [Header("[Parameters]")]
        [SerializeField]
        protected float SCALE = 1.0f;
        [SerializeField]
        protected Vector3 OFFSET_TARGET_CENTER = Vector3.zero;
        [SerializeField]
        protected int HIT_COUNT_NEEDED_TO_BREAK = 1;

        [Space(20.0f)]
        protected float OFFSET_TO_BOTTOM = 0.5f;

        protected int _id = 1;
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        protected float timeAtSpawned = 0.0f;
        protected float timeAtBeginAiming = 0.0f;
        protected float timeAtDestroyed = 0.0f;
        protected int hitCount = 0;
        protected bool destroyFlag = false;
        protected Vector3 _hitPoint = default(Vector3);
        public Vector3 hitPoint
        {
            get
            {
                return _hitPoint;
            }
        }

        private float initDurability = 0.0f;
        private float _durability = 0.0f;
        public float durability
        {
            set
            {
                _durability = value;
            }
        }

        virtual public Vector3 position
        {
            set
            { 
                var r = Vector2.Distance(Vector2.zero, new Vector2(value.x, value.z));
                value.y = r < StageBehaviour.instance.INNER_RADIUS ? value.y : (value.y < StageBehaviour.instance.STEP_HEIGHT + OFFSET_TO_BOTTOM ? StageBehaviour.instance.STEP_HEIGHT + OFFSET_TO_BOTTOM : value.y);
                gameObject.transform.position = value;
            }
        }
        public Transform parent
        {
            set
            {
                gameObject.transform.parent = value;
            }
        }

        protected Transform playerTransform = null;

        protected int initLayer = 0;

        protected bool hasBeenFocused = false;

        [SerializeField]
        private Material[] _materials;
        [SerializeField]
        private MeshRenderer _ren;

        #region MonoBehaviour
        virtual protected void Awake()
        {
            initLayer = gameObject.layer;
        }
        virtual protected void Start()
        {
            gameObject.transform.localScale = Vector3.one * SCALE;
            timeAtSpawned = Time.realtimeSinceStartup;
        }
        virtual protected void Update()
        {
            if (playerTransform == null)
            {
                playerTransform = MainBehaviour.instance.GetPlayer().GetCameraTransform();
            }
            else
            {
                gameObject.transform.LookAt(playerTransform);
            }
        }
        #endregion

        #region Private Methods
        protected void record(Vector3 _hitPosition, Vector3 _playerPosition)
        {
            var timeReaction = Time.realtimeSinceStartup - timeAtBeginAiming;
            // Debug.Log($"reaction {timeReaction}");
            ScorerBehaviour.instance.RecordReactionSpeed(timeReaction);
            var timeLife = Time.realtimeSinceStartup - timeAtSpawned;

            var targetPosition = gameObject.transform.TransformPoint(OFFSET_TARGET_CENTER);
            var p1 = _playerPosition;
            var distance = Vector3.Distance(Vector3.LerpUnclamped(p1, targetPosition, Vector3.Magnitude(_hitPosition - p1) / Vector3.Magnitude(targetPosition - p1)), _hitPosition);
            ScorerBehaviour.instance.RecordDistance(distance);

            saveDataResults(timeReaction, timeLife, distance, targetPosition);
        }
        protected void destroy()
        {
            destroyFlag = true;
            SequentialTaskBehaviour.instance.currentTargetableTarget = null;
        }

        private float startingToMove = 0.0f;
        protected void saveDataResults(float _reaction, float _life, float _distance, Vector3 _position)
        {
            CSVSaver.DataResult_Record(
                new DataResult() {
                    targetName = $"{id}",
                    time = TimerBehaviour.instance.currentTime,
                    timeReaction = _reaction,
                    timeLife = _life,
                    distance = _distance,
                    position = _position,
                    startingToMove = startingToMove
                }
            );

            CSVSaver.DataUpdate_SetDestroyedId(id);
        }
        #endregion

        #region Public Methods
        virtual public float Aim()
        {
            if (!hasBeenFocused)
            {
                CSVSaver.DataUpdate_SetFocusId(id);
                hasBeenFocused = true;
            }

            if (timeAtBeginAiming <= 0.0f)
            {
                timeAtBeginAiming = Time.realtimeSinceStartup;
            }

            // Debug.Log($"Aiming : {_durability}");
            if (_durability > 0.0f)
            {
                _durability -= Time.deltaTime;
            }
            return _durability;
        }
        virtual public bool Hit(Vector3 _hitPosition, Vector3 _playerPosition, RaycastHit _hit)
        {
            _hitPoint = _hitPosition;
            // Debug.Log($"hitCount : {hitCount}, hitCountNeededToBreak : {hitCountNeededToBreak}");
            bool result = false;
            hitCount++;
            if (hitCount >= HIT_COUNT_NEEDED_TO_BREAK)
            {
                record(_hitPosition, _playerPosition);
                destroy();
                result = true;
            }
            return result;
        }
        public void ResetAimTime()
        {
            timeAtBeginAiming = 0.0f;
        }
        public void Blur(bool _timeReset = true)
        {
            CSVSaver.DataUpdate_SetBlurId(id);
            if (_timeReset)
            {
                timeAtBeginAiming = 0.0f;
            }
        }
        public void TargetOff()
        {
            gameObject.layer = 0;
            if (_ren != null)
            {
                _ren.material = _materials[0];
            }
        }
        public void TargetOn()
        {
            CSVSaver.DataUpdate_SetSpawnInfo(id, gameObject.transform.position);
            SequentialTaskBehaviour.instance.currentTargetableTarget = this;

            gameObject.layer = initLayer;
            if (_ren != null)
            {
                _ren.material = _materials[1];
            }
        }
        public void SetTimeOfStartingToMove(float _time)
        {
            if (startingToMove == 0.0f)
            {
                startingToMove = _time;
            }
        }
        #endregion
    }
}