using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using jp.co.jetman.common.data;
using UnityEditor;

namespace jp.co.jetman.common
{
    public class CSVSaver
    {
        #region Update
        static private List<DataUpdate> dataUpdates = new List<DataUpdate>();
        static private DataUpdate cacheUpdate = null;
        static private double spawnTime = 0.0f;
        static public double durationTime  // Target出現タイミングを 0 とした時間
        {
            get
            {
                return Time.realtimeSinceStartupAsDouble - spawnTime;
            }
        }
        static public string filePath_Update1 = "";
        static public string filePath_Update2 = "";

            #region Private
            static private void setCacheUpdate()
            {
                if (cacheUpdate == null)
                {
                    cacheUpdate = new DataUpdate() {
                        time = TimerBehaviour.instance.currentTime,
                        durationTime = durationTime
                    };
                }
            }
            static private void saveDataUpdateToFile(string _filePath)
            {
                using (StreamWriter streamWriter = new StreamWriter(_filePath))
                {
                    // Header
                    var header = "";
                    header += "Time";
                    header += ",DurationTime";

                    header += ",Center of Cross Hair.x";
                    header += ",CameraRotation.x";
                    header += ",Center of Cross Hair.y(m)";
                    header += ",Center of Crosshair(degree)";
                    header += ",CameraRotation.y";

                    header += ",mousepos.x";
                    header += ",mousepos.y";

                    header += ",Speed.x";
                    header += ",Speed.y";
                    header += ",Speed(m/s)";

                    header += ",Starting to move";

                    header += ",Target";
                    header += ",Spawn Position.x";
                    header += ",Spawn Position.y";
                    header += ",Spawn Position.z";

                    header += ",On Target";
                    header += ",Leave Target";
                    header += ",Shot";
                    header += ",Destroy of Target";

                    header += ",Hap1";
                    header += ",Hap2";

                    header += ",Ray Distance";
                    streamWriter.WriteLine(header);
                    // Data Row
                    foreach (DataUpdate d in dataUpdates)
                    {
                        var data = "";
                        data += $"{d.time.ToString("f2")}";// Fixed Timestep = 0.01
                        data += $",{d.durationTime.ToString("f3")}";

                        data += $",{d.reticlePosition.x}";
                        data += $",{d.cameraRotation.x}";
                        data += $",{d.reticlePosition.y}";
                        data += $",{d.reticlePositionDegree}";
                        data += $",{d.cameraRotation.y}";

                        data += $",{d.mousepos.x}";
                        data += $",{d.mousepos.y}";

                        data += $",{d.speedVector.x}";
                        data += $",{d.speedVector.y}";
                        data += $",{d.speed}";

                        data += $",{d.startingToMove}";

                        data += $",{d.targetSpawned}";
                        data += $",{d.spawnPosition.x}";
                        data += $",{d.spawnPosition.y}";
                        data += $",{d.spawnPosition.z}";

                        data += $",{d.onTarget}";
                        data += $",{d.targetLeaved}";
                        data += $",{d.shot}";
                        data += $",{d.targetDestroyed}";

                        data += $",{d.hap1}";
                        data += $",{d.hap2}";

                        data += $",{d.rayDistance}";
                        streamWriter.WriteLine(data);
                    }
                }
            }
            #endregion
            #region Public
            static public void DataUpdate_Clear()
            {
                dataUpdates.Clear();
            }
            static public void DataUpdate_SetSpawnInfo(int _id, Vector3 _position)
            {
                spawnTime = Time.realtimeSinceStartupAsDouble;

                setCacheUpdate();
                cacheUpdate.time = TimerBehaviour.instance.currentTime;
                cacheUpdate.durationTime = durationTime;
                cacheUpdate.targetSpawned = _id;
                cacheUpdate.spawnPosition = _position;
            }
            static public void DataUpdate_SetReticlePosition(Vector2 _position, float _degree)
            {
                setCacheUpdate();
                cacheUpdate.reticlePosition = _position;
                cacheUpdate.reticlePositionDegree = _degree;
            }
            static public void DataUpdate_SetCameraRotation(Vector2 _angles)
            {
                setCacheUpdate();
                // CameraRotation
                cacheUpdate.cameraRotation = _angles;

                // mousepos
                cacheUpdate.mousepos = new Vector2(
                    (3.01f * _angles.x + 180.36f) / 1000f,
                    (-3.53f * _angles.y + 114.28f) / 1000f
                );

                // Speed
                if (dataUpdates.Any() && dataUpdates.Count > 1)
                {
                    var lastItem = dataUpdates.Last();
                    var deltaTime = cacheUpdate.time - lastItem.time;
                    if (deltaTime > 0.0f)
                    {
                        cacheUpdate.speedVector.x = (cacheUpdate.mousepos.x - lastItem.mousepos.x) / deltaTime;
                        cacheUpdate.speedVector.y = (cacheUpdate.mousepos.y - lastItem.mousepos.y) / deltaTime;
                        cacheUpdate.speed = Mathf.Sqrt(cacheUpdate.speedVector.x * cacheUpdate.speedVector.x + cacheUpdate.speedVector.y * cacheUpdate.speedVector.y);
                    }
                }
            }
            static public void DataUpdate_SetBlurId(int _id)
            {
                setCacheUpdate();
                cacheUpdate.targetLeaved = _id;
            }
            static public void DataUpdate_SetFocusId(int _id)
            {
                setCacheUpdate();
                cacheUpdate.onTarget = _id;
            }
            static public void DataUpdate_SetShot()
            {
                setCacheUpdate();
                cacheUpdate.shot++;
            }
            static public void DataUpdate_SetDestroyedId(int _id)
            {
                setCacheUpdate();
                cacheUpdate.targetDestroyed = _id;
            }
            static public void DataUpdate_SetStartingToMove()
            {
                setCacheUpdate();
                cacheUpdate.startingToMove = 1;
            }
            static public void DataUpdate_SetRayDistance(float _distance)
            {
                setCacheUpdate();
                cacheUpdate.rayDistance = _distance;
            }
            static public void DataUpdate_Record()
            {
                // Debug.Log("Record DataUpdate");
                setCacheUpdate();

                dataUpdates.Add(cacheUpdate);
                cacheUpdate = null;
            }
            static public void DataUpdate_Save()
            {
                DataUpdate_Record();
                var filename = $"update_{MainBehaviour.instance.gameCounter.ToString("000")}.csv";
                var filePath = savePath + "/" + filename;
                if (MainBehaviour.instance.gameCounter == 1)
                {
                    filePath_Update1 = filePath;
                    Debug.Log(filePath_Update1);
                }
                else if (MainBehaviour.instance.gameCounter == 2)
                {
                    filePath_Update2 = filePath;
                    Debug.Log(filePath_Update2);
                }
                saveDataUpdateToFile(filePath);
            }
            #endregion
        #endregion

