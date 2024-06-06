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

    private void GoDown()
    {

        if (!IsGrounded())
        {
            rb.position -= new Vector3(0, stepSmooth, 0);
        }
    }

    private void GoUpStair()
    {
        if (!Physics.Raycast(stepRayUpper.position, Vector3.forward, out RaycastHit hitUpper, .3f))
            return;
        if (IsGrounded())
        {
            Debug.Log("A");
            rb.position -= new Vector3(0, -stepSmooth, 0);

        }
    }
    public override void StopMoving()
    {
        base.StopMoving();
        
    }

    protected override void AwardPrize()
    {
        base.AwardPrize();
        joystick.enabled=false;
        StartCoroutine(ChangeState());
    }

    IEnumerator ChangeState(){
        yield return new WaitForSeconds(3);
        GameManager.Ins.ChangeState(GameManager.State.EndGame);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayUpper.position, stepRayUpper.position + new Vector3(0, 0, 0.3f));
        Gizmos.DrawLine(raycastPos.position, raycastPos.position + new Vector3(0, -2f, 0));

    }

}
