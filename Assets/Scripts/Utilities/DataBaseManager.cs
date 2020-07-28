using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scrips.Utilities
{
   public class DataBaseManager: MonoBehaviour
    {
        public static void CreateAccessibleDB(string fileName)
        {
            //Where to copy the db to
            string dbDestination = Path.Combine(Application.persistentDataPath, "data");
            dbDestination = Path.Combine(dbDestination, fileName);
            //Where the db file is at
            string dbStreamingAsset = Path.Combine(Application.streamingAssetsPath, fileName);

            byte[] result;

            //Read the File from streamingAssets. Use WWW for Android
            if (dbStreamingAsset.Contains("://") || dbStreamingAsset.Contains(":///"))
            {
                WWW www = new WWW(dbStreamingAsset);

                result = www.bytes;
            }
            else
            {
                result = File.ReadAllBytes(dbStreamingAsset);
            }
            Debug.Log("Loaded db file");
            //Create Directory if it does not exist
            if (!Directory.Exists(Path.GetDirectoryName(dbDestination)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dbDestination));
            }

            //Copy the data to the persistentDataPath where the database API can freely access the file
            try

            {
                File.WriteAllBytes(dbDestination, result);
                Debug.Log("Copied db file");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to rewrite DataBase: " + e.Message);

            }
        }
        public static SqliteConnection CreateConection(string fileName)
        {
            try
            {
                //Where to copy the db to
                string dbDestination = Path.Combine(Application.persistentDataPath, "data");
                dbDestination = Path.Combine(dbDestination, fileName);


                try
                {
                    //Tell the db final location for debugging
                    Debug.Log("DB Path: " + dbDestination.Replace("/", "\\"));
                    //Add "URI=file:" to the front of the url beore using it with the Sqlite API
                    dbDestination = "URI=file:" + dbDestination;

                    //Now you can do the database operation below
                    //open db connection
                    var connection = new SqliteConnection(dbDestination);


                    Debug.Log("Success!");
                    return connection;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed: " + e.Message);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed: " + e.Message);
                return null;
            }
        }
    }
}
