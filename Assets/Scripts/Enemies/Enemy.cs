using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
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
        protected GameObject player_body;

        public abstract void UpgrageLevelCorrespondToPhase(Phase phase);
        // public abstract void Instantiate();
        public abstract void Death();
        public abstract void TakeDamage(float damage);
        public abstract void ReceiveHealthBumpFromBoss();
        public abstract void SetCurrentHealth(float currentHealth);
        public abstract float GetCurrentHealth();
        public abstract GameObject GetSelf();
        public abstract EnemyType GetEnemyType();
        public abstract bool IsShortRangeStrike();

        protected GameObject FindAttackTarget()
        {

            GameObject _attackTarget = null;
            GameObject[] playerTargets = new GameObject[10];
            List<GameObject> allies = new List<GameObject>();

            // Key - value equivalent gameObject with distance between enemy
            Dictionary<GameObject, float> alliesDictionary = new Dictionary<GameObject, float>();

            playerTargets = GameObject.FindGameObjectsWithTag("player_body");
            //playerTarget = player_body;
            if (playerTargets != null)
            {
                for(int i = 0; i < playerTargets.Length; i++)
                {
                    if (!allies.Contains(playerTargets[i]))
                    {
                        allies.Add(playerTargets[i]);
                    }
                }
            }

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

            float shortestAttackTargetDistance = float.MaxValue;

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

        public GameObject GetPlayer()
        {
            return player;
        }

        public void IgnoreEnemies()
        {
            GameObject[] otherEnemyControllers = GameObject.FindGameObjectsWithTag("enemy_controller");
            foreach (GameObject enemyController in otherEnemyControllers)
            {
                Physics2D.IgnoreCollision(enemyController.GetComponent<PolygonCollider2D>(), GetComponent<PolygonCollider2D>());
            }
        }
    }
}
