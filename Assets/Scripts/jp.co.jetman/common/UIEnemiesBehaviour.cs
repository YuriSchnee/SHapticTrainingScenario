using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jp.co.jetman.common
{
    public class UIEnemiesBehaviour : MonoBehaviour
    {
        #region MonoBehaviour
        void Start()
        {
            gameObject.SetActive(AimAppSettings.instance?.GAME_MODE == GameMode.NKills);
        }
        #endregion

        #region Priavte Methods
        #endregion
    }
}
