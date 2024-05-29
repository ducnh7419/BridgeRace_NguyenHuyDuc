using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public FloatingJoystick joystick;
    
    

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Running();
    }

    protected override void Running(){
        rb.velocity = new Vector3(joystick.Horizontal * speed * Time.fixedDeltaTime, 0, joystick.Vertical * speed * Time.fixedDeltaTime);
        if(joystick.Horizontal!=0||joystick.Vertical!=0){
            transform.rotation=Quaternion.LookRotation(rb.velocity);
            isMoving=true;
            ChangeAnim("run");
            direct=(joystick.Horizontal<0)?Direct.Backward:Direct.Forward;
        }else{
            StopMoving();
        }
        
    }



}
