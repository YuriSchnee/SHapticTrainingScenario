using jp.co.jetman.sequentialTask;
using UnityEngine;


namespace jp.co.jetman.common
{
    public class PlayerCharacterBehaviour : MonoBehaviour
    {
        [Header("[Parameters]")]
        [SerializeField]
        public string ID_TEST_SUBJECT = "P001";

        [Space(20.0f)]
        // Input.GetAxis()
        // https://docs.unity3d.com/ja/2021.1/ScriptReference/Input.GetAxis.html
        // ※Input Mouse X, Mouse Y の Sensitivity を変更してもよい。フレームレート非依存。
        [Header("[Parameters, Movement Rate]")]
        [SerializeField]
        private float RATE_MOVEMENT_HORIZONTAL_DEGREE = 1.0f;// Mouse X 1 カウントで移動する Yaw (degree)
        [SerializeField]
        private float RATE_MOVEMENT_VERTICAL_DEGREE = 1.0f;// Mouse Y 1 カウントで移動する Pitch (degree)
        private float MOVE_SPEED = 1.2f;// 1(m/s)

        [Space(20.0f)]
        [Header("[Parameters, Restriction]")]
        [SerializeField]
        private float MAX_PITCH_DEGREE = 89.9f;// 最大 Pitch (degree)
        [SerializeField]
        private float LENGTH_SPHERECAST = 50.0f;// (m)
        [SerializeField]
        private float RADIUS_SPHERECAST = 0.1f;// (m)
        [SerializeField]
        public float RETICLE_SIZE_SCALE = 1.0f;

        [Space(20.0f)]
        [Header("[References]")]
        [SerializeField]
        private Transform _camera;
        [SerializeField]
        private AudioSource _soundShoot;
        [SerializeField]
        private ParticleSystem _flashGun;
        [SerializeField]
        private CursorBehaviour _cursor;
        [SerializeField]
        private LayerMask _layerMask;

        private Vector3 currentPosition = default(Vector3);
        private Vector3 currentAngles = default(Vector3);

        private RaycastHit hit;
        private bool isHit;
        private bool isTrigger = false;

        private TargetBehaviour currentTarget = null;

        #region MonoBehaviour
        void Start()
        {
            currentPosition = gameObject.transform.position;
            currentAngles = gameObject.transform.eulerAngles;
        }
        void FixedUpdate()
        {
            updateRaycast();
            updateTrigger();
        }
        void Update()
        {
            updatePosition();
            updateRotation();
            gameObject.transform.position = currentPosition;
            _camera.rotation = Quaternion.Euler(currentAngles);

            var p1 = _camera;
            if (isHit)
            {
                ScorerBehaviour.instance.RecordCursorPosition(p1.position + p1.forward * hit.distance);
                _cursor.Focus();
            }
            else
            {
                ScorerBehaviour.instance.RecordCursorPosition(p1.position + p1.forward * LENGTH_SPHERECAST);
                _cursor.Blur();
            }
        }
        void OnDrawGizmos()
        {
            var p1 = _camera;
            if (isHit)
            {
                Gizmos.DrawRay(p1.position, p1.forward * hit.distance);
                Gizmos.DrawWireSphere(p1.position + p1.forward * hit.distance, RADIUS_SPHERECAST);
            }
            else
            {
                Debug.DrawRay(p1.position, p1.forward * LENGTH_SPHERECAST, Color.green);
            }
        }
        #endregion

        #region Private Methods
        private void updatePosition()
        {
            if (AimAppSettings.instance.PLAYER_CAN_MOVE)
            {
                var h = Input.GetAxisRaw("Horizontal");
                var v = Input.GetAxisRaw("Vertical");
                var f = _camera.forward * v;
                f.y = 0.0f;
                var r = _camera.right * h;
                r.y = 0.0f;
                currentPosition = currentPosition + (f + r).normalized * MOVE_SPEED * Time.deltaTime;
            }
        }

