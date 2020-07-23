using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class trajectoryScript : MonoBehaviour {

	public Sprite dotSprite;					//All of the dots will become the sprite assigned to this if this has a sprite assigned to it and changeSpriteAfterStart is true
	public bool changeSpriteAfterStart;			//When enabled, you will be able to change the above in the update loop. (it's less efficient)
	public float initialDotSize;				//The intial size of the trajectoryDots gameobject
	public int numberOfDots;					//The number of points representing the trajectory
	public float dotSeparation;					//The space between the points representing the trajectory
	public float dotShift;						//How far the first dot is from the "ball"
	public float idleTime;						//How long the player has to be inactive for the Help Gesture to begin animating
	private GameObject trajectoryDots;			//The parent of all the points representing the trajectory
	private GameObject ball;					//The projectile the player will be shooting
	private Rigidbody2D ballRB;					//The Rigidbody2D attached to the projectile the player will be shooting
	private Vector3 ballPos;					//Position of the ball
	private Vector3 fingerPos;					//Position of the pressed down finger/cursor on the screen 
	private Vector3 ballFingerDiff;				//The distance between where the finger/cursor is and where the "ball" is when screen is being pressed
	private Vector2 shotForce;					//How much velocity will be applied to the ball
	private float x1, y1;						//X and Y position which will be applied to each point of the trajectory
	private GameObject helpGesture;				//The Help Gesture which will become active after a period of inactivity
	private float idleTimer = 7f;				//How long the initial inactivity period will need to be before the Help Gesture shows up
	private bool ballIsClicked = false;			//If the cursor is hovering over the "Ball Click Area"
	private bool ballIsClicked2 = false;		//If the finger/cursor is pressing down in the "Ball Click Area" to activate the shot
	
	public float shootingPowerX;				//The amount of power which can be applied in the X direction
	public float shootingPowerY;				//The amount of power which can be applied in the Y direction
	public bool usingHelpGesture;				//If you want to use the Help Gesture
	public bool explodeEnabled;					//If you want to do something when the projectile reaches the last point of the trajectory
	public bool grabWhileMoving;				//Off means the player won't be able to shoot until the "ball" is still. On means they can stop the "ball" by clicking on it and shoot
	public GameObject[] dots;					//The array of points that make up the trajectory
	public bool mask;
	private BoxCollider2D[] dotColliders;
    public GameObject projectile;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
   
    public static float mana;
    public float manaBullet1;
    public float manaBullet2;
    public float manaBullet3;


    private GameObject projectile1;


    private GameObject projectile2;
    private bool flag = false;

    public int damage;

    public float speedRotation = 5f;

    public GameObject wfx_explosion;

    public static bool flagShoot = true;
    public static bool flagForMana = false;

    public static bool block1 = false;
    public static bool block2 = false;
    public static bool block3 = false;

    public float maxDistance;
    private bool flagCheckGun = true;
    private bool flagMouse = true;




	void Start ()
    {
        
            ball = gameObject;                                          //Script has to be applied to the "ball"
          
            trajectoryDots = GameObject.Find("Trajectory Dots");        //TRAJECTORY DOTS MUST HAVE THE SAME NAME IN HIERARCHY AS IT DOES HERE
          
            ballRB = GetComponent<Rigidbody2D>();                       //"Ball"'s Rigidbody2D is applied to ballRB

            trajectoryDots.transform.localScale = new Vector3(initialDotSize, initialDotSize, trajectoryDots.transform.localScale.z); //Initial size of trajectoryDots is applied

            for (int k = 0; k < 40; k++)
            {
                dots[k] = GameObject.Find("Dot (" + k + ")");           //All points are applied to the corresponding position in the dots array
                if (dotSprite != null)
                {                               //If a sprite is applied to dotSprite
                    dots[k].GetComponent<SpriteRenderer>().sprite = dotSprite;  //All points will have that sprite applied
                }
            }
            for (int k = numberOfDots; k < 40; k++)
            {                   //If the number of points being used is less than 40, the maximum...
                GameObject.Find("Dot (" + k + ")").SetActive(false);    //They will be hidden
            }
            trajectoryDots.SetActive(false);                            //Trajectory initialization complete, the trajectory is hidden

            block1 = true;
            block2 = true;
            block3 = true;
       

    }




    void Update()
    {


        Vector3 diffrence = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float rotZ = Mathf.Atan2(diffrence.y, diffrence.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);


        if (numberOfDots > 40)
        {
            numberOfDots = 40;
        }



        ballPos = ball.transform.position;                                      //ballPos is updated to the position of the "ball"

        if (changeSpriteAfterStart == true)
        {
            // If you've allowed the sprite to be continiously changed...
            for (int k = 0; k < numberOfDots; k++)
            {
                if (dotSprite != null)
                {
                    //If a sprite is applied to dotSprite
                    dots[k].GetComponent<SpriteRenderer>().sprite = dotSprite;//Change all points' sprite to the dotSprite sprite
                }
            }
        }

        if (block1 && flagMouse)
        {
            if (projectile == bullet1)
                flagCheckGun = true;
            if (Input.GetKey(KeyCode.Alpha1) && ManaTank.manaTank >= manaBullet1)
            {
                projectile = bullet1;
                //block1 = false;

            }
        }

        if (block2 && flagMouse)
        {
            if (projectile == bullet2)
                flagCheckGun = true;
            if (Input.GetKey(KeyCode.Alpha2) && ManaTank.manaTank >= manaBullet2)
            {
                projectile = bullet2;
                //block2 = false;

            }
        }

        if (block3 && flagMouse)
        {
            if (projectile == bullet3)
                flagCheckGun = true;
            if (Input.GetKey(KeyCode.Alpha3) && ManaTank.manaTank >= manaBullet3)
            {
                projectile = bullet3;
                //block3 = false;


            }
        }
        //flagShoot&&
        if (flagCheckGun && projectile != null)
        {
            if ((Input.GetKey(KeyCode.Mouse0)))
            {   //If player has activated a shot										//when you press down
                ballIsClicked2 = true;                                              //Final step of activation is complete

                flagMouse = false;


                fingerPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 80f));   //The position of your finger/cursor is found
                                                                                                                              // fingerPos = Input.mousePosition;


                Vector3 ballFingerDiff1 = ballPos - fingerPos;
                Debug.Log(ballFingerDiff1);


                // if(Mathf.Sqrt((ballFingerDiff1.x*ballFingerDiff1.x)+(ballFingerDiff1.y*ballFingerDiff1.y))>maxDistance)

                ballFingerDiff = ballFingerDiff1;

                shotForce = new Vector2(ballFingerDiff.x * shootingPowerX, ballFingerDiff.y * shootingPowerY);  //The velocity of the shot is found

                Debug.Log("shotforce: " + shotForce.x + "  " + shotForce.y);
                if (shotForce.x > 50)
                    shotForce.x = 50;

                if (shotForce.x < -50)
                    shotForce.x = -50;
                //if (shotForce.y > 50)
                //    shotForce.y = 50;

                if ((Mathf.Sqrt((ballFingerDiff.x * ballFingerDiff.x) + (ballFingerDiff.y * ballFingerDiff.y)) > (0.4f)))
                { //If the distance between the finger/cursor and the "ball" is big enough...
                    trajectoryDots.SetActive(true);                             //Display the trajectory
                }
                else
                {
                    trajectoryDots.SetActive(false);
                }

                for (int k = 0; k < numberOfDots; k++)
                {                           //Each point of the trajectory will be given its position
                    x1 = (ballPos.x) + shotForce.x * Time.fixedDeltaTime * (dotSeparation * k + dotShift);  //X position for each point is found
                    y1 = (ballPos.y) + shotForce.y * Time.fixedDeltaTime * (dotSeparation * k + dotShift) - (-Physics2D.gravity.y / 2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (dotSeparation * k + dotShift) * (dotSeparation * k + dotShift));  //Y position for each point is found
                    dots[k].transform.position = new Vector3(x1, y1, dots[k].transform.position.z); //Position is applied to each point
                }
            }



            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (flag == false)
                {
                    flag = true;

                    projectile1 = Instantiate(projectile, transform.position, transform.rotation) as GameObject;


                    projectile1.GetComponent<Rigidbody2D>().isKinematic = true;

                    projectile1.SetActive(false);

                    // GameObject tankPlayer = GameObject.FindGameObjectWithTag("player");

                    // tankPlayer.GetComponent<AudioSource>().Play();
                    //AudioSource _audio = (AudioSource)Instantiate(Resources.Load("gun-cocking-01.mp3"));
                    //_audio.Play();

                }


                flagMouse = true;
                projectile1.SetActive(false);
                ballIsClicked2 = false;                                         //Aiming is no longer happening

                if (trajectoryDots.activeInHierarchy)
                {                           //If the player was aiming...
                    if (explodeEnabled == true)
                    {                                   //If the player was shooting and explodeEnabled is true...
                        StartCoroutine(explode());                                  //The "explode" coroutine will start
                    }
                    flagMouse = true;
                    projectile1.SetActive(false);
                    ballIsClicked2 = false;                                         //Aiming is no longer happening

                    if (trajectoryDots.activeInHierarchy)
                    {                           //If the player was aiming...
                        if (explodeEnabled == true)
                        {                                   //If the player was shooting and explodeEnabled is true...
                            StartCoroutine(explode());                                  //The "explode" coroutine will start
                        }


                        trajectoryDots.SetActive(false);
                        if (projectile1 != null)
                        {

                            if (projectile == bullet1)
                            {
                                mana = manaBullet1;
                                block1 = false;
                            }


                            if (projectile == bullet2)
                            {
                                mana = manaBullet2;
                                block2 = false;
                            }

                            if (projectile == bullet3)
                            {
                                mana = manaBullet3;
                                block3 = false;
                            }

                            flagForMana = true;
                            projectile1.SetActive(true);
                            projectile1.GetComponent<Rigidbody2D>().isKinematic = false;
                            projectile1.GetComponent<Rigidbody2D>().AddForce(shotForce, ForceMode2D.Impulse);
                            projectile2 = Instantiate(wfx_explosion, transform.position, transform.rotation) as GameObject;


                            flag = false;

                            projectile1 = null;
                            projectile2 = null;
                            flagCheckGun = false;
                            //projectile = null;
                            flagShoot = false;
                            //flagForMana = false;
                            ManaTank.manaTank -= mana;

                        }


                    }
                }
            }

        }

    }
    public IEnumerator explode()
    {                                           //The explode function
        yield return new WaitForSeconds(Time.fixedDeltaTime * (dotSeparation * (numberOfDots - 1f)));   //Nothing will happen until the time it takes for the projectile to reach the last point of the trajectory passes
        Debug.Log("exploded");


        //Insert what happens when the time it takes for the projectile to reach the last point of the trajectory expires, (explodeEnabled has to be true)

    }

    public void collided(GameObject dot)
    {

        for (int k = 0; k < numberOfDots; k++)
        {
            if (dot.name == "Dot (" + k + ")")
            {

                for (int i = k + 1; i < numberOfDots; i++)
                {

                    dots[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }

            }

        }
    }
    public void uncollided(GameObject dot)
    {
        for (int k = 0; k < numberOfDots; k++)
        {
            if (dot.name == "Dot (" + k + ")")
            {

                for (int i = k - 1; i > 0; i--)
                {

                    if (dots[i].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                    {
                        Debug.Log("nigggssss");
                        return;
                    }
                }

                if (dots[k].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                {
                    for (int i = k; i > 0; i--)
                    {

                        dots[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;

                    }

                }
            }

        }
    }
}

