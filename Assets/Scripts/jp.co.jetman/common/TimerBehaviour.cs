using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace jp.co.jetman.common
{
    public enum TimerMode
    {
        CountDown,
        CountUp
    }

    public class TimerBehaviour : SingletonMonoBehaviour<TimerBehaviour>
    {
        static public List<float> results = new List<float>();
        static public void ResetResults()
        {
            results.Clear();
        }
        static public void PushResult(float _time)
        {
            results.Add(_time);
        }

        private float _currentTime = 0.0f;// (s)
        public float currentTime
        {
            get
            {
                return _currentTime;
            }
        }
        private float duration = 60.0f;// (s)
        private float increaseSign = -1.0f;
        private bool _isRunning = false;
        public bool isRunning
        {
            get
            {
                return _isRunning;
            }
        }
        private TimerMode _mode = TimerMode.CountDown;
        public TimerMode mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                increaseSign = _mode == TimerMode.CountDown ? -1.0f : 1.0f;
            }
        }

        #region MonoBehaviour
        void FixedUpdate()
        {
            if (isRunning)
            {
                _currentTime += Time.fixedDeltaTime * increaseSign;
                if (_currentTime < 0.0f)
                {
                    _currentTime = 0.0f;
                    _isRunning = false;
                }
            }
        }
        #endregion

        #region Priavte Method
        #endregion

        #region Public Method
        public void Play()
        {
            _isRunning = true;
        }
        public void Stop()
        {
            _isRunning = false;
        }
        public void Reset(float _duration = 0.0f)
        {
            if (_duration <= 0.0f)
            {
                mode = TimerMode.CountUp;
                duration = 0.0f;
                _currentTime = 0.0f;
            }
            else
            {
                mode = TimerMode.CountDown;
                duration = _duration;
                _currentTime = duration;
            }
        }
        #endregion
    }
}