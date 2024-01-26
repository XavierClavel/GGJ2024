using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class DataBuilder<T>
{
    protected List<string> columnTitles = new List<string>();
    protected Dictionary<string, string> dictColumnToValue;

    private void Initialize(List<string> s)
    {
        columnTitles = InitializeColumnTitles(s);
    }
    
    protected static List<string> InitializeColumnTitles(List<string> s)
    {
        List<string> columnTitles = new List<string>();
        for (int i = 0; i < s.Count; i++)
        {
            columnTitles.Add(s[i].Trim());
        }
        return columnTitles;
    }
    
    protected void SetDictionary(List<string> columnTitles, List<string> s)
    {
        dictColumnToValue = new Dictionary<string, string>();

        for (int i = 0; i < s.Count; i++)
        {
            dictColumnToValue[columnTitles[i]] = s[i];
        }
    }
    
    protected void SetValue<T>(ref T variable, string key)
    {
        string value = dictColumnToValue[key];
        if (string.IsNullOrEmpty(value) || value == string.Empty) return;
        try
        {
            variable = Helpers.parseString<T>(dictColumnToValue[key]);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse value in column \"{key}\".");
        }

    }

    protected virtual string getKey(List<string> s)
    {
        if (s == null || s.Count != columnTitles.Count) return null;

        SetDictionary(columnTitles, s);

        string key = string.Empty;
        SetValue(ref key, Vault.key.Key);
        if (key == string.Empty) return null;
        else return key;
    }

    protected abstract T BuildData(List<string> s);

    public void loadText(TextAsset csv, ref Dictionary<string, T> dict, string tableName)
    {
        try
        {
            List<string> stringArray = csv.text.Split('\n').ToList();
            Initialize(stringArray[0].Split(';').ToList());
            stringArray.RemoveAt(0);

            List<string> correctedArray = new List<string>();
            string currentString = "";
            bool insideComma = false;

            foreach (string s in stringArray)
            {
                currentString += s;
                if (insideComma) insideComma = s.Count(f => f == '"') % 2 == 0;
                else insideComma = s.Count(f => f == '"') % 2 == 1;

                if (!insideComma)
                {
                    currentString = currentString.Trim();
                    correctedArray.Add(currentString);
                    currentString = "";
                }
                else currentString += "\n";
            }

            foreach (string array in correctedArray)
            {
                List<string> s = array.Split(';').ToList();
                string key = getKey(s);
                if (key == null) continue;
                T value = BuildData(s);
                if (value == null) continue;

                dict[key] = value;
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to read value in table \"{tableName}\" : {e}");
        }
    }
    
    public void loadText(TextAsset csv, ref List<T> list, string tableName)
    {
        try
        {
            List<string> stringArray = csv.text.Split('\n').ToList();
            Initialize(stringArray[0].Split(';').ToList());
            stringArray.RemoveAt(0);

            List<string> correctedArray = new List<string>();
            string currentString = "";
            bool insideComma = false;

            foreach (string s in stringArray)
            {
                currentString += s;
                if (insideComma) insideComma = s.Count(f => f == '"') % 2 == 0;
                else insideComma = s.Count(f => f == '"') % 2 == 1;

                if (!insideComma)
                {
                    currentString = currentString.Trim();
                    correctedArray.Add(currentString);
                    currentString = "";
                }
                else currentString += "\n";
            }

            foreach (string array in correctedArray)
            {
                List<string> s = array.Split(';').ToList();
                SetDictionary(columnTitles, s);
                T value = BuildData(s);
                if (value == null) continue;

                list.Add(value);
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to read value in table \"{tableName}\" : {e}");
        }
    }

}