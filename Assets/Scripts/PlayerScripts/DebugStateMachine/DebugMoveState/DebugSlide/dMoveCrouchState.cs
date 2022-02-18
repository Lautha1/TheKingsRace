using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dMoveCrouchState : dMoveBaseState
{

    //Slide Variables
    RaycastHit slideRay; // slide raycast

    public override void EnterState(dMoveStateManager mSM, dMoveBaseState previousState){

        //if not coming from slide state then rotate player and adjust height
        if(previousState != mSM.SlideState){
            mSM.pStats.CurVel = 0;
            mSM.gameObject.transform.eulerAngles = new Vector3(mSM.transform.localEulerAngles.x - 90, mSM.transform.localEulerAngles.y, mSM.transform.localEulerAngles.z);
            mSM.moveController.height *= .5f;
        }
    }
    
    public override void ExitState(dMoveStateManager mSM, dMoveBaseState nextState){
        
        //if next state isn't crouch walk revert rotation, speed, and height
        if(nextState != mSM.CrouchWalkState){
            mSM.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            mSM.pStats.CurVel = mSM.calculatedCurVel;
            mSM.moveController.height *= 2.0f; 
        }
        
    }

    public override void UpdateState(dMoveStateManager mSM){
        
    }

    public override void FixedUpdateState(dMoveStateManager mSM){
        mSM.transform.Rotate(Vector3.forward * -mSM.sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));

        ///////ONCE WE HAVE IT SO SLIDE DOESNT ROTATE PLAYER MOVE THIS TO UPDATE
        //If player isn't pressing either Q or the joystick button they stop crouching if nothing is above them
        if((!Input.GetKey(KeyCode.JoystickButton1) && !Input.GetKey(KeyCode.Q))){
            if ((Physics.Raycast(mSM.gameObject.transform.position, mSM.slideUp, out slideRay, 5f) == false)){

                mSM.SwitchState(mSM.IdleState);
            }
            else{
                Debug.Log("Object above you");
            }
        }

        /*
        //If falling stop sliding and go to wasd states
        if(mSM.aSM.currentState == mSM.aSM.FallingState){
            ExitCrouchState(mSM);
            mSM.SwitchState(mSM.IdleState);
        }
        */
    }
}
