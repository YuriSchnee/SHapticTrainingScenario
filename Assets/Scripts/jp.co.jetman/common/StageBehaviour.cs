using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jp.co.jetman.common
{
    [ExecuteAlways]
    public class StageBehaviour : SingletonMonoBehaviour<StageBehaviour>
    {
        [Header("[Parameters]")]
        [SerializeField, Range(10.0f, 20.0f)]
        public float INNER_RADIUS = 15.0f;

        [SerializeField, Range(10.0f, 20.0f)]
        public float OUTER_RADIUS = 20.0f;

        [SerializeField, Range(0.1f, 10.0f)]
        public float STEP_HEIGHT = 2.9f;

        [Space(20.0f)]
        [Header("[References]")]
        [SerializeField]
        private Transform _wall;
        private float initDistanceToWall = 20.0f;
        [SerializeField]
        private MeshRenderer _meshWall1;
        [SerializeField]
        private MeshRenderer _meshWall2;
        [SerializeField]
        private Transform _step;
        private float initDistanceToStep = 15.15f;
        private float initStepHeight = 2.9f;
        [SerializeField]
        private MeshRenderer _meshStep1;
        [SerializeField]
        private MeshRenderer _meshStep2;


        public bool stepAvailable
        {
            get
            {
                return _step.gameObject.activeSelf;
            }
        }

        #region MonoBehaviour
        void Start()
        {
            
        }
        void Update()
        {
            updateWall();
            updateStep();
        }
        #endregion

        #region Priavte Methods
        private void updateWall()
        {
            var ratio = OUTER_RADIUS / initDistanceToWall;
            _wall.transform.localScale = (Vector3.right + Vector3.forward) * ratio + Vector3.up;

            _meshWall1.sharedMaterial.mainTextureScale = new Vector2(1.0f, ratio);
            _meshWall2.sharedMaterial.mainTextureScale = new Vector2(1.0f, ratio);
        }
        private void updateStep()
        {
            var ratio = INNER_RADIUS / initDistanceToStep;
            var ratioV = STEP_HEIGHT / initStepHeight;
            _step.transform.localScale = (Vector3.right + Vector3.forward) * ratio + Vector3.up * ratioV;

            _meshStep1.sharedMaterials[0].mainTextureScale = new Vector2(ratio, ratio);
            _meshStep1.sharedMaterials[1].mainTextureScale = new Vector2(ratioV, ratio);
            _meshStep2.sharedMaterials[0].mainTextureScale = new Vector2(ratio, ratio);
            _meshStep2.sharedMaterials[1].mainTextureScale = new Vector2(ratioV, ratio);

        }
        #endregion

        #region Public Methods
        public void AddStep()
        {
            _step.gameObject.SetActive(true);
        }
        public void RmoveStep()
        {
            _step.gameObject.SetActive(false);
        }
        #endregion
    }
}

