using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using jp.co.jetman.common.data;
using UnityEditor;

namespace jp.co.jetman.common
{
    public class CSVLoader
    {
        #region Update

            #region Private
            static private List<string[]> loadDataUpdateFromFile(string _filePath)
            {
                var fieldList = new List<string[]>();

                if (File.Exists(_filePath))
                {
                    var text = File.ReadAllText(_filePath);
                    string[] lines = text.Split('\n');
                    for (var i = 1; i < lines.Length; i++)
                    {
                        if (lines[i] != "")
                        {
                            fieldList.Add(lines[i].Split(','));
                        }
                    }
                }
                else
                {
                    Debug.LogError("File not found: " + _filePath);
                }

                return fieldList;
            }
            #endregion
            #region Public
            static public List<string[]> GetFieldDataByEnemyIndex(string _filePath, int _index)
            {
                var fieldList = loadDataUpdateFromFile(_filePath);

                //0:Time
                //1:DurationTime
                //2:Center of Cross Hair.x
                //3:CameraRotation.x
                //4:Center of Cross Hair.y(m)
                //5:Center of Crosshair(degree)
                //6:CameraRotation.y
                //7:mousepos.x
                //8:mousepos.y
                //9:Speed.x
                //10:Speed.y
                //11:Speed(m/s)
                //12:Starting to move
                //13:Target
                //14:Spawn Position.x
                //15:Spawn Position.y
                //16:Spawn Position.z
                //17:On Target
                //18:Leave Target
                //19:Shot
                //20:Destroy of Target
                //21:Hap1
                //22:Hap2
                //23:Ray Distance
                var results = new List<String[]>();
                if (fieldList.Any())
                {
                    var flag = false;
                    foreach (var item in fieldList)
                    {
                        if (item.Length > 0)
                        {
                            if (item[13] != "0" && item[13] != "")
                            {
                                flag = item[13] == $"{_index}";
                            }
                            if (flag)
                            {
                                results.Add(item);
                            }
                        }
                    }
                }
                return results;
            }
            #endregion
        #endregion

        #region Result
        #endregion

        #region Private
        #endregion

    }
}