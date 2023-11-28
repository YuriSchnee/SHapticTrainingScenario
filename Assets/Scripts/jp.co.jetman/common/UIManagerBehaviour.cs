
using System.Collections.Generic;
using jp.co.jetman.common.gameInstances;
using UnityEngine;

using TMPro;

namespace jp.co.jetman.common
{
    public class UIManagerBehaviour : SingletonMonoBehaviour<UIManagerBehaviour>
    {
        [SerializeField]
        private UIGameSceneBehaviour[] _scenes;
        [SerializeField]
        private Canvas _canvasBg;

        [SerializeField]
        private TextMeshProUGUI _text;

        #region MonoBehaviour
        void Start()
        {
        }
        void Update()
        {
            _text?.SetText($"{((NKillsGameParameters)AimAppSettings.instance.GetGameParams()).TOTAL_NUMBER_OF_ENEMIES_TO_KILL}");
        }
        #endregion

        #region Public Methods
        public void Play(GameScene _scene)
        {
            foreach (var s in _scenes)
            {
                s.Show(_scene);
            }
            _canvasBg.gameObject.SetActive(
                _scene == GameScene.Title ||
                _scene == GameScene.Menu ||
                _scene == GameScene.Result ||
                _scene == GameScene.Analytics ||
                _scene == GameScene.Recommend
            );
        }
        #endregion

        #region Private Methods
        private void addEnemies(int _delta)
        {
            var i = ((NKillsGameParameters)AimAppSettings.instance.GetGameParams()).TOTAL_NUMBER_OF_ENEMIES_TO_KILL;
            i += _delta;
            i = Mathf.Max(1, i);
            ((NKillsGameParameters)AimAppSettings.instance.GetGameParams()).TOTAL_NUMBER_OF_ENEMIES_TO_KILL = i;

            _text?.SetText($"{i}");
        }
        #endregion

        #region Bindings
        public void Bind_001_OnClickNext()
        {
            MainBehaviour.instance.NextScene();
        }
        public void Bind_002_OnClickNext()
        {
            MainBehaviour.instance.NextScene();
        }
        public void Bind_004_OnClickNext()
        {
            MainBehaviour.instance.NextScene();
        }
        public void Bind_005_OnClickNext()
        {
            MainBehaviour.instance.NextScene();
        }
        public void Bind_006_OnClickNext()
        {
            MainBehaviour.instance.NextScene();
        }

        public void Bind_002_OnClickIncrease()
        {
            addEnemies(1);
        }
        public void Bind_002_OnClickDecrease()
        {
            addEnemies(-1);
        }

        public void Bind_003_OnClickContinue()
        {
            foreach (var s in _scenes)
            {
                if (s.scene == GameScene.InGame)
                {
                    ((UIGameSceneInGameBehaviour)s).Continue();
                    break;
                }
            }
        }
        public void Bind_003_OnClickNext()
        {
            MainBehaviour.instance.NextScene();
        }
        #endregion
    }
}