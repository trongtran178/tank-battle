using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyFactory: MonoBehaviour
    {
        public GameObject effectDestroyEnemyStation;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            Debug.Log(bullet);

            if (bullet != null)
            {

                effectDestroyEnemyStation.SetActive(true);

            }
        }

      
    }
}
