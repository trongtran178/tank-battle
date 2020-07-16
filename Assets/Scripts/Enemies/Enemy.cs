using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemy;
using UnityEngine;
namespace Assets.Scripts.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public float maxHealth = 100.0f;
        protected float moveSpeed = 30.0f;
        protected float attackSpeed = 50.0f;

        protected GameObject attackTarget;
        protected GameObject player;

        public abstract void UpgrageLevelCorrespondToPhase(Phase phase);
        public abstract void Instantiate();
        public abstract void Death();
        public abstract void TakeDamage(float damage);
        public abstract void ReceiveHealthBumpFromBoss();
        public abstract void SetCurrentHealth(float currentHealth);
        public abstract float GetCurrentHealth();
        public abstract GameObject GetSelf();
        public abstract EnemyType GetEnemyType();
        public abstract bool IsShortRangeStrike();
        // Detect collision with other enemy, enemy should go through another enemy
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("player");

            GameObject[] otherEnemyControllers = GameObject.FindGameObjectsWithTag("enemy_controller");
            foreach (GameObject enemyController in otherEnemyControllers)
            {
                Physics2D.IgnoreCollision(enemyController.GetComponent<PolygonCollider2D>(), GetComponent<PolygonCollider2D>());
            }
        }
        
        protected GameObject FindAttackTarget()
        {
            GameObject _attackTarget = null, playerTarget = null;
            List<GameObject> allies = new List<GameObject>();

            // Key - value equivalent gameObject with distance between enemy
            Dictionary<GameObject, float> alliesDictionary = new Dictionary<GameObject, float>();



            playerTarget = GameObject.FindGameObjectWithTag("player");
            if (playerTarget != null) allies.Add(playerTarget);

            // get all allies
            GameObject[] alliesArray = GameObject.FindGameObjectsWithTag("allies");
            for (int i = 0; i < alliesArray.Length; i++)
            {
                // If enemy attack form is short range strike and target is plane, then ignore it
                // Handle logic here
                if (GetEnemyType() != EnemyType.BOSS_LEVEL_3
                    && IsShortRangeStrike()
                    && alliesArray[i].GetComponentInChildren<PlaneControiler>() != null
                )
                {
                    continue;
                }
                allies.Add(alliesArray[i]);
            }

            float shortestAttackTargetDistance = Vector2.Distance(playerTarget.transform.position, transform.position);

            foreach (GameObject alliesGameObject in allies)
            {
                float distance = Vector2.Distance(alliesGameObject.transform.position, transform.position);
                if (shortestAttackTargetDistance >= distance)
                {
                    shortestAttackTargetDistance = distance;
                }
                alliesDictionary.Add(alliesGameObject, distance);
            }
            _attackTarget = alliesDictionary.FirstOrDefault(x => x.Value <= shortestAttackTargetDistance).Key;
            return _attackTarget;
        }
    }
}
