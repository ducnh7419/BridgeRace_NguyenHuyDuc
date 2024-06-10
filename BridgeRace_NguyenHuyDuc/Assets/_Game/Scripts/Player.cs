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
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            if (!IsMovable)
            {
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

    /// <summary>
    /// Move the player down
    /// </summary>
    private void GoDown()
    {

        if (!IsGrounded())
        {
            rb.position -= new Vector3(0, stepSmooth, 0);
        }
    }

    /// <summary>
    /// Move the player up. Use it when player going upstair
    /// </summary>
    private void GoUpStair()
    {
        if (!Physics.Raycast(stepRayUpper.position, Vector3.forward, out RaycastHit hitUpper, .3f))
            return;
        if (IsGrounded())
        {
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
        if(LevelManager.Ins.Rank<=2){
            GameManager.Ins.CurrentResult=GameManager.GameResult.Win;
        }
        UserDataManager.Ins.Score=score;
        StartCoroutine(ChangeGameState());
    }

    

    public void EnableJoystick(){
        joystick.OnPointerUp(null);
        joystick.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayUpper.position, stepRayUpper.position + new Vector3(0, 0, 0.3f));
        Gizmos.DrawLine(raycastPos.position, raycastPos.position + new Vector3(0, -2f, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(TF.position,TF.position+new Vector3(0,-0.8f,0));

    }

}
