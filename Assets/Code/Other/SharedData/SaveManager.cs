using System;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessClicker.Data
{
    public class SaveManager
    {
        private const string BUSINESS_SAVE_KEY = "BusinessSaveData";
        private const string BALANCE_SAVE_KEY = "Balance";

        public void SaveBusinesses(Dictionary<int, BusinessSaveData> businessesData)
        {
            var serializableDict = new BusinessSaveDictionary();
            
            foreach (var kvp in businessesData)
            {
                serializableDict.Keys.Add(kvp.Key);
                serializableDict.Values.Add(kvp.Value);
            }

            string json = JsonUtility.ToJson(serializableDict);
            PlayerPrefs.SetString(BUSINESS_SAVE_KEY, json);
            PlayerPrefs.Save();
        }

        public Dictionary<int, BusinessSaveData> LoadBusinesses()
        {
            if (!PlayerPrefs.HasKey(BUSINESS_SAVE_KEY))
                return null;

            string json = PlayerPrefs.GetString(BUSINESS_SAVE_KEY);
            try
            {
                var serializableDict = JsonUtility.FromJson<BusinessSaveDictionary>(json);
                var result = new Dictionary<int, BusinessSaveData>();
                
                for (int i = 0; i < serializableDict.Keys.Count; i++)
                {
                    result[serializableDict.Keys[i]] = serializableDict.Values[i];
                }
                
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load save data: {e.Message}");
                return new Dictionary<int, BusinessSaveData>();
            }
        }

        public void SaveBalance(float amount)
        {
            PlayerPrefs.SetFloat(BALANCE_SAVE_KEY, amount);
        }
        
        public float LoadBalance()
        {
            return PlayerPrefs.GetFloat(BALANCE_SAVE_KEY);
        }

        [Serializable]
        private class BusinessSaveDictionary
        {
            public List<int> Keys = new List<int>();
            public List<BusinessSaveData> Values = new List<BusinessSaveData>();
        }
    }
    
    [Serializable]
    public class BusinessSaveData
    {
        public int Lvl;
        public bool Upgrade1Status;
        public bool Upgrade2Status;
        public float ProgressTime;
    }
}