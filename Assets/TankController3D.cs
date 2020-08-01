using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject cameraMain;
    new public GameObject playerCamera;

    public Image healthyBar;
    public float maxHealth = 100f;
    public float tankHealth;
    private int damge;


    public GameObject bulletPrefab;
    public GameObject pointShooting;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject tank= GameObject.Find("TankPzIV");

        //pv = GetComponent<PhotonView>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 15;

        if (photonView.IsMine)
        {

            tankHealth = maxHealth;
            nametxt.text = PhotonNetwork.NickName;
            anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            //cameraMain = GameObject.Find("Main Camera");
            playerCamera.SetActive(true);
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

    public void TakeDamage(int damge)
    {
        this.damge = damge;
        if (photonView.IsMine)
        {
            this.GetComponent<PhotonView>().RPC("PunTakeDamage", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    private void PunTakeDamage()
    {
        tankHealth -= this.damge;
    }


    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smootMove, Time.deltaTime * 10);
        healthyBar.fillAmount = tankHealth / maxHealth;
        //if (tankHealth <= 0)
        //    this.GetComponent<PhotonView>().RPC("destroyTank", RpcTarget.AllBuffered);
    }
    private void ProcessInput()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        transform.position += move * speed*Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            shooting();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.GetComponent<PhotonView>().RPC("destroyTank", RpcTarget.AllBuffered);
        }

        healthyBar.fillAmount = tankHealth / maxHealth;
        if (tankHealth <= 0)
            this.GetComponent<PhotonView>().RPC("destroyTank", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void destroyTank()
    {
        //cameraMain = GameObject.Find("Main Camera");
        //cameraMain.SetActive(true);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();

        PhotonNetwork.Destroy(this.gameObject);
        SceneManager.LoadScene("Menu");
    }
    public void shooting()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, pointShooting.transform.position, pointShooting.transform.rotation);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(tankHealth);
        }
        else if (stream.IsReading)
        {
           smootMove = (Vector3)stream.ReceiveNext();
            tankHealth = (float)stream.ReceiveNext();
        }

    }
}
