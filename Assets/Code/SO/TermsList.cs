using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BusinessClicker.SO
{
    // Здесь должна быть таблица с переводами, но по заданию это не нужно, поэтому ограничился простым вариантом
    
    [CreateAssetMenu(fileName = "TermsTable", menuName = "ScriptableObjects/TermsTable", order = 1)]
    public class TermsListSO : SerializedScriptableObject
    {
        public string Income;
        public string Price;
        public string Purchased;
        public string Currency;

        public Dictionary<int, BusinessTerms> Businesses = new();
    }

    [Serializable]
    public class BusinessTerms
    {
        public string Name;
        public string Upgrade1;
        public string Upgrade2;
    }
}