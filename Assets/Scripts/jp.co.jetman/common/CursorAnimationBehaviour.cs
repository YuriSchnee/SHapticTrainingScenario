using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jp.co.jetman.common
{
    public class CursorAnimationBehaviour : CursorBehaviour
    {
        [Header("[References]")]
        [SerializeField]
        private Animator _anim;
        private const string PARAMETER_NAME = "Targeting";

        #region MonoBehaviour
        void Start()
        {
        }
        #endregion

        #region Private Methods
        #endregion

        #region CursorBehaviour # Public Methods
        override public void Focus()
        {
            if (AimAppSettings.instance.CURSOR_CHANGE_COLOR)
            {
                _anim.SetBool(PARAMETER_NAME, true);
            }
        }
        override public void Blur()
        {
            if (AimAppSettings.instance.CURSOR_CHANGE_COLOR)
            {
                _anim.SetBool(PARAMETER_NAME, false);
            }
        }
        #endregion
    }
}

