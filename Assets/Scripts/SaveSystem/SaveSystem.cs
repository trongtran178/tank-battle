using System.Collections;
using UnityEngine;
using Assets.Scripts.Enemies;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System;

namespace Assets.Scripts.SaveSystem
{
    public static class SaveSystem
    {
        private static string enemiesPath = Application.persistentDataPath + "/enemies.fun";
        private static string alliesPath = Application.persistentDataPath + "/allies.fun";
        private static string initAlliesTimePath = Application.persistentDataPath + "/init_allies_time.fun";
        private static string initBulletTimePath = Application.persistentDataPath + "/init_bullet_time.fun";
        private static string playerPath = Application.persistentDataPath + "/player.fun";

        public static void SaveGameFactory(GameObject playerGameObject, EnemyFactory enemyFactory, GameObject manageRecoveryTime)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                FileStream enemiesStream = new FileStream(enemiesPath, FileMode.Create);
                FileStream alliesStream = new FileStream(alliesPath, FileMode.Create);
                FileStream initAlliesTimeStream = new FileStream(initAlliesTimePath, FileMode.Create);
                FileStream initBulletTimeStream = new FileStream(initBulletTimePath, FileMode.Create);
                FileStream playerStream = new FileStream(playerPath, FileMode.Create);

                EnemyFactoryData enemyFactoryData = new EnemyFactoryData(enemyFactory);

                PlayerData playerDataSerializable = new PlayerData();

                ArrayList alliesObjectsSerializable = new ArrayList();
                ArrayList initAlliesTimesSerializable = new ArrayList();
                ArrayList initBulletTimesSerializable = new ArrayList();


                GameObject[] alliesObjects = GameObject.FindGameObjectsWithTag("allies");

                foreach (GameObject alliesObject in alliesObjects)
                {
                    if (alliesObject.GetComponentInChildren<Dogcollider>() != null)
                    {
                        Dogcollider dogCollider = alliesObject.GetComponentInChildren<Dogcollider>();
                        float currentHealth = dogCollider.health;
                        bool isDizzy = alliesObject.GetComponent<dog>() ? alliesObject.GetComponent<dog>().isDizzy : false;
                        AlliesObjectData alliesObjectData = new AlliesObjectData(AlliesType.DOG,
                                                                currentHealth,
                                                                alliesObject.transform.position.x,
                                                                alliesObject.transform.position.y,
                                                                alliesObject.transform.position.z,
                                                                isDizzy);
                        alliesObjectsSerializable.Add(alliesObjectData);
                    }
                    else if (alliesObject.GetComponentInChildren<PlaneCollider>() != null)
                    {
                        PlaneCollider planCollider = alliesObject.GetComponentInChildren<PlaneCollider>();
                        float currentHealth = planCollider.PlaneHealth;
                        //bool isDizzy = alliesObject.GetComponent<PlaneControiler>().isDizzy;
                        AlliesObjectData alliesObjectData = new AlliesObjectData(AlliesType.PLANE,
                                                                currentHealth,
                                                                alliesObject.transform.position.x,
                                                                alliesObject.transform.position.y,
                                                                alliesObject.transform.position.z,
                                                                false); // no dizzy
                        alliesObjectsSerializable.Add(alliesObjectData);
                    }

                    else if (alliesObject.GetComponentInChildren<EnemyTu>() != null)
                    {
                        EnemyTu enemyTu = alliesObject.GetComponentInChildren<EnemyTu>();
                        float currentHealth = enemyTu.health;
                        bool isDizzy = enemyTu ? enemyTu.isDizzy : false;
                        AlliesObjectData alliesObjectData = new AlliesObjectData(AlliesType.TANK,
                                                                currentHealth,
                                                                alliesObject.transform.position.x,
                                                                alliesObject.transform.position.y,
                                                                alliesObject.transform.position.z,
                                                                isDizzy);
                        alliesObjectsSerializable.Add(alliesObjectData);
                    }
                }


