using System;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace BankSystem.BLL.Helpers
{
    public class RegistryHelper
    {
        ////Path Of Local Registry :-
        //public static string KeyPath = @"HKEY_CURRENT_USER\Software\DVLD";
        ////public static clsUser CurrentUser;

        //public static string GetDataFromLocalRegistry(string ValueName)
        //{
        //    //string KeyPath = @"HKEY_CURRENT_USER\Software\DVLD";
        //    string ValueData = "";

        //    try
        //    {
        //        ValueData = (string)Registry.GetValue(KeyPath, ValueName, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("An Error Occurred : " + ex.Message);
        //        return null;
        //    }
        //    return ValueData;
        //}

        //public static bool GetUserNameANDPassWord(ref string UserName, ref string PassWord)
        //{
        //    UserName = GetDataFromLocalRegistry("UserName");
        //    PassWord = GetDataFromLocalRegistry("PassWord");

        //    return (UserName != null && PassWord != null) ? true : false;
        //}

        //public static bool DeleteSubKeyFromLocalRegistry(string ValueName)
        //{
        //    string KeyPath = @"Software\DVLD";

        //    try
        //    {
        //        // Open the registry key in read/write mode with explicit registry view
        //        using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
        //        {
        //            using (RegistryKey Subkey = baseKey.OpenSubKey(KeyPath, true))
        //            {
        //                if (Subkey != null)
        //                {
        //                    // Delete the specified value
        //                    if (Subkey.GetValue(ValueName) != null)
        //                    {
        //                        Subkey.DeleteValue(ValueName);
        //                    }
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        //MessageBox.Show("UnauthorizedAccessException: Run the program with administrative privileges.");
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("An Error Occurred : " + ex.Message);
        //    }
        //    return false;
        //}

        //public static bool SaveDataToLocalRegistry(string ValueName, string ValueData)
        //{
        //    //string KeyPath = @"HKEY_CURRENT_USER\Software\DVLD";
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(ValueData))
        //        {
        //            return DeleteSubKeyFromLocalRegistry(ValueName);
        //        }

        //        Registry.SetValue(KeyPath, ValueName, ValueData, RegistryValueKind.String);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("An Error Occurred : " + ex.Message);
        //        return false;
        //    }
        //}

        //public static bool SaveUserNameANDPassWord(string UserName, string PassWord)
        //{
        //    return SaveDataToLocalRegistry("UserName", UserName) && SaveDataToLocalRegistry("PassWord", PassWord);
        //}
    }
}