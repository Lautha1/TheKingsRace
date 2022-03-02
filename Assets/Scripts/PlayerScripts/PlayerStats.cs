using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStats : MonoBehaviour
{
    //Weather types
    public enum Weather {Rain, Snow, Wind, Fog};

    //Soft cap max speed a player can achieve they can achieve this speed by running
    [SerializeField] private float maxVel = 40.0f;
    public float MaxVel{
        get{ return maxVel; }
        set{ maxVel = value; }
    }

    //Minimum possible player speed
    [SerializeField] private float minVel = 15.0f;
    public float MinVel{
        get{ return minVel; }
        set{ minVel = value; }
    }
    
    //Current player speed
    [SerializeField] private float curVel = 0.0f;
    public float CurVel{
        get{ return curVel; }
        set{ curVel = value; }
    }

    //The absolute max speed a player can achieve through boosts or momentum increases will disipate down to soft cap
    [SerializeField] private float hardCapMaxVel = 60.0f;
    public float HardCapMaxVel{
        get{ return hardCapMaxVel; }
        set{ hardCapMaxVel = value; }
    }

    //Player Acceleration
    [SerializeField] private float acc = 0.01f;
    public float Acc{
        get{ return acc; }
        set{ acc = value; }
    }

    [SerializeField] private float curAcc;
    public float CurAcc{
        get{ return curAcc; }
        set{ curAcc = value;}
    }

    //Player Jump power
    [SerializeField] private float jumpPow = 60.0f;
    public float JumpPow{
        get{ return jumpPow; }
        set{ jumpPow = value; }
    }

    //Players Number of Jumps
    [SerializeField] private int jumpNum = 2;
    public int JumpNum{
        get{ return jumpNum; }
        set{ jumpNum = value; }
    }

    //Player Traction
    [SerializeField] private float traction = 3.0f;
    public float Traction{
        get{ return traction; }
        set{ traction = value; }
    }

    [SerializeField] private float curTraction;
    public float CurTraction{
        get{ return curTraction; }
        set{ curTraction = value; }
    }

    //Player Kick Power
    [SerializeField] private float kickPow = 500.0f;
    public float KickPow{
        get{ return kickPow; }
        set{ kickPow = value; }
    }

    //Player Recovery Time When Knocked Down
    [SerializeField] private float recovTime = 3.0f;
    public float RecovTime{
        get{ return recovTime; }
        set{ recovTime = value; }
    }

    //Gravity Affecting the player
    [SerializeField] private float playerGrav = 200.0f;
    public float PlayerGrav{
        get{ return playerGrav; }
        set{ playerGrav = value; }
    }

    //Players Downwards Velocity
    [SerializeField] private float gravVel = 0;
    public float GravVel{
        get{ return gravVel; }
        set{ gravVel = value; }
    }

    //If The Player Has Glider
    [SerializeField] private bool hasGlider = false;
    public bool HasGlider{
        get{ return hasGlider; }
        set{ hasGlider = value; }
    }

    //If The Player Has Grapple
    [SerializeField] private bool hasGrapple = false;
    public bool HasGrapple{
        get{ return hasGrapple; }
        set{ hasGrapple = value; }
    }

    //If The Player Has Wallrun
    [SerializeField] private bool hasWallrun = false;
    public bool HasWallrun{
        get{ return hasWallrun; }
        set{ hasWallrun = value; }
    }

    //If The Player Has Nitro
    [SerializeField] private bool hasNitro = false;
    public bool HasNitro{
        get{ return hasNitro; }
        set{ hasNitro = value; }
    }

    //If The Player Has Dash
    [SerializeField] private bool hasDash = false;
    public bool HasDash{
        get{ return hasDash; }
        set{ hasDash = value; }
    }

    //Spending points the player has
    [SerializeField] private int playerPoints = 15;
    public int PlayerPoints{
        get{ return playerPoints; }
        set{ playerPoints = value; }
    }

    //Is player paused
    [SerializeField] private bool isPaused = false;
    public bool IsPaused{
        get{ return isPaused; }
        set{ isPaused = value; }
    }

    ////Weather Related stats
    //Is wind on
    [SerializeField] private bool windOn = false;
    public bool WindOn{
        get{ return windOn; }
    }

    //Wind direction
    [SerializeField] private Vector3 windDirection;
    public Vector3 WindDirection{
        get{ return windDirection; }
        set{ windDirection = value; }
    }

    //temp values for reseting stuff
    private float tempTraction;
    private float tempAcc;
    private float accModification;

    //sets weather effect on player
    public void SetWeather(Weather weather, Vector3 windDir){
        switch(weather){
            case Weather.Rain:{
                Debug.Log("Rain");

                tempTraction = traction;
                tempAcc = acc;

                traction -= 2;
                if(curTraction - 2 > 0){
                   curTraction -= 2; 
                }

                acc += 2;
                curAcc += 2;
                accModification = 2;

                break;
            }
                
            case Weather.Snow:{
                Debug.Log("Snow");

                tempTraction = traction;
                tempAcc = acc;

                traction += 2;
                if(curTraction > .01 && curTraction != 1){
                   curTraction += 2;
                }

                acc *= .5f;
                curAcc *= .5f;
                accModification = -acc;

                break;
            }
            
            case Weather.Wind:{
                Debug.Log("Wind");

                windDirection = windDir;
                windOn = true;
                break;
            }

            case Weather.Fog:{
                Debug.Log("Fog");
                break;
            }
                
            default:{
                Debug.Log("Incorrect thing passed in");
                break;
            }
                
        }
    }

    //clears weather effects
    public void ClearWeather(){

        acc = tempAcc;
        traction = tempTraction;

        if(curTraction > .01 && curTraction != 1){
            curTraction = traction;
        }

        curAcc -= accModification;

        windOn = false;

    }
    ////

    void Update(){
        VariableValidation();
    }

    void VariableValidation(){
        Debug.Assert((maxVel >= 0), "maxVel cannot be below zero");
        Debug.Assert((minVel >= 0), "minVel cannot be below zero"); 
        Debug.Assert((curVel >= 0), "curVel cannot be below zero");
        Debug.Assert((jumpPow >= 0), "jumpPow cannot be below zero"); 
        Debug.Assert((jumpNum >= 2), "jumpNum cannot be below 2"); 
        Debug.Assert((traction >= 0), "traction cannot be below zero"); 
        Debug.Assert((kickPow >= 0), "Playerpoints cannot be below zero"); 
        Debug.Assert((recovTime >= 0), "recovTime cannot be below zero"); 
        Debug.Assert((playerGrav >= 0), "playerGrav cannot be below zero"); 
        Debug.Assert((playerPoints >= 0), "Playerpoints cannot be below zero");
    }
    
}
