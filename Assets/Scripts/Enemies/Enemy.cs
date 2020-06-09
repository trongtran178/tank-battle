﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected float health = 100.0f;
        protected float moveSpeed = 30.0f;
        protected float attackSpeed = 50.0f; 

        public abstract void UpgrageLevelCorrespondToPhase(Phase phase);
        public abstract void Instantiate();

        protected GameObject player;
        protected GameObject player_body;
        protected GameObject attackTarget;
        // Detect collision with other enemy, enemy should go through other enemy
        private void OnEnable()
        {

            player = GameObject.FindGameObjectWithTag("player");
            player_body = GameObject.FindGameObjectWithTag("player_body");

            GameObject[] otherEnemyControllers = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject enemyController in otherEnemyControllers)
            {
                Physics2D.IgnoreCollision(enemyController.GetComponent<PolygonCollider2D>(), GetComponent<PolygonCollider2D>());
            }
            
        }

        protected GameObject FindAttackTarget()
        {
            GameObject attackTarget = null;
            List<GameObject> allies = new List<GameObject>();
            GameObject playerTarget;

            // Key - value equivalent gameObject with distance between enemy
            Dictionary<GameObject, float> alliesDictionary = new Dictionary<GameObject, float>();

            playerTarget = player_body;

            if (player == null || player.activeSelf == false) return null;
            allies.Add(playerTarget);

            // get all allies
            GameObject[] alliesArray = GameObject.FindGameObjectsWithTag("allies");
            for (int i = 0; i < alliesArray.Length; i++)
            {
                allies.Add(alliesArray[i]);
            }
            float shortestAttackTargetDistance;

            shortestAttackTargetDistance = Vector2.Distance(playerTarget.transform.position, transform.position);

            foreach (GameObject alliesGameObject in allies)
            {
                float distance = Vector2.Distance(alliesGameObject.transform.position, transform.position);
                if (shortestAttackTargetDistance >= distance)
                {
                    shortestAttackTargetDistance = distance;
                }
                alliesDictionary.Add(alliesGameObject, distance);
            };

            attackTarget = alliesDictionary.FirstOrDefault(x => x.Value <= shortestAttackTargetDistance).Key;
            return attackTarget;
        }

    }
}
