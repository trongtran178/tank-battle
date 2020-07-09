using System;
using System.Collections;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts.SaveSystem {

    [System.Serializable]
    public class EnemyFactoryData
    {
        private float currentHealth;
        private float generateEnemyTime = 10.0f;
        private bool isBurn = false;
        private bool flag = false;
        private bool isActive;
        private ArrayList enemiesData;
        //private GameObject[] effectBurnArray;
        //private System.Random random = new System.Random();


        public EnemyFactoryData(EnemyFactory enemyFactory)
        {
            CurrentHealth = enemyFactory.GetCurrentHealth();
            GenerateEnemyTime = enemyFactory.GetGenerateEnemyTime();
            IsBurn = enemyFactory.IsBurn();
            Flag = enemyFactory.GetFlag();
            IsActive = enemyFactory.GetSelf().activeSelf;
            EnemiesData = new ArrayList();
            ArrayList enemies = enemyFactory.getEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                GameObject enemyObject = (GameObject) enemies[i];
                Enemies.Enemy enemy = enemyObject.GetComponentInChildren<Enemies.Enemy>();

                EnemyData enemyData = new EnemyData(enemy.GetEnemyType(), enemy.GetCurrentHealth(), enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z); ;
                EnemiesData.Add(enemyData);
            }
        }

        public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
        public float GenerateEnemyTime { get => generateEnemyTime; set => generateEnemyTime = value; }
        public bool IsBurn { get => isBurn; set => isBurn = value; }
        public bool Flag { get => flag; set => flag = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public ArrayList EnemiesData { get => enemiesData; set => enemiesData = value; }
    }
}
