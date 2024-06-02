using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public DynamicJoystick joystick;
    
    

    protected override void FixedUpdate()
    {
        base.FixedUpdate();  
        Running();
        
    }

    protected override void Running(){
        
        if(joystick.Vertical<0){        
            isMovable=true;
        }
        if(isMovable){
            rb.velocity = new Vector3(joystick.Horizontal * speed * Time.fixedDeltaTime, 0, joystick.Vertical * speed * Time.fixedDeltaTime);
        }else{
            StopMoving();
        }
        if(joystick.Horizontal!=0||joystick.Vertical!=0){
            transform.rotation=Quaternion.LookRotation(rb.velocity);
            isMoving=true;
            ChangeAnim("run");
            direct=(joystick.Vertical<0)?Direct.Backward:Direct.Forward;
        }else{
            StopMoving();
        }
        
    }



}
