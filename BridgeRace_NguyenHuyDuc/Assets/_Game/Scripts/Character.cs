using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class Character : GameUnit
{

    public ColorByEnum ColorByEnum;

    [SerializeField]protected CharacterController characterController;

    private string currAnim = "idle";
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private ColorData colorData;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] protected float speed;
    [SerializeField] protected Transform raycastPos;
    protected bool isMoving = false;
    [SerializeField] public Transform brickBag;

    protected enum Direct
    {
        None = 0,
        Forward = 1,
        Backward = 2
    }

    protected Direct direct;

    [Header("Player step climb")]
    [SerializeField] Transform stepRayUpper;
    [SerializeField] Transform stepRayLower;
    [SerializeField] float stepSmooth = 0.5f;




    public void SetCharacterColor(int index)
    {
        ColorByEnum = (ColorByEnum)index;
        meshRenderer.material = colorData.GetColorByEnum(index);
    }

    protected void ChangeAnim(string animName)
    {
        if (animName != currAnim)
        {
            currAnim = animName;
            animator.SetTrigger(animName);
        }
    }

    protected void StopMoving()
    {
        isMoving = false;
        ChangeAnim("idle");
        direct = Direct.None;
    }

    protected virtual void Running()
    {

    }

    protected virtual void FixedUpdate()
    {
        Debug.Log(direct);
        if (direct == Direct.Forward)
        {
            ConstructingBridge();
            GoUpStair();
        }
        else if (direct == Direct.Backward)
        {
            // GoDown();
        }
    }

    public void ConstructingBridge()
    {
        if (Physics.Raycast(raycastPos.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.CompareTag(GlobalConstants.Tag.Stair))
            {
                Stair stair = Cache.GetStair(hit.collider);
                stair.SetStairColor((int)ColorByEnum);

            }
        }
    }

    private void GoDown()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, .8f))
        {                   
                    rb.position -= new Vector3(0, stepSmooth, 0);         
        }
    }

    private void GoUpStair()
    {
        if (Physics.Raycast(stepRayUpper.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitUpper, .3f))
        {           
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, .85f))
                {
                    Debug.Log(hit.collider.tag);
                    if (hit.collider.CompareTag(GlobalConstants.Tag.Stair))
                        
                        rb.position -= new Vector3(0, -stepSmooth, 0);

                }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayUpper.position, stepRayUpper.position + new Vector3(0, 0, 0.3f));
        Gizmos.DrawLine(stepRayLower.position, stepRayLower.position + new Vector3(0, 0, .25f));
        Gizmos.DrawLine(raycastPos.position, raycastPos.position + new Vector3(0, .85f, 0));
    }


}
