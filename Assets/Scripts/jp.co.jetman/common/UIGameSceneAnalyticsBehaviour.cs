using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using jp.co.jetman.common.graph;
using jp.co.jetman.common.gameInstances;

namespace jp.co.jetman.common
{
    public class UIGameSceneAnalyticsBehaviour : UIGameSceneBehaviour
    {
        private enum PlayerDataMode
        {
            NO_FEEDBACK,
            FEEDBACK
        }

        [Header("Parameters")]
        [SerializeField]
        private PlayerDataMode playerDataMode = PlayerDataMode.NO_FEEDBACK;

        [SerializeField]
        private int _enemyIndex = 1;

        [Space(20)]
        [Header("References")]
        [SerializeField]
        private GraphLineBehaviour _graph1;
        [SerializeField]
        private GraphLineBehaviour _graph2;

        [SerializeField]
        private GraphPointsBehaviour _point1;
        [SerializeField]
        private GraphPointsBehaviour _point2;

        [SerializeField]
        private TextMeshProUGUI _timeText;
        [SerializeField]
        private Image _timeLabel;
        [SerializeField]
        private Sprite[] _timeLabelSprites;

        private readonly string PRO_CSV_FILE = "speed_ID02P003_t10.csv";

        #region Gap
        private float cognitionTimePlayer = 0.0f;
        private float cognitionTimePro = 0.0f;
        private float clickedTimePlayer = 0.0f;
        private float clickedTimePro = 0.0f;
        private bool isCognition = true;
        #endregion


        #region MonoBehaviour
        void Update()
        {
        }
        #endregion

