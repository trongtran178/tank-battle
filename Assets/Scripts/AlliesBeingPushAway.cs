using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlliesBeingPushAway : MonoBehaviour
{

    public GameObject self;
    public bool isPushAway = false;
    public bool isMoveToLeft = true;
    public GameObject effectDizzy;
    private Vector3 moveDir;
    private float beginningPushAwayTime;
    private bool collisionWithWall = false;
    private float moveDirSpeed = 5;
    void Start()
    {
        moveDir = self.transform.position - new Vector3(self.transform.position.x + (isMoveToLeft ? (5) : (-5)), self.transform.position.y, self.transform.position.z);

    }
    void Update()
    {
        moveDir = self.transform.position - new Vector3(self.transform.position.x + (isMoveToLeft ? (5) : (-5)), self.transform.position.y, self.transform.position.z);
        if (isPushAway)
        {
            self.transform.position += moveDir * moveDirSpeed * Time.fixedDeltaTime;
            float pushAwayTime = Time.time;
            self.GetComponent<dog>()?.Dizzy();
            self.GetComponent<EnemyTu>()?.Dizzy();
            self.GetComponent<TankController2>()?.Dizzy();
            
            //self.GetComponent<PlaneControiler>()?.Dizzy();
            if (pushAwayTime - beginningPushAwayTime >= 0.3f)
            {
                isPushAway = false;
                effectDizzy.SetActive(true);
                Invoke("DizzyFinished", 4.0f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision e " + collision.gameObject.tag);
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.Equals("vertical_wall")) {

            moveDirSpeed = 0;
        }
        


    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Collision exit " + collision.gameObject.tag);
        if (collision.gameObject.tag.Equals("vertical_wall"))
        {

            moveDirSpeed = 5;
        }
    }

    private void DizzyFinished()
    {
        effectDizzy.SetActive(false);
    }

    public void PushAway(bool isMoveToLeft)
    {
        isPushAway = true;
        this.isMoveToLeft = isMoveToLeft;
        beginningPushAwayTime = Time.time;
    }
}
