using System.Collections.Generic;
using UnityEngine;

using jp.co.jetman.common.interfaces;
using jp.co.jetman.common.data;
using jp.co.jetman.sequentialTask;

namespace jp.co.jetman.common
{
    public enum AimMenuMode 
    {
        SequentialTask
    }
    public enum GameScene
    {
        Title,
        Menu,
        InGame,
        Result,
        Analytics,
        Recommend
    }
    public enum HapticsMode
    {
        NoFeedback,
        Feedback
    }

    public class MainBehaviour : SingletonMonoBehaviour<MainBehaviour>
    {
        static public readonly string TAG = "[SYSTEM][DEBUG]";

        static public HapticsMode hapticsMode = HapticsMode.NoFeedback;

        private GameScene currentScene = GameScene.Title;

        [SerializeField]
        private PlayerCharacterBehaviour _player;
        [SerializeField]
        private GameObject[] _tasks;

        private IAimMenu _currentAimMenu;
        public IAimMenu currentAimMenu
        {
            get
            {
                return _currentAimMenu;
            }
        }

        private AimMenuMode _aimMenuMode;
        public AimMenuMode mode
        {
            get
            {
                return _aimMenuMode;
            }
            set
            {
                _aimMenuMode = value;
            }
        }

        #region for CSV
        [System.NonSerialized]
        public int gameCounter = 0;

        public void CSV_ClearDataUpdate()
        {
            MainBehaviour.instance.gameCounter++;
            CSVSaver.DataUpdate_Clear();
            CSVSaver.DataResult_Clear();
        }
        #endregion

        #region MonoBehaviour
        void Awake()
        {
            Application.targetFrameRate = AimAppSettings.instance.TARGET_FRAME_RATE;
        }
        void Start()
        {
            _currentAimMenu = _tasks[0].GetComponent<IAimMenu>();

            resetScene();
        }
        void Update()
        {
            // シーン切り替え
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                resetScene();
            }

            // アプリケーション終了
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (currentScene == GameScene.InGame)
            {
                // プレイヤー視点リセット
                if (Input.GetKeyDown(KeyCode.C))
                {
                    _player.Reset();
                }
            }
        }
        void LateUpdate()
        {
            if (currentAimMenu?.playing == true)
            {
                CSVSaver.DataUpdate_Record();
            }
        }
        void OnDestroy()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        #endregion

        #region Private Methods
        private void nextScene()
        {
            switch (currentScene)
            {
                case GameScene.Title:
                    currentScene = GameScene.Menu;
                    UIManagerBehaviour.instance.Play(currentScene);

                    currentAimMenu?.Show();
                    break;
                case GameScene.Menu:
                    currentScene = GameScene.InGame;
                    MainBehaviour.hapticsMode = HapticsMode.NoFeedback;

                    TimerBehaviour.ResetResults();
                    currentAimMenu?.Reset();
                    UIManagerBehaviour.instance.Play(currentScene);
                    break;
                case GameScene.InGame:
                    if (!(currentAimMenu.inGame))
                    {
                        currentScene = GameScene.Result;
                        UIManagerBehaviour.instance.Play(currentScene);
                    }
                    break;
                case GameScene.Result:
                    currentScene = GameScene.Analytics;
                    UIManagerBehaviour.instance.Play(currentScene);
                    break;
                case GameScene.Analytics:
                    currentScene = GameScene.Recommend;
                    UIManagerBehaviour.instance.Play(currentScene);
                    break;
                case GameScene.Recommend:
                    resetScene();
                    break;
            }
        }
        private void resetScene()
        {
            currentScene = GameScene.Title;
            UIManagerBehaviour.instance.Play(currentScene);

            CSVSaver.ResetSavePath();
            gameCounter = 0;
        }
        #endregion

        #region Public Methods
        public PlayerCharacterBehaviour GetPlayer()
        {
            return _player;
        }
        public void Aim(TargetBehaviour _other)
        {
            _currentAimMenu.Aim(_other);
        }
        public void Hit(TargetBehaviour _other)
        {
            _currentAimMenu.Hit(_other);
        }
        public void Shot()
        {
            if (currentAimMenu?.playing == true)
                CSVSaver.DataUpdate_SetShot();

            _currentAimMenu.Shot();
        }
        public void NextScene()
        {
            nextScene();
        }
        #endregion
    }
}

