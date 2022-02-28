using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dMoveIdleState : dMoveBaseState
{
    public override void EnterState(dMoveStateManager mSM, dMoveBaseState previousState){
        
    }
    
    public override void ExitState(dMoveStateManager mSM, dMoveBaseState nextState){

    }

    public override void UpdateState(dMoveStateManager mSM){

        //Move to Walk State after speed increases
        if(mSM.calculatedCurVel >= mSM.idleLimit){
            mSM.SwitchState(mSM.WalkState);
        }

        //If Q or joystick button1 crouch state
        else if((Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.Q)) && (mSM.aSM.currentState != mSM.aSM.FallingState && mSM.aSM.currentState != mSM.aSM.WallRunState && mSM.aSM.currentState != mSM.aSM.WallIdleState && mSM.aSM.currentState != mSM.aSM.GrappleGroundedState)){
            mSM.SwitchState(mSM.CrouchState);
        }
    }

    public override void FixedUpdateState(dMoveStateManager mSM){
        
        //actual directional movement
        mSM.DirectionalMovement();
    }
}