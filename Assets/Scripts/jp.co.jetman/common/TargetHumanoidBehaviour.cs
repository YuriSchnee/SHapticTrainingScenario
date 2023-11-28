using UnityEngine;

namespace jp.co.jetman.common
{
    public class TargetHumanoidBehaviour : TargetBehaviour
    {
        [Header("[Parameters(Humanoid)]")]
        [SerializeField]
        private Vector3 OFFSET_BODY_TARGET_CENTER = Vector3.zero;
        [SerializeField]
        protected int HIT_COUNT_NEEDED_TO_BREAK_BODY = 2;
        [SerializeField]
        private float MAX_DISTANCE_3D_SOUND = 10.0f;

        [Space(20.0f)]
        [Header("[References]")]
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private AudioSource _audio;
        [SerializeField]
        private SkinnedMeshRenderer _mesh;

        private bool walking = false;
        private Vector3 walkingVector = default(Vector3);

        override public Vector3 position
        {
            set
            {
                var r = Vector2.Distance(Vector2.zero, new Vector2(value.x, value.z));
                if (StageBehaviour.instance.stepAvailable)
                {
                    value.y = r < StageBehaviour.instance.INNER_RADIUS ? 0.0f : StageBehaviour.instance.STEP_HEIGHT;
                }
                else
                {
                    value.y = 0.0f;
                }
                gameObject.transform.position = value;
            }
        }

        #region TargetBehaviour
        protected override void Awake()
        {
            base.Awake();
            _audio.maxDistance = MAX_DISTANCE_3D_SOUND;
        }
        override protected void Start()
        {
            base.Start();
        }
        override protected void Update()
        {
            if (playerTransform == null)
            {
                playerTransform = MainBehaviour.instance.GetPlayer().GetCameraTransform();
            }
            else
            {
                if (!walking)
                {
                    var p = playerTransform.position;
                    gameObject.transform.LookAt(new Vector3(p.x, gameObject.transform.position.y, p.z));
                }
                else
                {
                    walkingVector.y = gameObject.transform.position.y;
                    gameObject.transform.LookAt(walkingVector);
                }
            }
        }
        #endregion

        #region Private Methods
        protected void record(Vector3 _hitPosition, Vector3 _playerPosition, RaycastHit _hit)
        {
            var timeReaction = Time.realtimeSinceStartup - timeAtBeginAiming;
            // Debug.Log($"reaction {timeReaction}");
            ScorerBehaviour.instance.RecordReactionSpeed(timeReaction);
            var timeLife = Time.realtimeSinceStartup - timeAtSpawned;

            var targetPosition = _hit.collider.gameObject.name == "head" ? gameObject.transform.TransformPoint(OFFSET_TARGET_CENTER) : gameObject.transform.TransformPoint(OFFSET_BODY_TARGET_CENTER);
            var p1 = _playerPosition;
            var distance = Vector3.Distance(Vector3.LerpUnclamped(p1, targetPosition, Vector3.Magnitude(_hitPosition - p1) / Vector3.Magnitude(targetPosition - p1)), _hitPosition);
            ScorerBehaviour.instance.RecordDistance(distance);

            saveDataResults(timeReaction, timeLife, distance, targetPosition);
        }
        #endregion

        #region Public Methods
        public void Walk(Vector3 _to, float _speed = 1.0f)
        {
            walkingVector = _to;
            walking = true;
            _anim.SetBool("Walk", true);
            _anim.speed = _speed;
        }
        public void SetInvisible(bool _flag)
        {
            _mesh.enabled = !_flag;
        }
        #endregion

        #region TargetBehaviour # Public Methods
        override public bool Hit(Vector3 _hitPosition, Vector3 _playerPosition, RaycastHit _hit)
        {
            // base.Hit(_hitPosition, _playerPosition, _hit)    // DoN't call
 
            _hitPoint = _hitPosition;

            bool result = false;
            hitCount++;
            var breakCount = _hit.collider.gameObject.name == "head" ? HIT_COUNT_NEEDED_TO_BREAK : HIT_COUNT_NEEDED_TO_BREAK_BODY; 
            if (hitCount >= breakCount)
            {
                record(_hitPosition, _playerPosition, _hit);
                destroy();
                result = true;
            }
            return result;
        }
        #endregion
    }
}