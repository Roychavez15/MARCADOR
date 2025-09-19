namespace MARCADORMANUAL
{
    using System;
    using System.Runtime.InteropServices;

    public class cIniArray
    {
        private string sBuffer;

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileSection(string lpAppName, string lpReturnedString, int nSize, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileSectionNames(string lpszReturnBuffer, int nSize, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileString(string lpAppName, int lpKeyName, string lpDefault, string lpReturnedString, int nSize, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, int nSize, string lpFileName);
        public void IniDeleteKey(string sIniFile, string sSection, string sKey)
        {
            if (sKey == "")
            {
                WritePrivateProfileString(sSection, 0, 0, sIniFile);
            }
            else
            {
                WritePrivateProfileString(sSection, sKey, 0, sIniFile);
            }
        }

        public void IniDeleteSection(string sIniFile, string sSection)
        {
            WritePrivateProfileString(sSection, 0, 0, sIniFile);
        }

        public string IniGet(string sFileName, string sSection, string sKeyName, string sDefault)
        {
            string lpReturnedString = new string(' ', 0xff);
            int length = GetPrivateProfileString(sSection, sKeyName, sDefault, lpReturnedString, lpReturnedString.Length, sFileName);
            if (length == 0)
            {
                return sDefault;
            }
            return lpReturnedString.Substring(0, length);
        }

        public string[] IniGetSection(string sFileName, string sSection)
        {
            string[] strArray = new string[0];
            this.sBuffer = new string('\0', 0x7fff);
            int num = GetPrivateProfileSection(sSection, this.sBuffer, this.sBuffer.Length, sFileName);
            if (num > 0)
            {
                this.sBuffer = this.sBuffer.Substring(0, num - 2).TrimEnd(new char[0]);
                char[] separator = new char[2];
                separator[1] = '=';
                strArray = this.sBuffer.Split(separator);
            }
            return strArray;
        }

        public string[] IniGetSections(string sFileName)
        {
            string[] strArray = new string[0];
            this.sBuffer = new string('\0', 0x7fff);
            int num = GetPrivateProfileSectionNames(this.sBuffer, this.sBuffer.Length, sFileName);
            if (num > 0)
            {
                this.sBuffer = this.sBuffer.Substring(0, num - 2).TrimEnd(new char[0]);
                strArray = this.sBuffer.Split(new char[1]);
            }
            return strArray;
        }

        public void IniWrite(string sFileName, string sSection, string sKeyName, string sValue)
        {
            WritePrivateProfileString(sSection, sKeyName, sValue, sFileName);
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int WritePrivateProfileString(string lpAppName, int lpKeyName, int lpString, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, int lpString, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
    }
}



