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

        // Detect collision with other enemy, enemy should go through other enemy
        private void OnEnable()
        {
            GameObject[] otherEnemyControllers = GameObject.FindGameObjectsWithTag("enemy_controller");
            foreach (GameObject enemyController in otherEnemyControllers)
            {
                Physics2D.IgnoreCollision(enemyController.GetComponent<PolygonCollider2D>(), GetComponent<PolygonCollider2D>());
            }
        }

    }
}
