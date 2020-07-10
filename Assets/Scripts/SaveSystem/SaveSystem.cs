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
        private static string enemiesPath = Application.persistentDataPath + "/enemies.fun";
        private static string alliesPath = Application.persistentDataPath + "/allies.fun";
        private static string initAlliesTimePath = Application.persistentDataPath + "/init_allies_time.fun";
        private static string initBulletTimePath = Application.persistentDataPath + "/init_bullet_time.fun";
        private static string playerPath = Application.persistentDataPath + "/player.fun";

        public static void SaveGameFactory(EnemyFactory enemyFactory, GameObject manageRecoveryTime)
        {

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream enemiesStream = new FileStream(enemiesPath, FileMode.Create);
            FileStream alliesStream = new FileStream(alliesPath, FileMode.Create);
            FileStream initAlliesTimeStream = new FileStream(initAlliesTimePath, FileMode.Create);
            // FileStream initBulletTimeStream = new FileStream(initBulletTimePath, FileMode.Create);
            // FileStream playerStream = new FileStream(playerPath, FileMode.Create);

            EnemyFactoryData enemyFactoryData = new EnemyFactoryData(enemyFactory);

            GameObject[] alliesObjects = GameObject.FindGameObjectsWithTag("allies");



            ArrayList alliesObjectsSerializable = new ArrayList();
            ArrayList initAlliesTimesSerializable = new ArrayList();

            foreach (GameObject alliesObject in alliesObjects)
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


            // ?
            ManaArmy[] scripts = manageRecoveryTime.GetComponentsInChildren<ManaArmy>();

            for(int i = 0; i < scripts.Length; i++)
            {
                if(scripts[i].alliesObject.GetComponentInChildren<Dogcollider>() != null)
                {
                    // DOG
                    InitAlliesTimeData initDogTimeData = new InitAlliesTimeData(AlliesType.DOG, scripts[i].manaArmy);
                    initAlliesTimesSerializable.Add(initDogTimeData);
                }
                if (scripts[i].alliesObject.GetComponentInChildren<PlaneCollider>() != null)
                {
                    // PLANE
                    InitAlliesTimeData initPlaneTimeData = new InitAlliesTimeData(AlliesType.PLANE, scripts[i].manaArmy);
                    initAlliesTimesSerializable.Add(initPlaneTimeData);

                }
                if (scripts[i].alliesObject.GetComponentInChildren<EnemyTu>() != null)
                {
                    // TANK
                    InitAlliesTimeData initTankTimeData = new InitAlliesTimeData(AlliesType.TANK, scripts[i].manaArmy);
                    initAlliesTimesSerializable.Add(initTankTimeData);
                }
            }
                



            formatter.Serialize(enemiesStream, enemyFactoryData);
            formatter.Serialize(alliesStream, alliesObjectsSerializable);
            formatter.Serialize(initAlliesTimeStream, initAlliesTimesSerializable);

            enemiesStream.Close();
            alliesStream.Close();
            initAlliesTimeStream.Close();
            //playerStream.Close();

        }




        public static EnemyFactoryData LoadEnemyFactory()
        {
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

        public static ArrayList LoadInitAlliesTimes()
        {
            if (File.Exists(initAlliesTimePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(initAlliesTimePath, FileMode.Open);
                ArrayList initAlliesTimes = formatter.Deserialize(stream) as ArrayList;

                stream.Close();
                return initAlliesTimes;
            }
            else
            {
                Debug.Log("Error: Save file not found!");
                return null;
            }
        }

        public static ArrayList LoadInitBulletTimes()
        {
            return null;
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
