using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jp.co.jetman.common;
using jp.co.jetman.common.interfaces;
using jp.co.jetman.sequentialTask.spawners;
using jp.co.jetman.common.gameInstances;

namespace jp.co.jetman.sequentialTask
{

    public class SequentialTaskBehaviour : SingletonMonoBehaviour<SequentialTaskBehaviour>, IAimMenu
    {
        private TaskScene taskScene = TaskScene.Title;


        public readonly int MAX_TARGETS_AT_A_TIME = 2;
        public readonly SpawnMode SPAWN_MODE = SpawnMode.Sequential;

        [Header("[Parameters]")]
        [SerializeField]
        private float TITLE_TIME_LENGTH = 3.0f;
        [SerializeField]
        public SequentialSpawnerParameters SEQUENTIAL_PARAMS;
        public object GetSpawnerParams()
        {
            switch (SPAWN_MODE)
            {
                case SpawnMode.Sequential:
                    return SEQUENTIAL_PARAMS;
                default:
                    throw new Exception();
            }
        }

        [Space(20.0f)]
        [Header("[References]")]
        [SerializeField]
        private UIGameSceneInGameBehaviour _ui;
        // [SerializeField]
        // private PlayerCharacterBehaviour _player;
        [SerializeField]
        private GameObject _prefabHitEffect;
        [SerializeField]
        private AudioSource _soundHit;
        // [SerializeField]
        // private Transform _playerCamera;

        #region IAimMenu
        private bool _playing = false;
        public bool playing
        {
            get
            {
                return _playing;
            }
        }
        private bool _inGame = false;
        public bool inGame
        {
            get
            {
                return _inGame;
            }
        }
        #endregion

        private GameInstanceFactory gameInstanceFactory = new GameInstanceFactory();
        private IGameInstance gameInstance;
        private SpawnerFactory spawnerFactory = new SpawnerFactory();


        [System.NonSerialized]
        public TargetBehaviour currentTargetableTarget = null;


        #region MonoBehaviour
        void Start()
        {
            SpawnPositionLoader.Load();
            // foreach (var p in SpawnPositionLoader.positions)
            // {
            //     Debug.Log($"{p}");
            // }

            gameInstance = gameInstanceFactory.Create();
            gameInstance.Init(this, spawnerFactory.Create());
            gameInstance.Reset();
        }
        void Update()
        {
            gameInstance?.Update();

            if (_playing && gameInstance?.isPlaying == false)
            {
                Debug.Log($"{MainBehaviour.TAG} -----> 終了");

                CSVSaver.DataUpdate_Save();
                CSVSaver.DataResult_Save();

                TimerBehaviour.PushResult(TimerBehaviour.instance.currentTime);
                taskScene = TaskScene.Result;
                Play();
            }
            _playing = gameInstance?.isPlaying ?? false;
        }
        #endregion

        #region Priavte Methods
        private void sequenceStartCountDown()
        {
            CancelInvoke("sequenceStartCountDown");
            taskScene = TaskScene.CountDown;
            Invoke("sequenceCountDown3", 1.0f);
        }
        private void sequenceCountDown3()
        {
            CancelInvoke("sequenceCountDown3");
            _ui.Change(taskScene, 3);
            Invoke("sequenceCountDown2", 1.0f);
        }
        private void sequenceCountDown2()
        {
            CancelInvoke("sequenceCountDown2");
            _ui.Change(taskScene, 2);
            Invoke("sequenceCountDown1", 1.0f);
        }
        private void sequenceCountDown1()
        {
            CancelInvoke("sequenceCountDown1");
            _ui.Change(taskScene, 1);
            Invoke("sequenceCountDown0", 1.0f);
        }
        private void sequenceCountDown0()
        {
            CancelInvoke("sequenceCountDown0");
            taskScene = TaskScene.InGame;
            _ui.Change(taskScene);
            Play();
        }
        #endregion

        #region IAimMenu
        public void Play()
        {
            if (taskScene == TaskScene.Title)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                _ui.Change(taskScene);
                Invoke("sequenceStartCountDown", TITLE_TIME_LENGTH);
            }
            if (taskScene == TaskScene.CountDown)
            {
                // idle
            }
            else if (taskScene == TaskScene.InGame)
            {
                if (gameInstance?.needReset == true)
                {
                    Debug.Log($"{MainBehaviour.TAG} リセットが必要です。");
                }
                if (gameInstance?.isPlaying == false && gameInstance?.needReset == false)
                {
                    Debug.Log($"{MainBehaviour.TAG} 開始 ----->");

                    ScorerBehaviour.instance.Reset(((NKillsGameParameters)AimAppSettings.instance.GetGameParams()).TOTAL_NUMBER_OF_ENEMIES_TO_KILL);
                    MainBehaviour.instance.CSV_ClearDataUpdate();

                    gameInstance?.Play();
                }
            }
            else if (taskScene == TaskScene.Result)
            {
                _ui.Change(taskScene);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        public void Reset()
        {
            taskScene = TaskScene.Title;
            ScorerBehaviour.instance.Reset(((NKillsGameParameters)AimAppSettings.instance.GetGameParams()).TOTAL_NUMBER_OF_ENEMIES_TO_KILL);

            gameInstance?.Reset();
        }
        public void Aim(TargetBehaviour _other)
        {
            // Debug.Log($"Time:{Time.realtimeSinceStartup}, Aim");
            if (gameInstance?.isPlaying == true)
            {
                gameInstance?.Aim(_other);
            }
        }
        public void Shot()
        {
            if (gameInstance?.isPlaying == true)
            {
                ScorerBehaviour.instance.Shot();
            }
            else
            {
                Debug.Log($"{MainBehaviour.TAG} 開始してください。");
            }
        }
        public void Hit(TargetBehaviour _other)
        {
            // Debug.Log($"Time:{Time.realtimeSinceStartup}, Hit");
            if (gameInstance?.isPlaying == true)
            {
                ScorerBehaviour.instance.Hit();

                var g = GameObject.Instantiate(_prefabHitEffect, _other.hitPoint, Quaternion.identity);
                g.transform.parent = gameObject.transform;

                _soundHit.Play();

                gameInstance?.Hit(_other);

                if (MainBehaviour.hapticsMode == HapticsMode.Feedback)
                    HapticsConnectorBehaviour.instance.OnTargetDefeated();
            }
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region Public Methods
        public void Continue()
        {
            if (taskScene == TaskScene.Result)
            {
                MainBehaviour.hapticsMode = HapticsMode.Feedback;
                Reset();

                Play();
            }
        }
        public void Kill()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            CancelInvoke();
            gameInstance?.Stop();
        }
        #endregion
    }
}
