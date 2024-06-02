using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class Character : GameUnit
{

    public ColorByEnum ColorByEnum;

    [SerializeField] protected CharacterController characterController;
    public int NumberOfBricks = 0;

    protected bool isMovable;

    private string currAnim = "idle";
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private ColorData colorData;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] protected float speed;
    [SerializeField] protected Transform raycastPos;
    protected bool isMoving = false;
    [SerializeField] public Transform brickBag;
    [Header("Character Brick")]
    public CharacterBrick characterBrickPrefab;
    public List<CharacterBrick> brickHolder;

    protected enum Direct
    {
        None = 0,
        Forward = 1,
        Backward = 2
    }

    protected Direct direct;

    [Header("Player step climb")]
    [SerializeField] Transform stepRayUpper;
    [SerializeField] float stepSmooth = 0.5f;
    private float brickHeight;

    private void Awake()
    {
        isMovable = true;

    }

    protected virtual void OnInit(){

    }


    protected bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, .8f))
            return true;
        return false; 

    }


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

    protected virtual void StopMoving()
    {
        isMoving = false;
        ChangeAnim("idle");
        direct = Direct.None;
        rb.velocity = Vector3.zero;
    }

    protected virtual void Running()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (direct == Direct.Forward)
        {
            ConstructingBridge();
            GoUpStair();
            return;
        }
        if (direct == Direct.Backward)
        {
            GoDown();
            isMovable = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollideWithBrick(other);
    }

    private void CollideWithBrick(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.BRICK)) return;
        Brick brick = Cache.GetBrick(other);
        if (brick.ColorByEnum != ColorByEnum) return;
        SpawnCharacterBrick();
        brick.TurnOff();
    }

    public void ConstructingBridge()
    {
        Debug.Log("BBBBBBBB");
        if (!Physics.Raycast(raycastPos.position, Vector3.down, out RaycastHit hit, 1f)) return;
        if (!hit.collider.CompareTag(GlobalConstants.Tag.Stair)) return;
        Stair stair = Cache.GetStair(hit.collider);
        if (stair.ColorByEnum == ColorByEnum)
        {
            isMovable = true;
            return;
        }
        if (!(brickHolder.Count > 0))
        {
            isMovable = false;
            return;
        }
        stair.SetStairColor((int)ColorByEnum);
        stair.ActiveRenderer();
        SimplePool.Despawn(brickHolder[^1]);
        brickHolder.Remove(brickHolder[^1]);
        brickHeight -= 0.3f;
        isMovable = true;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayUpper.position, stepRayUpper.position + new Vector3(0, 0, 0.3f));
        Gizmos.DrawLine(raycastPos.position, raycastPos.position + new Vector3(0, -1f, 0));

    }

    internal void SpawnCharacterBrick()
    {
        brickHeight += 0.3f;
        Vector3 brickPos = new(brickBag.position.x, brickBag.position.y + brickHeight, brickBag.position.z);
        CharacterBrick characterBrick = SimplePool.Spawn<CharacterBrick>(characterBrickPrefab);
        characterBrick.transform.SetParent(brickBag);
        characterBrick.transform.localRotation = characterBrickPrefab.transform.rotation;
        characterBrick.SetPosition(brickPos);
        characterBrick.SetBrickColor((int)ColorByEnum);
        brickHolder.Add(characterBrick);
    }
}