        #region Result
        static private List<DataResult> dataResults = new List<DataResult>();

            #region Private
            static private void saveDataResultToFile(string _filename)
            {
                var filePath = savePath + "/" + _filename;
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    // Header
                    var header = "";
                    header += "TargetName";
                    header += ",Time";
                    header += ",Reaction";
                    header += ",Time from target appearance to click";
                    header += ",Distance";
                    header += ",Target Position.x";
                    header += ",Target Position.y";
                    header += ",Target Position.z";
                    header += ",Starting To Move";
                    streamWriter.WriteLine(header);
                    // Data Row
                    foreach (DataResult d in dataResults)
                    {
                        var data = "";
                        data += $"{d.targetName}";
                        data += $",{d.time.ToString("f2")}";
                        data += $",{d.timeReaction}";
                        data += $",{d.timeLife}";
                        data += $",{d.distance}";
                        data += $",{d.position.x}";
                        data += $",{d.position.y}";
                        data += $",{d.position.z}";
                        data += $",{d.startingToMove}";
                        streamWriter.WriteLine(data);
                    }
                }
            }
            #endregion

            #region Public
            static public void DataResult_Clear()
            {
                dataResults.Clear();
            }
            static public void DataResult_Record(DataResult _result)
            {
                Debug.Log("Result DataUpdate");
                dataResults.Add(_result);
            }
            static public void DataResult_Save()
            {
                saveDataResultToFile($"result_{MainBehaviour.instance.gameCounter.ToString("000")}.csv");
            }
            #endregion
        #endregion

        static public readonly string SAVE_DIRECTORY = "/csv";
        static private string _savePath = "";
        static public string savePath
        {
            get
            {
                if (_savePath == "")
                {
                    var path = Application.dataPath + SAVE_DIRECTORY;
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = path + "/" + CSVSaver.getDateString() + "_" + MainBehaviour.instance.GetPlayer().ID_TEST_SUBJECT;
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);
                    }
                    _savePath = path;
                }
                return _savePath;
            }
        }
        static public void ResetSavePath()
        {
            _savePath = "";
        }


        #region Private
        static private string getDateString()
        {
            var now = System.DateTime.Now;
            return $"{now.Year}-{now.Month.ToString("00")}-{now.Day.ToString("00")}_{now.Hour.ToString("00")}-{now.Minute.ToString("00")}-{now.Second.ToString("00")}";
        }
        #endregion

    }
}