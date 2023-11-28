
using System;
using System.Collections.Generic;
using jp.co.jetman.sequentialTask;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace jp.co.jetman.common
{
    public class UIGameSceneResultBehaviour : UIGameSceneBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _result1;
        [SerializeField]
        private TextMeshProUGUI _result2;

        #region Private Methods
        #endregion

        #region UIGameSceneBehaviour
        override public void Show(GameScene _scene)
        {
            base.Show(_scene);

            if (_scene == GameScene.Result)
            {
                _result1.text = $"{TimerBehaviour.results[0].ToString("F2")}";
                _result2.text = $"{TimerBehaviour.results[1].ToString("F2")}";
            }
        }
        #endregion

        #region Public Methods
        #endregion

    }
}