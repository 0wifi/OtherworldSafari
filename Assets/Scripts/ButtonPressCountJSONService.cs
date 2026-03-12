using NaughtyAttributes;
using System;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class ButtonPressCountJSONService : MonoBehaviour
{
    public static ButtonPressCountData pressCounts = new ButtonPressCountData();

    public static void SaveLastRunCounts()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "ButtonPressCounts");
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath); //create directory

        DateTime localTime = DateTime.Now;
        string formattedTime = localTime.ToString("MM-dd-yyyy_hh-mm-ss_tt");
        string fileName = $"{formattedTime}.json";

        string path = Path.Combine(dirPath, fileName);

        string json = JsonUtility.ToJson(pressCounts, true);
        File.WriteAllText(path, json);
        Debug.Log("Button press counts saved to " + path);
    }
}

public class ButtonPressCountData
{
    public int TOP_LEFT_COUNT = 0;
    public int TOP_MIDDLE_COUNT = 0;
    public int TOP_RIGHT_COUNT = 0;

    public int MIDDLE_LEFT_COUNT = 0;
    public int CENTER_COUNT = 0;
    public int MIDDLE_RIGHT_COUNT = 0;

    public int BOTTOM_LEFT_COUNT = 0;
    public int BOTTOM_MIDDLE_COUNT = 0;
    public int BOTTOM_RIGHT_COUNT = 0;
}
