using System;
using System.Text.RegularExpressions;

namespace Xbyte_Common_Code.QAs
{
    class QA_Checker
    {
        public bool IsContentMatch(string PastValue, string CurrentValue)
        {
            return Convert.ToString(PastValue).Trim() == Convert.ToString(CurrentValue).Trim();
        }

        public bool IsContentNumeric(string value)
        {
            Regex regex = new Regex("^[0-9.,]*$");

            if (regex.IsMatch(value) || Convert.ToString(value).Trim() == "")
                return true;
            else
                return false;
        }
    }
}
