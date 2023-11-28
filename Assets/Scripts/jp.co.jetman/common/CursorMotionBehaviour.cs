using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jp.co.jetman.utils;
using jp.co.jetman.events;
using Motion = jp.co.jetman.utils.Motion;

namespace jp.co.jetman.common
{
    public class CursorMotionBehaviour : CursorBehaviour
    {
        [Header("[References]")]
        [SerializeField]
        private RectTransform _pointer;
        [SerializeField]
        private RectTransform _dot;
        [SerializeField]
        private RectTransform _expanding;

        private bool isFocus = false;

        private readonly float MOTION_DURATION = 0.2f;
        private readonly float MOTION_DELAY = 0.0f;
        private readonly float FOCUS_SCALE = 0.89f;
        private readonly float BLUR_SCALE = 1.0f;
        private Motion motion;
        private float startScale = 1.0f;
        private float endScale = 1.0f;

        private readonly float FOCUS_SCALE_POINTER = 0.9f;
        private readonly float BLUR_SCALE_POINTER = 1.0f;
        private Motion motionPointer;
        private float startScalePointer = 1.0f;
        private float endScalePointer = 1.0f;

        private readonly float FADE_DURATION = 0.2f;
        private readonly float FADE_DELAY = 0.0f;
        private readonly float FOCUS_ALPHA = 0.6f;
        private readonly float BLUR_ALPHA = 1.0f;
        private Fade fade;
        private CanvasGroup fadeCanvas;
        private float startAlpha = 1.0f;
        private float endAlpha = 1.0f;

        private readonly float FOCUS_ALPHA_POINTER = 1.0f;
        private readonly float BLUR_ALPHA_POINTER = 0.0f;
        private CanvasGroup fadeCanvasPointer;
        private float startAlphaPointer = 0.0f;
        private float endAlphaPointer = 0.0f;

        #region MonoBehaviour
        void Awake()
        {
            fadeCanvas = _expanding.gameObject.GetComponent<CanvasGroup>();
            fadeCanvasPointer = _pointer.gameObject.GetComponent<CanvasGroup>();

            motion = new Motion();
            motion.motionEventHandler += OnMotionEventHandler;

            motionPointer = new Motion();

            fade = new Fade();
            fade.fadeEventHandler += OnFadeEventHandler;
        }
        void Start()
        {
        }
        void Update()
        {
            gameObject.transform.localScale = Vector3.one * MainBehaviour.instance.GetPlayer().RETICLE_SIZE_SCALE;

            updateAlpha();
            updateScale();
        }
        #endregion

        #region Private Methods
        private void updateAlpha()
        {
            fade.Update(Time.deltaTime);
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, endAlpha, fade.value);
            fadeCanvasPointer.alpha = Mathf.Lerp(startAlphaPointer, endAlphaPointer, fade.value);
        }
        private void updateScale()
        {
            motion.Update(Time.deltaTime);
            _expanding.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, motion.value);

            motionPointer.Update(Time.deltaTime);
            _pointer.gameObject.SetActive(true);
            _pointer.localScale = Vector3.one * Mathf.Lerp(startScalePointer, endScalePointer, motionPointer.value);
        }
        private void OnMotionEventHandler(object _sender, MotionEventArgs _e)
        {
            if (_e.type == MotionEvent.Complete)
            {
            }
        }
	    private void OnFadeEventHandler(object _sender, FadeEventArgs _e)
        {
            if (_e.type == FadeEvent.Complete)
            {
            }
        }
        #endregion

        #region CursorBehaviour # Public Methods
        override public void Focus()
        {
            if (AimAppSettings.instance.CURSOR_CHANGE_COLOR)
            {
                if (!isFocus)
                {
                    isFocus = true;

                    startAlpha = fadeCanvas.alpha;
                    endAlpha = FOCUS_ALPHA;
                    startAlphaPointer = fadeCanvasPointer.alpha;
                    endAlphaPointer = FOCUS_ALPHA_POINTER;
                    fade.Play(FADE_DURATION, Easing.linear, FADE_DELAY);

                    startScale = _expanding.localScale.x;
                    endScale = FOCUS_SCALE;
                    motion.Play(MOTION_DURATION, Easing.easeInOutQuad, MOTION_DELAY);

                    startScalePointer = _pointer.localScale.x;
                    endScalePointer = FOCUS_SCALE_POINTER;
                    motionPointer.Play(MOTION_DURATION, Easing.easeInOutBack, MOTION_DELAY);
                }
            }
        }
        override public void Blur()
        {
            if (AimAppSettings.instance.CURSOR_CHANGE_COLOR)
            {
                if (isFocus)
                {
                    isFocus = false;

                    startAlpha = fadeCanvas.alpha;
                    endAlpha = BLUR_ALPHA;
                    startAlphaPointer = fadeCanvasPointer.alpha;
                    endAlphaPointer = BLUR_ALPHA_POINTER;
                    fade.Play(FADE_DURATION, Easing.linear, FADE_DELAY);

                    startScale = _expanding.localScale.x;
                    endScale = BLUR_SCALE;
                    motion.Play(MOTION_DURATION, Easing.easeInOutQuad, MOTION_DELAY);

                    startScalePointer = _pointer.localScale.x;
                    endScalePointer = BLUR_SCALE_POINTER;
                    motionPointer.Play(MOTION_DURATION, Easing.easeInBack, MOTION_DELAY);
                }
            }
        }
        #endregion
    }
}

