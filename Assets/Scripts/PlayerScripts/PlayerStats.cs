using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStats : MonoBehaviour
{
    //Maximum possible player speed
    private float maxVel = 25.0f;
    public float MaxVel{
        get{ return maxVel; }
        set{ maxVel = value; }
    }

    //Minimum possible player speed
    private float minVel = 2.0f;
    public float MinVel{
        get{ return minVel; }
        set{ minVel = value; }
    }
    
    //Current player speed
    private float curVel = 0.0f;
    public float CurVel{
        get{ return curVel; }
        set{ curVel = value; }
    }

    //Player Acceleration
    private float acc = 0.1f;
    public float Acc{
        get{ return acc; }
        set{ acc = value; }
    }

    //Player Jump power
    private float jumpPow = 300.0f;
    public float JumpPow{
        get{ return jumpPow; }
        set{ jumpPow = value; }
    }

    //Players Number of Jumps
    private int jumpNum = 2; ////// UPDATE TO 2 WHEN THE JUMP ISSUE IS FIXED
    public int JumpNum{
        get{ return jumpNum; }
        set{ jumpNum = value; }
    }

    //Player Traction
    private float traction = 3.0f;
    public float Traction{
        get{ return traction; }
        set{ traction = value; }
    }

    //Player Kick Power
    private float kickPow = 100.0f;
    public float KickPow{
        get{ return kickPow; }
        set{ kickPow = value; }
    }

    //Player Recovery Time When Knocked Down
    private float recovTime = 3.0f;
    public float RecovTime{
        get{ return recovTime; }
        set{ recovTime = value; }
    }

    //MAY BE UNNECCESARY DEPENDING ON HOW GLIDER TURNS OUT
    //Gravity Affecting the player
    private float playerGrav = 20.0f;
    public float PlayerGrav{
        get{ return playerGrav; }
        set{ playerGrav = value; }
    }

    //If The Player Has Blink
    private bool hasBlink = false;
    public bool HasBlink{
        get{ return hasBlink; }
        set{ hasBlink = value; }
    }

    //If The Player Has Glider
    private bool hasGlider = false;
    public bool HasGlider{
        get{ return hasGlider; }
        set{ hasGlider = value; }
    }

    //If The Player Has Grapple
    private bool hasGrapple = false;
    public bool HasGrapple{
        get{ return hasGrapple; }
        set{ hasGrapple = value; }
    }

    //If The Player Has Wallrun
    private bool hasWallrun = false;
    public bool HasWallrun{
        get{ return hasWallrun; }
        set{ hasWallrun = value; }
    }

    //If The Player Has Nitro
    private bool hasNitro = false;
    public bool HasNitro{
        get{ return hasNitro; }
        set{ hasNitro = value; }
    }

    //If The Player Has Dash
    private bool hasDash;
    public bool HasDash{
        get{ return hasDash; }
        set{ hasDash = value; }
    }

    private int playerPoints = 15;
    public int PlayerPoints{
        get{ return playerPoints; }
        set{ playerPoints = value; }
    }

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
