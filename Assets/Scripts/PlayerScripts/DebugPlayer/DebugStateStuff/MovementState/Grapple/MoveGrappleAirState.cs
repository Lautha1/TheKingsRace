using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrappleAirState : MoveBaseState
{
    public override void EnterState(MoveStateManager mSM, MoveBaseState previousState){
        mSM.driftVel = Vector3.zero;
    }

    public override void ExitState(MoveStateManager mSM, MoveBaseState nextState){

    }

    public override void UpdateState(MoveStateManager mSM){
        if(mSM.aSM.currentState != mSM.aSM.GrappleAirState){
            //Determine which state to go into based on player speed
            if(mSM.calculatedCurVel < mSM.walkLimit){
                mSM.SwitchState(mSM.WalkState);
            }
            else if(mSM.calculatedCurVel < mSM.runLimit){
                mSM.SwitchState(mSM.JogState);
            }
            else{
                mSM.SwitchState(mSM.RunState);
            }
        }
    }   

    public override void FixedUpdateState(MoveStateManager mSM){

    }

}
