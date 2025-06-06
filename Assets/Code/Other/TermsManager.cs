using BusinessClicker.SO;
using UnityEngine;

namespace BusinessClicker.Data
{
    public class TermsManager
    {
        private const string TABLE_NAME = "TermsList";

        public TermsListSO TermsList { get; }

        public TermsManager()
        {
            TermsList = Resources.Load<TermsListSO>(TABLE_NAME);
        }
    }
}