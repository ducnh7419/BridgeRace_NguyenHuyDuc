using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public DynamicJoystick joystick;
    [Header("Player step climb")]
    [SerializeField] Transform stepRayUpper;
    [SerializeField] float stepSmooth = 0.5f;


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ControlMove();

        if (direct == Direct.Forward)
        {
            GoUpStair();
            return;
        }
        if (direct == Direct.Backward)
        {
            GoDown();
        }
        
    }

    private void Update(){
        UserDataManager.Ins.Score=score;
    }

    protected override void OnInit()
    {
        base.OnInit();
        joystick=LevelManager.Ins.joystick;
        EnableJoystick();
    }


    /// <summary>
    /// Moving the player using joystick
    /// </summary>
    private void ControlMove()
    {
        if (joystick.Vertical < 0)
        {
            IsMovable = true;
        }
        if (joystick.Vertical > 0)
        {
            isBlockedByDoor = false;
        }
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            if (!IsMovable||isBlockedByDoor)
            {
                StopMoving();
                return;
            }
            rb.velocity = new Vector3(joystick.Horizontal * speed * Time.fixedDeltaTime, 0, joystick.Vertical * speed * Time.fixedDeltaTime);
            TF.rotation = Quaternion.LookRotation(rb.velocity);
            Moving();
            direct = (joystick.Vertical < 0) ? Direct.Backward : Direct.Forward;
        }
        else
        {
            StopMoving();
        }

    }

     protected bool OnGrounded()
    {
        if (Physics.Raycast(TF.position, Vector3.down, out RaycastHit hit, .2f))
        {
            return true;
        }
        return false;

    }

    /// <summary>
    /// Move the player down
    /// </summary>
    private void GoDown()
    {
        if (!Physics.Raycast(stepRayUpper.position, Vector3.down, out RaycastHit hit, 3f,1<<3))
            return;
        Debug.Log(hit.collider.tag);
        if (!OnGrounded())
        {
            rb.position -= new Vector3(0, stepSmooth, 0);
        }
    }

    /// <summary>
    /// Move the player up. Use it when player going upstair
    /// </summary>
    private void GoUpStair()
    {
        if (!Physics.Raycast(stepRayUpper.position, Vector3.forward, out RaycastHit hitUpper, .3f,1<<3))
            return;
        if (OnGrounded()){
            rb.position -= new Vector3(0, -stepSmooth, 0);
        }
    }

    public override void StopMoving()
    {
        base.StopMoving();
        rb.velocity = Vector3.zero;
    }

    protected override void AwardPrize()
    {
        base.AwardPrize(); 
        joystick.gameObject.SetActive(false);
        UserDataManager.Ins.Score=score;
        if(rank==0) return;
        if(rank==1){
            GameManager.Ins.CurrentResult=GameManager.GameResult.Win;
        }else{
            GameManager.Ins.CurrentResult=GameManager.GameResult.Lose;
            if(rank>3){
                return;
            }
        }
        Transform place=GetPodiumPlace();
        TF.SetPositionAndRotation(place.position, Quaternion.Euler(Vector3.up * 180));
        
        
    }

    

    public void EnableJoystick(){
        joystick.OnPointerUp(null);
        joystick.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayUpper.position, stepRayUpper.position + new Vector3(0, 0, 0.3f));
        Gizmos.DrawLine(stepRayUpper.position, raycastPos.position + new Vector3(0, -1f, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(TF.position,TF.position+new Vector3(0,-0.8f,0));

    }

}
