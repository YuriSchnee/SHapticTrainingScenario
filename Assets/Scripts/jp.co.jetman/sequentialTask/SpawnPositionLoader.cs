using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace jp.co.jetman.sequentialTask
{
    public class SpawnPositionLoader
    {
        static public string text = "";
        static public List<Vector3> positions = new List<Vector3>();

        #region Public Methods
        static public void Load()
        {
            // text = Resources.Load<TextAsset>("SequentialTaskPosition").text;

            var filePath = Path.Combine(Application.streamingAssetsPath, "SequentialTaskPosition.csv");
            if (File.Exists(filePath))
            {
                var text = File.ReadAllText(filePath);

                string[] lines = text.Split('\n');
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');
                    if (fields.Length > 2)
                    {
                        float x = float.Parse(fields[0]);
                        float y = float.Parse(fields[1]);
                        float z = float.Parse(fields[2]);
                        positions.Add(new Vector3(x, y, z));
                    }
                }
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
            }
        }
        #endregion
    }
}