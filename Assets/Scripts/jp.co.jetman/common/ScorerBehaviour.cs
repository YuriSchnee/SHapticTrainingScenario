using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using jp.co.jetman.common.data;

namespace jp.co.jetman.common
{
    public enum EnemyCountMode
    {
        CountDown,
        CountUp
    }
    public class ScorerBehaviour : SingletonMonoBehaviour<ScorerBehaviour>
    {
        [Header("References")]
        [SerializeField]
        private TextMeshProUGUI _textHits;
        [SerializeField]
        private TextMeshProUGUI _textAccuracy;
        [SerializeField]
        private TextMeshProUGUI _textConsole;
        [SerializeField]
        private TextMeshProUGUI _textEnemies;

        private EnemyCountMode _mode = EnemyCountMode.CountUp;
        public EnemyCountMode mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }
        private int total = 0;
        private int _hits = 0;
        public int hits
        {
            get
            {
                return _hits;
            }
        }
        private int _shots = 0;
        public int shots
        {
            get
            {
                return _shots;
            }
        }
        private float _accuracy = 0.0f;
        public float accuracy
        {
            get
            {
                return _accuracy;
            }
        }

        private float distance = 0.0f;
        private float reactionSpeed = 0.0f;
        private Vector3 cursorPosition = default(Vector3);

        public bool anyEnemiesRemaining
        {
            get
            {
                return (total - hits) > 0;
            }
        }

        private float totalReactionTime = 0.0f;

        private List<DataUpdate> dataUpdates = new List<DataUpdate>();

        private Vector2 reticlePosition = default(Vector2);

        #region MonoBehaviour
        #endregion

        #region Priavte Method
        private void updateAccuracy()
        {
            _accuracy = (shots == 0) ? 0.0f : (float)hits / (float)shots;
        }
        private void updateText()
        {
            _textHits?.SetText(hits.ToString());
            _textAccuracy?.SetText((accuracy * 100.0f).ToString("F0"));
            _textEnemies?.SetText($"{total - hits}");
            var time = TimerBehaviour.instance.currentTime;

            _textConsole?.SetText($"# {MainBehaviour.instance.mode}\ndistance : {distance.ToString("F3")}(m)\nreaction : {reactionSpeed}(sec)\ncursor position : {cursorPosition}\nelapsed time : {time}(sec)");
        }
        private void updateData()
        {
        }
        #endregion

        #region Public Method
        public void Reset(int _enemies = 0)
        {
            if (_enemies > 0)
            {
                mode = EnemyCountMode.CountDown;
                total = _enemies;
            }
            else
            {
                mode = EnemyCountMode.CountUp;
                total = 99999999;
            }
            _hits = 0;
            _shots = 0;

            totalReactionTime = 0.0f;

            updateAccuracy();
            updateText();
            updateData();
        }
        public void Shot()
        {
            _shots++;
            updateAccuracy();
            updateText();
        }
        public void Hit()
        {
            _hits++;
            updateAccuracy();
            updateText();
        }
        public void RecordDistance(float _distance)
        {
            distance = _distance;
            updateText();
        }
        public void RecordReactionSpeed(float _reactionSpeed)
        {
            reactionSpeed = _reactionSpeed;
            // Debug.Log($"reaction(score) {reactionSpeed}");
            totalReactionTime += reactionSpeed;
            updateText();
        }
        public void RecordCursorPosition(Vector3 _cusorPosition)
        {
            cursorPosition = _cusorPosition;
            updateText();
        }
        #endregion
    }
}