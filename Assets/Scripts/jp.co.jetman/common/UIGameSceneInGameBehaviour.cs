
using System;
using System.Collections.Generic;
using jp.co.jetman.sequentialTask;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace jp.co.jetman.common
{
    public enum TaskScene
    {
        Title,
        CountDown,
        InGame,
        Result
    }

    public class UIGameSceneInGameBehaviour : UIGameSceneBehaviour
    {
        [SerializeField]
        private SequentialTaskBehaviour _task;
        
        [SerializeField]
        private RawImage _title01;
        [SerializeField]
        private RawImage _title02;
        [SerializeField]
        private Image _cd3;
        [SerializeField]
        private Image _cd2;
        [SerializeField]
        private Image _cd1;
        [SerializeField]
        private RawImage _result01;
        [SerializeField]
        private RawImage _result02;
        [SerializeField]
        private GameObject _uiCursor;
        [SerializeField]
        private TextMeshProUGUI _resultText01;
        [SerializeField]
        private TextMeshProUGUI _resultText02;


        #region Private Methods
        private void changeScene(TaskScene _scene, uint _count = 0)
        {
            _title01.gameObject.SetActive(_scene == TaskScene.Title && MainBehaviour.hapticsMode == HapticsMode.NoFeedback);
            _title02.gameObject.SetActive(_scene == TaskScene.Title && MainBehaviour.hapticsMode == HapticsMode.Feedback);
            _cd3.gameObject.SetActive(_scene == TaskScene.CountDown && _count == 3);
            _cd2.gameObject.SetActive(_scene == TaskScene.CountDown && _count == 2);
            _cd1.gameObject.SetActive(_scene == TaskScene.CountDown && _count == 1);
            _result01.gameObject.SetActive(_scene == TaskScene.Result && MainBehaviour.hapticsMode == HapticsMode.NoFeedback);
            if (_scene == TaskScene.Result && MainBehaviour.hapticsMode == HapticsMode.NoFeedback)
            {
                _resultText01.text = $"{TimerBehaviour.results[0].ToString("F2")}";
            }
            _result02.gameObject.SetActive(_scene == TaskScene.Result && MainBehaviour.hapticsMode == HapticsMode.Feedback);
            if (_scene == TaskScene.Result && MainBehaviour.hapticsMode == HapticsMode.Feedback)
            {
                _resultText02.text = $"{TimerBehaviour.results[1].ToString("F2")}";
            }
            _uiCursor.gameObject.SetActive(_scene == TaskScene.InGame);
        }
        #endregion

        #region UIGameSceneBehaviour
        override public void Show(GameScene _scene)
        {
            base.Show(_scene);
            if (_scene == GameScene.InGame)
            {
                _task.Play();
            }
            else
            {
                _task.Kill();
            }
        }
        #endregion

        #region Public Methods
        public void Change(TaskScene _scene, uint _count = 0)
        {
            changeScene(_scene, _count);
        }
        public void Continue()
        {
            _task.Continue();
        }
        #endregion

    }
}