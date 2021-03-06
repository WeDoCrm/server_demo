﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace WDMsgServer
{
    class CommonUtil
    {
        public static bool IsAppInstalled(string p_machineName, string p_name)
        {
            string keyName;

            // search in: CurrentUser
            keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            if (ExistsInRemoteSubKey(p_machineName, RegistryHive.CurrentUser, keyName, "DisplayName", p_name) == true)
            {
                return true;
            }

            // search in: LocalMachine_32
            keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            if (ExistsInRemoteSubKey(p_machineName, RegistryHive.LocalMachine, keyName, "DisplayName", p_name) == true)
            {
                return true;
            }

            // search in: LocalMachine_64
            keyName = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            if (ExistsInRemoteSubKey(p_machineName, RegistryHive.LocalMachine, keyName, "DisplayName", p_name) == true)
            {
                return true;
            }

            return false;
        }
        private static bool ExistsInRemoteSubKey(string p_machineName, RegistryHive p_hive, string p_subKeyName, string p_attributeName, string p_name)
        {
            RegistryKey subkey;
            string displayName;

            using (RegistryKey regHive = RegistryKey.OpenRemoteBaseKey(p_hive, p_machineName))
            {
                using (RegistryKey regKey = regHive.OpenSubKey(p_subKeyName))
                {
                    if (regKey != null)
                    {
                        foreach (string kn in regKey.GetSubKeyNames())
                        {
                            using (subkey = regKey.OpenSubKey(kn))
                            {
                                displayName = subkey.GetValue(p_attributeName) as string;
                                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true) // key found!
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