        #region Private Methods
        private void drawGraph()
        {
            cognitionTimePlayer = 0.0f;
            cognitionTimePro = 0.0f;
            clickedTimePlayer = 0.0f;
            clickedTimePro = 0.0f;

            #region TEST
            // var path = "F:/Sony/Haptic/INZONE_TrainingToolApp/Assets/csv/2023-11-27_00-42-13_P001/update_001.csv";
            // var fieldList = CSVLoader.GetFieldDataByEnemyIndex(path, 1);
            #endregion
            
            // Pro
            var filePath = Application.dataPath + CSVSaver.SAVE_DIRECTORY + "/" + PRO_CSV_FILE;
            var fieldListPro = CSVLoader.GetFieldDataByEnemyIndex(filePath, 1);
            var maxPro = getMax(fieldListPro);

            // Player
            List<string[]> fieldListPlayer = null;
            if (playerDataMode == PlayerDataMode.NO_FEEDBACK)
            {
                fieldListPlayer = CSVLoader.GetFieldDataByEnemyIndex(CSVSaver.filePath_Update1, _enemyIndex);
            }
            else
            {
                fieldListPlayer = CSVLoader.GetFieldDataByEnemyIndex(CSVSaver.filePath_Update2, _enemyIndex);
            }
            var maxPlayer = getMax(fieldListPlayer);

            var max = new Vector2(Mathf.Max(maxPro.x, maxPlayer.x), Mathf.Max(maxPro.y, maxPlayer.y));

            // Pro
            var dataPro = new List<Vector2>();
            foreach (var item in fieldListPro)
            {
                dataPro.Add(new Vector2(float.Parse(item[1]) / max.x, float.Parse(item[11]) / max.y));
            }
            
            // Player
            var dataPlayer = new List<Vector2>();
            foreach (var item in fieldListPlayer)
            {
                dataPlayer.Add(new Vector2(float.Parse(item[1]) / max.x, float.Parse(item[11]) / max.y));
            }


            _graph1.Draw(dataPlayer.ToArray());
            // _graph1.Draw(GaussianGraph.Generate(100, 0.7f, 0.8f));
            // _graph1.Draw(new Vector2[] {
            //     new Vector2(0.0f, 0.0f),
            //     new Vector2(1.0f, 1.0f)
            // });

            _graph2.Draw(dataPro.ToArray());
            // _graph2.Draw(GaussianGraph.Generate(100, 0.3f, 0.5f));
            // _graph2.Draw(new Vector2[] {
            //     new Vector2(0.0f, 0.0f),
            //     new Vector2(0.5f, 1.0f),
            //     new Vector2(1.0f, 0.0f)
            // });


            // Player Cognition
            var dataPlayer_Cognition = new List<Vector2>();
            for (var i = 0; i < fieldListPlayer.Count; i++)
            {
                var item = fieldListPlayer[i];
                if (item[12] == "1")
                {
                    dataPlayer_Cognition.Add(dataPlayer[i]);
                    if (cognitionTimePlayer == 0.0f)
                    {
                        cognitionTimePlayer = float.Parse(item[1]);
                    }
                }
            }
            _point1.DrawCognition(dataPlayer_Cognition.ToArray());

            // Player OnTarget
            var dataPlayer_OnTarget = new List<Vector2>();
            for (var i = 0; i < fieldListPlayer.Count; i++)
            {
                var item = fieldListPlayer[i];
                if (item[17] != "0" && item[17] != "")
                {
                    dataPlayer_OnTarget.Add(dataPlayer[i]);
                }
            }
            _point1.DrawOnTarget(dataPlayer_OnTarget.ToArray());

            // Player Clicked
            var dataPlayer_Clicked = new List<Vector2>();
            for (var i = 0; i < fieldListPlayer.Count; i++)
            {
                var item = fieldListPlayer[i];
                if (item[20] != "0" && item[20] != "")
                {
                    dataPlayer_Clicked.Add(dataPlayer[i]);
                    if (clickedTimePlayer == 0.0f)
                    {
                        clickedTimePlayer = float.Parse(item[1]);
                    }
                }
            }
            _point1.DrawClicked(dataPlayer_Clicked.ToArray());


            // Pro Cognition
            var dataPro_Cognition = new List<Vector2>();
            for (var i = 0; i < fieldListPro.Count; i++)
            {
                var item = fieldListPro[i];
                if (item[12] == "1")
                {
                    dataPro_Cognition.Add(dataPro[i]);
                    if (cognitionTimePro == 0.0f)
                    {
                        cognitionTimePro = float.Parse(item[1]);
                    }
                }
            }
            _point2.DrawCognition(dataPro_Cognition.ToArray());

            // Pro OnTarget
            var dataPro_OnTarget = new List<Vector2>();
            for (var i = 0; i < fieldListPro.Count; i++)
            {
                var item = fieldListPro[i];
                if (item[17] != "0" && item[17] != "")
                {
                    dataPro_OnTarget.Add(dataPro[i]);
                }
            }
            _point2.DrawOnTarget(dataPro_OnTarget.ToArray());

            // Pro Clicked
            var dataPro_Clicked = new List<Vector2>();
            for (var i = 0; i < fieldListPro.Count; i++)
            {
                var item = fieldListPro[i];
                if (item[20] != "0" && item[20] != "")
                {
                    dataPro_Clicked.Add(dataPro[i]);
                    if (clickedTimePro == 0.0f)
                    {
                        clickedTimePro = float.Parse(item[1]);
                    }
                }
            }
            _point2.DrawClicked(dataPro_Clicked.ToArray());


            // GAP
            printGapTime();
        }
        private Vector2 getMax(List<string[]> _fieldList)
        {
            var max = Vector2.zero;
            for (var i = 0; i < _fieldList.Count; i++)
            {
                if (i == _fieldList.Count - 1)
                {
                    max.x = float.Parse(_fieldList[i][1]);
                }
                max.y = Mathf.Max(max.y, float.Parse(_fieldList[i][11]));
            }
            return max;
        }
        private void printGapTime(bool _isCognition = true)
        {
            _timeText.text = "";

            var gap = 0.0f;
            if (_isCognition)
            {
                gap = cognitionTimePlayer - cognitionTimePro;
                _timeLabel.sprite = _timeLabelSprites[0];
            }
            else
            {
                gap = clickedTimePlayer - clickedTimePro;
                _timeLabel.sprite = _timeLabelSprites[1];
            }

            if (gap >= 0.0f)
            {
                _timeText.text += "+";
            }
            _timeText.text += $"{gap.ToString("f2")}";
        }
        #endregion

        #region UIGameSceneBehaviour
        override public void Show(GameScene _scene)
        {
            base.Show(_scene);
            if (_scene == GameScene.Analytics)
            {
                drawGraph();
            }
        }
        #endregion

        #region Public Methods
        #endregion
    }

}
