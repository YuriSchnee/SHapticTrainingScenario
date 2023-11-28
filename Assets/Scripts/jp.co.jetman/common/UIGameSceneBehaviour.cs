
using System;
using System.Collections.Generic;
using UnityEngine;

namespace jp.co.jetman.common
{
    public class UIGameSceneBehaviour : MonoBehaviour
    {
        [SerializeField]
        public GameScene scene;
        
        #region Public Methods

        virtual public void Show(GameScene _scene)
        {
            gameObject.SetActive(_scene == scene);
        }
        #endregion
    }
}