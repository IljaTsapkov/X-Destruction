using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileID)
    {

        // if the profileId is null, return right away
        if(profileID == null)
        {
            return null;
        }

        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                
                Debug.LogError("Error when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileID)
    {
        if(profileID == null)
        {
            return;
        }
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileID)
    {

        // if the profileId is null, return right away
        if (profileID == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        try
        {
            // ensure the data file exists at this path before deleting the directory
            if ( File.Exists(fullPath))
            {
                // delete the profile folder and everything within it
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to delete profile data for profileID: "
                + profileID + " at path: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        // loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in  dirInfos)
        {
            string profileID = dirInfo.Name;

            // check if the data file exists
            // if it doesn't, then this folder isn't a profile and should be skipped
            string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
            if(!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles, because it does not contain data:"
                + profileID);
                continue; 
            }

            // load the game data for this profile and put it in the dictionary
            GameData profileData = Load(profileID);

             // ensure the profile data isn't null,
            // because if it is then something went wrong and we should let ourselves know
            if(profileData != null) 
            {
                profileDictionary.Add(profileID, profileData);
            }
            else 
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileID: " +profileID);
            }
        }

        return profileDictionary;
    }

    //TODO: Continue button fuctionality
    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            GameData gameData = pair.Value;

            // skip this entry if the gamedata is null
            if(gameData == null)
            {
                continue;
            }

            // if this is the first data we've come across that exists, it's the most recent so far
            if(mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            // otherwise, compare to see which date is the most recent
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileID].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                // the greatest DateTime value is the most recent
                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileID = profileID;
                }
            }
        }
        return mostRecentProfileID;
    }
}