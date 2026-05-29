using UnityEngine;

public static class CommonLib
{
    public static bool DirectoryExists(string path)
    {
        bool result = false;

        result = System.IO.Directory.Exists(path);
        if (!result)
        {
            Debug.LogError($"Directory does not exist: {path}");
        }

        return result;
    }
    
    public static bool FileExists(string path)
    {
        bool result = false;

        result = System.IO.File.Exists(path);
        if (!result)
        {
            Debug.LogError($"File does not exist: {path}");
        }

        return result;
    }
    public static int GetWinVersion()
    {
        int version = 0;
        string osVersion = System.Environment.OSVersion.Version.ToString();
        if (osVersion.StartsWith("10.0"))
        {
            version = 10;
        }
        else if (osVersion.StartsWith("6.3"))
        {
            version = 8;
        }
        else if (osVersion.StartsWith("6.2"))
        {
            version = 8;
        }
        else if (osVersion.StartsWith("6.1"))
        {
            version = 7;
        }
        else if (osVersion.StartsWith("6.0"))
        {
            version = 6;
        }
        
        return version*100;
    }
}