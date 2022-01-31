using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class dGrapplingHook : NetworkBehaviour
{
    public float maxGrappleDistance = 15;

    public bool isGrappled;
    private int hookPointIndex;
    private GameObject hookPoint;
    private GameObject[] hookPoints;
    private float distance;

    private CharacterController movementController;
    private dPlayerMovement playerMovement;
    private PlayerStats pStats;
    [SerializeField] private float ropeLength;
    private float climbRate = 5;
    private Vector3 swingDirection;
    float inclinationAngle;
    float theta = -1;

    private float maxSwingSpeed = 90;
    private float minSwingSpeed = 10;
    private float swingAcc = 5f;
    private float swingSpeed = 10;

    private float swingMom = 50;
    private Vector3 tensionMomDirection;
    private Vector3 hookPointRight;
    private Vector3 momDirection;
    private Vector3 curXZDir;
    private Vector3 oldXZDir;
    private float flip = -1; // flip variables for swing momentum
    private bool swingback = false; //swing the player back
    private float oldSwingMom;

    Vector3 tensionDirection;
    float tensionForce;
    public Vector3 forceDirection;


    // Start is called before the first frame update
    void Start()
    {
        isGrappled = false; 
        hookPoints = GameObject.FindGameObjectsWithTag("HookPoint");
        movementController = gameObject.GetComponent<CharacterController>();
        playerMovement = gameObject.GetComponent<dPlayerMovement>();
        pStats = gameObject.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!IsLocalPlayer) { return; }

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2)) && pStats.HasGrapple) //If grapple button is hit
        {
            if (!isGrappled) //If we are not grappling
            {
                hookPointIndex = FindHookPoint(); //Find the nearest hook point within max distance
                if (hookPointIndex != -1) //If there is a hookpoint
                {
                    hookPoint = hookPoints[hookPointIndex]; //The point we are grappling from
                    ropeLength = Vector3.Distance(gameObject.transform.position, hookPoint.transform.position);
                    oldXZDir = (new Vector3(hookPoint.transform.position.x,0,hookPoint.transform.position.z) - new Vector3(transform.position.x,0,transform.position.z)).normalized;
                    curXZDir = (new Vector3(hookPoint.transform.position.x,0,hookPoint.transform.position.z) - new Vector3(transform.position.x,0,transform.position.z)).normalized;
                    oldSwingMom = swingMom;
                    isGrappled = true; //toggle grappling state
                }
            } 
            else //Else we are grappling
            {
                isGrappled = false; //toggle grappling state to release
                playerMovement.g = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        //if (!IsLocalPlayer) { return; }
        if (isGrappled)
        {
            Debug.DrawRay(gameObject.transform.position, (hookPoint.transform.position - gameObject.transform.position)); //Visual of line

            //Extend Hook
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton4))
            {
                ropeLength += climbRate * Time.deltaTime;
                if (ropeLength > maxGrappleDistance)
                {
                    ropeLength = maxGrappleDistance;
                }
                //Debug.Log(ropeLength.ToString());
            }
            
            //Retract Hook
            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.JoystickButton5))
            {
                ropeLength -= climbRate * Time.deltaTime;
                if (ropeLength < 5)
                {
                    ropeLength = 5;
                }
                //Debug.Log(ropeLength.ToString());
            }
            //Debug.Log(Vector3.Distance(gameObject.transform.position, hookPoint.transform.position));
            Debug.Log(ropeLength);
            //Calculate tether force direction based on hookpoint
            if (Vector3.Distance(gameObject.transform.position, hookPoint.transform.position) >= ropeLength )
            {
                
                forceDirection = CalculateForceDirection(1, playerMovement.g, hookPoint.transform.position) + RopeLengthOffset(hookPoint.transform.position, Vector3.Distance(gameObject.transform.position, hookPoint.transform.position));
                

            }
            else{
                forceDirection = Vector3.zero;
            }

            //apply special swing movement when aerial
            if(!playerMovement.isGrounded){
                movementController.Move(SwingMoveController());
                if(swingMom != 0){
                    movementController.Move(CalculateMomentumDirection(playerMovement.g, hookPoint.transform.position));
                    swingMom -= .5f;
                }
            }
            if(swingMom<0) swingMom = 0;
        }
        else if(!isGrappled || playerMovement.isGrounded){
            flip = -1;
            swingback = true;
            swingMom = 50;
        }
        //WILL NEED ADJUSTMENT OR REMOVAL IN THE FUTURE
        //ungrapple on jump
        if(playerMovement.GetJumpPressed() && !playerMovement.isGrounded) isGrappled = false;

        //Reset force direction after unhook
        if(isGrappled == false) forceDirection = Vector3.zero;
    }

    //Finds the nearest hook to the player
    int FindHookPoint()
    {
        float least = maxGrappleDistance;
        int index = -1;
        for(int i = 0; i<hookPoints.Length; i++)
        {
            distance = Vector3.Distance(gameObject.transform.position, hookPoints[i].transform.position);
            if (distance <= least)
            {
                index = i;
            }
        }
        return index;
    }

    //Needs to be Overhauled to properly implement energy loss taking into account players current Velocity, height and input
    //------Important Equations--------
    // TensionForce = Cos(theta) * g * m * Vector3(tensionDirection)
    // Potential Energy = m * g * h -- Will need to modify adding current speed
    // Kinetic Energy = 1/2 * m * V^2 -- assuming no energy loss at the bottom KE = PE at peak we will add energy loss ourselves
    // Total Energy = m * ((g*h) + (1/2 * V^2))
    // Velocity = (2 * ((TotalEnergy/m) - (g*h)))^(1/2) -- This hypothetically could be used for our swing momentum calculation
    // Movement Direction right angle of tension force = Sin(theta) * g * m * Vector3(Right angle to tensionDirection)

    //Calculate the tether direction vector and how much force that vector needs
    Vector3 CalculateForceDirection(float mass, float g, Vector3 hPoint){

        //tension direction and angle calculation
        tensionDirection = (hPoint - transform.position).normalized;
        inclinationAngle = Vector3.Angle((transform.position - hPoint).normalized, -transform.up);
        theta = Mathf.Deg2Rad * inclinationAngle;
        if(theta<=.1) theta = 0;

        //How much force the tension needs
        tensionForce = mass * -g * Mathf.Cos(theta);

        //force direction calculation based on tension direction and force
        Vector3 fDirection = tensionDirection * tensionForce;
        return fDirection;
    }

    Vector3 CalculateMomentumDirection(float g, Vector3 hPoint){

        tensionMomDirection = (hPoint - transform.position).normalized;
        hookPointRight = Vector3.Cross(oldXZDir, transform.up).normalized;
        momDirection = -1 * Vector3.Cross(hookPointRight, tensionMomDirection).normalized;
        
        if(oldXZDir != curXZDir && flip == -1){
            //midpointMom = swingMom;
            swingback = false;
            Debug.Log("flip");
        }

        if(swingback == false && swingMom <= (oldSwingMom*(.75f))){
            swingback = true;
            oldSwingMom = swingMom;
            oldXZDir = (new Vector3(hookPoint.transform.position.x,0,hookPoint.transform.position.z) - new Vector3(transform.position.x,0,transform.position.z)).normalized;
            Debug.Log("swingback");
        }

        curXZDir = (new Vector3(hPoint.x,0,hPoint.z) - new Vector3(transform.position.x,0,transform.position.z)).normalized;
        Debug.DrawRay(transform.position,  momDirection * Time.deltaTime* 100, Color.green);
        return (momDirection * Time.deltaTime * swingMom);
    }

    //Calculates the players initial swing momentum using their height and their current velocity
    float CalculateSwingMom(Vector3 playerSpeed){
        return 0.0f;
    }
    
    //Special movement for the player while they swing
    Vector3 SwingMoveController(){

        //WASD input
        float inputVert = Input.GetAxis("Vertical");
        float inputHor = Input.GetAxis("Horizontal");

        //input is zero when nothing is pressed to prevent button easing values
        if((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))) inputVert = 0;
        if((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))) inputHor = 0;

        //Swingspeed build up
        if((inputVert != 0 || inputHor != 0) && swingSpeed < maxSwingSpeed){
            swingSpeed += swingAcc;
        }
        else if((inputVert != 0 || inputHor != 0) && swingSpeed >= maxSwingSpeed){
            swingSpeed = maxSwingSpeed;
        }
        else if((inputVert == 0 && inputHor == 0)){
            swingSpeed = minSwingSpeed;
        }

        //Swing direction based on player input
        swingDirection = Vector3.Cross(tensionDirection, ((transform.right * -inputVert) + (transform.forward * inputHor))).normalized;


        //NEED TO ADD SWINGSPEED EASING
        //this could be just adding a gradual increase in the swing speed instead of using a flat rate
        //Swing movement with swing speed added
        Vector3 swingMovement = (swingDirection * Time.deltaTime * swingSpeed);
        

        return (swingMovement);
    }

    Vector3 RopeLengthOffset(Vector3 hPoint, float curDistance){
        
        //How powerful our offset movement has to be
        float offsetPower = ((curDistance - ropeLength) * 200f);

        //The direction we need to apply force to offset when the rope gets lengthened beyond the necessary point
        Vector3 tenDirOffset = (hPoint - transform.position).normalized;
 

        return tenDirOffset * offsetPower * Time.deltaTime;
    }
}
