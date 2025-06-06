using BusinessClicker.SO;
using UnityEngine;

namespace BusinessClicker.Data
{
    public class TermsManager
    {
        private const string LIST_NAME = "TermsList";

        public TermsListSO TermsList { get; }

        public TermsManager()
        {
            TermsList = Resources.Load<TermsListSO>(LIST_NAME);
        }
    }
}