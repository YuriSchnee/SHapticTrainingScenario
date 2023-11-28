using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jp.co.jetman.common
{
    public class HitEffectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _effect;

        #region MonoBehaviour
        void Start()
        {
            _effect.Play();
            Invoke("destroy", 1.0f);
        }
        #endregion

        #region Private Methods
        private void destroy()
        {
            GameObject.Destroy(gameObject);
        }
        #endregion
    }
}

