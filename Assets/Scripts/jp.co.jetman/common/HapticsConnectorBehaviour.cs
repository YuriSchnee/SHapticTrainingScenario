using System;
using UnityEngine;

namespace jp.co.jetman.common
{
    public class HapticsConnectorBehaviour : SingletonMonoBehaviour<HapticsConnectorBehaviour>
    {
        [Header("Parameters")]
        [SerializeField]
        public float THRESHOLD_FOR_MOVEMENT = 0.01f;

        #region Public Methods
        public void OnTargetAvailable()
        {
            Debug.Log($"ターゲットが打てるようになった。");
        }
        public void OnMouseBeginMove()
        {
            Debug.Log($"マウスが動き出した。");
        }
        public void OnTargetDefeated()
        {
            Debug.Log($"ターゲットを倒した。");
        }
        #endregion
    }
}