using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TankController3D : MonoBehaviourPun,IPunObservable
{
    public Rigidbody2D Rb;
    public float speed = 6;
    private float velx;
    private float vely;
    private Animator anim;
    public PhotonView pv;

    public Text nametxt;

    private Vector3 smootMove;
    //public GameObject cameraMain;
    new public GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject tank= GameObject.Find("TankPzIV");

        //pv = GetComponent<PhotonView>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 15;

        if (photonView.IsMine)
        {

            nametxt.text = PhotonNetwork.NickName;
            anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            //cameraMain = GameObject.Find("Main Camera");
            playerCamera.SetActive(trueư;
            //cameraMain.SetActive(false);
            
        }
        else
        {
            nametxt.text = pv.Owner.NickName;
            //Destroy(playerCamera);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!photonView.IsMine)
        //    return;
        //velx = Input.GetAxis("Horizontal");
        //vely = Rb.velocity.y;
        //Rb.velocity = new Vector2(speed, vely);

        if (photonView.IsMine)
        {
            Debug.Log("eeee: "+ pv.IsMine);
            animation();
       
            ProcessInput();
        }
        else
        {
            Debug.Log("eeee1: " );
            //animation();
            smoothMovement();
        }

        
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    anim.SetBool("isHit", true);
        //}
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    anim.SetBool("isHit", false);

        //}
    }

    private void animation()
    {

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMoveForward", true);
            anim.SetBool("isMoveBack", false);
           

        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isMoveForward", false);
            anim.SetBool("isMoveBack", true);
      

        }
        else
        {
            anim.SetBool("isMoveForward", false);
            anim.SetBool("isMoveBack", false);
            //speed = 0;
        }
    }
    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smootMove, Time.deltaTime * 10);
    }
    private void ProcessInput()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        transform.position += move * speed*Time.deltaTime;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
           smootMove = (Vector3)stream.ReceiveNext();
        }

    }
}
