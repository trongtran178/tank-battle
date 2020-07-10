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
        public static void SaveGameFactory(EnemyFactory enemyFactory)
        {

            BinaryFormatter formatter = new BinaryFormatter();

            string enemiesPath = Application.persistentDataPath + "/enemies.fun";
            string alliesPath = Application.persistentDataPath + "/allies.fun";
            string playerPath = Application.persistentDataPath + "/player.fun";

            FileStream enemiesStream = new FileStream(enemiesPath, FileMode.Create);
            FileStream alliesStream = new FileStream(alliesPath, FileMode.Create);
            FileStream playerStream = new FileStream(playerPath, FileMode.Create);

            EnemyFactoryData enemyFactoryData = new EnemyFactoryData(enemyFactory);

            GameObject[] alliesObjects = GameObject.FindGameObjectsWithTag("allies");

            
            ArrayList alliesObjectsSerializable = new ArrayList();

            foreach(GameObject alliesObject in alliesObjects)
            {
                if (alliesObject.GetComponentInChildren<Dogcollider>() != null)
                {
                    Dogcollider dogCollider = alliesObject.GetComponentInChildren<Dogcollider>();
                    float currentHealth = dogCollider.health;

                    AlliesObjectData alliesObjectData = new AlliesObjectData(AlliesType.DOG,
                                                            currentHealth,
                                                            alliesObject.transform.position.x,
                                                            alliesObject.transform.position.y,
                                                            alliesObject.transform.position.z);
                    alliesObjectsSerializable.Add(alliesObjectData);
                }
                else if (alliesObject.GetComponentInChildren<PlaneCollider>() != null)
                {
                    PlaneCollider planCollider = alliesObject.GetComponentInChildren<PlaneCollider>();
                    float currentHealth = planCollider.PlaneHealth;
                    AlliesObjectData alliesObjectData = new AlliesObjectData(AlliesType.PLANE,
                                                            currentHealth,
                                                            alliesObject.transform.position.x,
                                                            alliesObject.transform.position.y,
                                                            alliesObject.transform.position.z);
                    alliesObjectsSerializable.Add(alliesObjectData);
                }

                else if (alliesObject.GetComponentInChildren<EnemyTu>() != null)
                {
                    EnemyTu enemyTu = alliesObject.GetComponentInChildren<EnemyTu>();
                    float currentHealth = enemyTu.health;
                    AlliesObjectData alliesObjectData = new AlliesObjectData(AlliesType.TANK,
                                                            currentHealth,
                                                            alliesObject.transform.position.x,
                                                            alliesObject.transform.position.y,
                                                            alliesObject.transform.position.z);
                    alliesObjectsSerializable.Add(alliesObjectData);
                    Debug.Log("69 - save tank allies");

                }
            }


            formatter.Serialize(enemiesStream, enemyFactoryData);
            formatter.Serialize(alliesStream, alliesObjectsSerializable);
            //formatter.Serialize(playerStream, enemyFactoryData);

            enemiesStream.Close();
            alliesStream.Close();
            //playerStream.Close();

        }

        public static EnemyFactoryData LoadEnemyFactory()
        {
           
            string enemiesPath = Application.persistentDataPath + "/enemies.fun";
            //string alliesPath = Application.persistentDataPath + "/allies.fun";

            if (File.Exists(enemiesPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(enemiesPath, FileMode.Open);
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

        public static ArrayList LoadAllies()
        {
            string alliesPath = Application.persistentDataPath + "/allies.fun";

            if (File.Exists(alliesPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(alliesPath, FileMode.Open);
                ArrayList alliesData = formatter.Deserialize(stream) as ArrayList;


                stream.Close();
                return alliesData;
            }
            else
            {
                Debug.Log("Error: Save file not found!");
                return null;
            }
        }

        //public static EnemyFactoryData LoadPlayer()
        //{


        //    string playerPath = Application.persistentDataPath + "/player.fun";
        //    if (File.Exists(playerPath))
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        FileStream stream = new FileStream(playerPath, FileMode.Open);
        //        EnemyFactoryData enemyFactoryData = formatter.Deserialize(stream) as EnemyFactoryData;


        //        stream.Close();
        //        return enemyFactoryData;
        //    }
        //    else
        //    {
        //        Debug.Log("Error: Save file not found!");
        //        return null;
        //    }
        //}

    }
}
