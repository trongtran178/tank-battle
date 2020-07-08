using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Enemies;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Assets.Scripts.SaveSystem
{
    public static class SaveSystem
    {
        public static void SaveEnemiesFactory(EnemyFactory enemyFactory)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.fun";
            FileStream stream = new FileStream(path, FileMode.Create);

            EnemyFactoryData enemyFactoryData = new EnemyFactoryData(enemyFactory);
            Debug.Log(enemyFactoryData.CurrentHealth);
            Debug.Log(enemyFactoryData.IsBurn);
            Debug.Log(enemyFactoryData.GenerateEnemyTime);
            Debug.Log(enemyFactoryData.Flag);
            formatter.Serialize(stream, enemyFactoryData);
            stream.Close();

        }

        public static EnemyFactoryData LoadEnemyFactory()
        {
            string path = Application.persistentDataPath + "/player.fun";
            if(File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                FileStream stream = new FileStream(path, FileMode.Open);

                EnemyFactoryData enemyFactoryData = formatter.Deserialize(stream) as EnemyFactoryData;
                stream.Close();
                return enemyFactoryData;

            }
            else
            {
                Debug.Log("Error: Save file not found!");
                return null;
            }
        }

    }
}