        private bool isCursorMoving = false;
        private float arrangeAngle(float _angle)
        {
            if (_angle > 181.0f)
            {
                _angle -= 360.0f;
            }
            return _angle;
        }
        private void updateRotation()
        {
            var h = Input.GetAxisRaw("Mouse X");
            var v = Input.GetAxisRaw("Mouse Y") * (-1.0f);
            var yaw = currentAngles.y + h * RATE_MOVEMENT_HORIZONTAL_DEGREE;
            // var pitch = currentAngles.x + v * RATE_MOVEMENT_VERTICAL_DEGREE;
            var pitch = Mathf.Max(Mathf.Min(currentAngles.x + v * RATE_MOVEMENT_VERTICAL_DEGREE, MAX_PITCH_DEGREE), -MAX_PITCH_DEGREE);
            currentAngles = new Vector3(pitch, yaw, 0.0f);
            // Debug.Log(currentAngles);
            // Debug.Log($"{h.ToString("f3")}, {v.ToString("f3")}");

            if (MainBehaviour.instance.currentAimMenu.playing)
            {
                CSVSaver.DataUpdate_SetCameraRotation(new Vector2(arrangeAngle(yaw), arrangeAngle(pitch)));
            }

            if (Vector2.Distance(Vector2.zero, Vector2.right * h + Vector2.up * v) > HapticsConnectorBehaviour.instance.THRESHOLD_FOR_MOVEMENT)
            {
                if (!isCursorMoving)
                {
                    if (MainBehaviour.instance.currentAimMenu.playing)
                    {
                        if (MainBehaviour.hapticsMode == HapticsMode.Feedback)
                            HapticsConnectorBehaviour.instance.OnMouseBeginMove();

                        CSVSaver.DataUpdate_SetStartingToMove();
                        SequentialTaskBehaviour.instance.currentTargetableTarget?.SetTimeOfStartingToMove((float) CSVSaver.durationTime);
                    }
                }
                isCursorMoving = true;
            }
            else
            {
                isCursorMoving = false;
            }
        }
        private void updateRaycast()
        {
            if (MainBehaviour.instance.currentAimMenu.playing)
            {
                var p1 = _camera;
                TargetBehaviour comp = null;
                isHit = Physics.SphereCast(p1.position, RADIUS_SPHERECAST, p1.forward, out hit, LENGTH_SPHERECAST, _layerMask);
                if (isHit)
                {
                    comp = hit.collider.gameObject.GetComponent<TargetBehaviour>();
                    if (comp == null)
                    {
                        comp = hit.collider.gameObject.GetComponentInParent<TargetBehaviour>();
                    }
                }

                var rayDistance = LENGTH_SPHERECAST;
                if (comp != null)
                {
                    if (comp != currentTarget)
                    {
                        currentTarget?.Blur();
                        currentTarget = comp;
                    }

                    var durability = comp.Aim();
                    MainBehaviour.instance.Aim(comp);

                    rayDistance = Vector3.Distance(p1.position, hit.collider.ClosestPoint(comp.gameObject.transform.position));
                }
                else
                {
                    currentTarget?.Blur();
                    currentTarget = null;
                }

                // Record Reticle Position
                var vec2 = new Vector2(p1.eulerAngles.y, LENGTH_SPHERECAST * Mathf.Sin((-1.0f) * p1.eulerAngles.x * Mathf.Deg2Rad) + p1.position.y);
                CSVSaver.DataUpdate_SetReticlePosition(vec2, p1.eulerAngles.x);
                CSVSaver.DataUpdate_SetRayDistance(rayDistance);
            }
        }
        private void updateTrigger()
        {
            if (MainBehaviour.instance.currentAimMenu.playing)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (!isTrigger)
                    {
                        // Debug.Log($"Time:{Time.realtimeSinceStartup}, Fire");
                        isTrigger = true;

                        _soundShoot.Play();
                        _flashGun.Play();

                        MainBehaviour.instance.Shot();

                        if (isHit)
                        {
                            var comp = hit.collider.gameObject.GetComponent<TargetBehaviour>();
                            if (comp == null)
                            {
                                comp = hit.collider.gameObject.GetComponentInParent<TargetBehaviour>();
                            }
                            if (comp != null)
                            {
                                // 奥行き分常にずれがある
                                // ScorerBehaviour.instance.RecordDistance(Vector3.Distance(target.position, hit.point));

                                if (comp.Hit(hit.point, _camera.position, hit))
                                {
                                    MainBehaviour.instance.Hit(comp);
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (isTrigger)
                {
                    isTrigger = false;
                }
            }
        }
        #endregion

        #region Public Methods
        public void Reset()
        {
            currentAngles = Vector3.zero;
        }
        public Transform GetCameraTransform()
        {
            return _camera;
        }
        #endregion
    }
}

