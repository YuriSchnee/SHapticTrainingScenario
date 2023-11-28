using System;
using UnityEngine;
using jp.co.jetman.events;

namespace jp.co.jetman.utils
{
    public class Motion
    {
        #region Properties
        private bool isAnim = false;
        private float endValue = 1.0f;
        private float startValue = 0.0f;
        private float currentValue = 0.0f;
        public float value
        {
            get
            {
                return currentValue;
            }
        }
        private float currentTime = 0.0f;
        private float duration = 1.0f;
        private float delay = 0.0f;
        private string easingName = "";
        private Func<float, float, float, float, float> easing = Easing.linear;
        #endregion

        #region Event
        public event MotionEventHandler motionEventHandler;
        protected virtual void OnMotionEvent(MotionEventArgs _e)
        {
            if (motionEventHandler != null)
            {
                motionEventHandler(this, _e);
            }
        }
        #endregion

        #region Constructor
        public Motion() {}
        #endregion
        
        #region Private Methods
        #endregion
        
        #region Public Methods
        public void Update(float _deltaTime)
        {
            if (isAnim)
            {
                if (delay > 0.0f)
                {
                    delay -= _deltaTime;
                    return;
                }

                if (currentTime < duration)
                {
                    currentTime += _deltaTime;
                }
                else
                {
                    currentTime = duration;
                    isAnim = false;
                }

                var t = currentTime / duration;// normalized time
                if (easing != null)
                {
                    currentValue = easing(t, startValue, (endValue - startValue), 1.0f);
                }
                else
                {
                    currentValue = (float) typeof(Easing).GetMethod(easingName).Invoke(null, new object[] { t, startValue, (endValue - startValue), 1.0f });
                }

                if (!isAnim)
                {
                    OnMotionEvent(new MotionEventArgs(MotionEvent.Complete));
                }
            }
        }
        public void Play(float _duration, Func<float, float, float, float, float> _easing, float _delay = 0.0f)
        {
            duration = _duration;
            delay = _delay;
            easing = _easing;

            currentTime = 0.0f;

            isAnim = true;
        }
        public void Play(float _duration, string _easing = "linear", float _delay = 0.0f)
        {
            duration = _duration;
            easingName = _easing;
            easing = null;
            delay = _delay;

            currentTime = 0.0f;

            isAnim = true;
        }
        public void Pause()
        {
            isAnim = false;
        }
        public void Resume()
        {
            isAnim = true;
        }
        public void Reset()
        {
            isAnim = false;
            
            currentTime = 0.0f;
            currentValue = 0.0f;
        }
        public void Finish()
        {
            isAnim = false;

            currentTime = duration;
            currentValue = 1.0f;
        }
        #endregion
    }
}