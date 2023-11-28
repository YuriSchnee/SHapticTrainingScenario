using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jp.co.jetman.common
{
    public abstract class CursorBehaviour : MonoBehaviour
    {
        #region MonoBehaviour
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        abstract public void Focus();
        abstract public void Blur();
        #endregion
    }
}