                // SAVE RECOVERY INITIALIZE TIME OF ALLIES !
                ManaArmy[] manaArmyArray = manageRecoveryTime.GetComponentsInChildren<ManaArmy>();

                for (int i = 0; i < manaArmyArray.Length; i++)
                {
                    if (manaArmyArray[i].alliesObject.GetComponentInChildren<Dogcollider>() != null)
                    {
                        // DOG
                        InitAlliesTimeData initDogTimeData = new InitAlliesTimeData(AlliesType.DOG, manaArmyArray[i].manaArmy);
                        initAlliesTimesSerializable.Add(initDogTimeData);
                    }
                    if (manaArmyArray[i].alliesObject.GetComponentInChildren<PlaneCollider>() != null)
                    {
                        // PLANE
                        InitAlliesTimeData initPlaneTimeData = new InitAlliesTimeData(AlliesType.PLANE, manaArmyArray[i].manaArmy);
                        initAlliesTimesSerializable.Add(initPlaneTimeData);

                    }
                    if (manaArmyArray[i].alliesObject.GetComponentInChildren<EnemyTu>() != null)
                    {
                        // TANK
                        InitAlliesTimeData initTankTimeData = new InitAlliesTimeData(AlliesType.TANK, manaArmyArray[i].manaArmy);
                        initAlliesTimesSerializable.Add(initTankTimeData);
                    }
                }


                // SAVE RECOVERY TIME OF BULLET !
                ManaBullet[] manaBulletArray = manageRecoveryTime.GetComponentsInChildren<ManaBullet>();
                for (int i = 0; i < manaBulletArray.Length; i++)
                {
                    // Bullet order start from 0, 1, 2, 3, 4, 5 ...
                    InitBulletTimeData initBulletTimeData = new InitBulletTimeData(i, manaBulletArray[i].manaBullet);
                    initBulletTimesSerializable.Add(initBulletTimeData);
                }

                playerDataSerializable.CurrentLevel = SceneManager.GetActiveScene().name; // Level1, Level2, Level3
                playerDataSerializable.CurrentHealth = playerGameObject.GetComponentInChildren<TankController2>().health;
                playerDataSerializable.CurrentMana = ManaTank.manaTank;
                playerDataSerializable.PositionX = playerGameObject.transform.position.x;
                playerDataSerializable.PositionY = playerGameObject.transform.position.y;
                playerDataSerializable.PositionZ = playerGameObject.transform.position.z;

                formatter.Serialize(enemiesStream, enemyFactoryData);
                formatter.Serialize(alliesStream, alliesObjectsSerializable);
                formatter.Serialize(initAlliesTimeStream, initAlliesTimesSerializable);
                formatter.Serialize(initBulletTimeStream, initBulletTimesSerializable);
                formatter.Serialize(playerStream, playerDataSerializable);

                enemiesStream.Close();
                alliesStream.Close();
                initAlliesTimeStream.Close();
                initBulletTimeStream.Close();
                playerStream.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
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
            try
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
            catch (Exception e)
            {
                Debug.Log("Error :" + e.Message);
                return null;
            }
        }

        public static ArrayList LoadInitAlliesTimes()
        {
            try
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
            catch (Exception e)
            {
                Debug.Log("Error :" + e.Message);
                return null;
            }
        }

        public static ArrayList LoadInitBulletTimes()
        {
            try
            {
                if (File.Exists(initBulletTimePath))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(initBulletTimePath, FileMode.Open);
                    ArrayList initBulletTimes = formatter.Deserialize(stream) as ArrayList;

                    stream.Close();
                    return initBulletTimes;
                }
                else
                {
                    Debug.Log("Error: Save file not found!");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error :" + e.Message);
                return null;
            }
        }

        public static PlayerData LoadPlayer()
        {
            try
            {
                if (File.Exists(playerPath))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(playerPath, FileMode.Open);

                    PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
                    stream.Close();
                    return playerData;

                }
                else
                {
                    Debug.Log("Error: Save file not found!");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error :" + e.Message);
                return null;
            }
        }

    }
}
