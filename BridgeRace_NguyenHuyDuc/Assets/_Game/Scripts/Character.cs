
using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;


public class Character : GameUnit
{

    public ColorByEnum ColorByEnum;
    protected int currentPlatform = 1;

    [SerializeField] protected CharacterController characterController;
    public int NumberOfBricks = 0;
    private float brickHeight;

    public bool IsMovable;

    private string currAnim = "idle";
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private ColorData colorData;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] protected float speed;
    [SerializeField] protected Transform raycastPos;
    [SerializeField] public Transform brickBag;
    [Header("Character Brick")]
    public CharacterBrick characterBrickPrefab;
    public List<CharacterBrick> brickHolder;
    protected List<GameObject> blockedDoor = new List<GameObject>();

    protected enum Direct
    {
        None = 0,
        Forward = 1,
        Backward = 2
    }

    protected Direct direct;



    private void Awake()
    {
        IsMovable = true;

    }

    protected virtual void OnInit()
    {

    }

    protected virtual void AwardPrize()
    {
        StopMoving();
        IsMovable=false;
        Transform place = LevelManager.Ins.GetPodiumPlace();
        TF.SetPositionAndRotation(place.position, Quaternion.Euler(Vector3.up * -180));
        Dance(Random.Range(1, 5));
    }

    protected void Dance(int number)
    {
        if(LevelManager.Ins.Rank==0){
            ChangeAnim("victory");
            return;
        }
        switch (number)
        {
            case 1:
                ChangeAnim("dance1");
                break;
            case 2:
                ChangeAnim("dance2");
                break;
            case 3:
                ChangeAnim("dance3");
                break;
            case 4:
                ChangeAnim("dance4");
                break;
        }

    }


    protected bool IsGrounded()
    {
        if (Physics.Raycast(TF.position, Vector3.down, out RaycastHit hit, .8f))
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

    public virtual void StopMoving()
    {
        ChangeAnim("idle");
        direct = Direct.None;
        rb.velocity = Vector3.zero;
    }

    public virtual void Moving(){
        ChangeAnim("run");
        IsMovable=true;
    }

    protected void DropBrick(){
        for(int i=0;i<brickHolder.Count;i++){
            SimplePool.Despawn(brickHolder[i]);
            brickHolder.RemoveAt(i);
        }
    }

    protected virtual void FixedUpdate()
    {
        switch (direct)
        {
            case Direct.Forward:
                ConstructingBridge();
                break;
            case Direct.Backward:
                IsMovable = true;
                break;
            case Direct.None:
                StopMoving();
                break;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        CollideWithBrick(other);
        ChangePlatform(other);
        ReachingPodium(other);
    }

    private void ReachingPodium(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.PODIUM)) return;
        DropBrick();
        AwardPrize();
        
    }


    private void CollideWithBrick(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.BRICK)) return;
        Brick brick = CacheCollider<Brick>.GetCollider(other);
        if (brick.ColorByEnum != ColorByEnum) return;
        SpawnCharacterBrick();
        brick.TurnOff();
    }

    protected void ChangePlatform(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.DOOR)) return;
        GameObject door = CacheCollider<GameObject>.GetCollider(other);
        if (blockedDoor.Contains(door))
        {
            Debug.Log("Stop");
            StopMoving();
            return;
        }
        blockedDoor.Add(door);
        ++currentPlatform;
        Platform.EnablePlatform(currentPlatform, (int)ColorByEnum);
    }

    public void ConstructingBridge()
    {
        if (!Physics.Raycast(raycastPos.position, Vector3.down, out RaycastHit hit, 2f, 1 << 3)) return;
        if (!hit.collider.CompareTag(GlobalConstants.Tag.STAIR)) return;
        Stair stair = CacheCollider<Stair>.GetCollider(hit.collider);
        if (stair.ColorByEnum == ColorByEnum)
        {
            IsMovable = true;
            return;
        }
        if (brickHolder.Count <= 0)
        {
            IsMovable = false;
            StopMoving();
            return;
        }
        stair.SetStairColor((int)ColorByEnum);
        stair.ActiveRenderer();
        SimplePool.Despawn(brickHolder[^1]);
        brickHolder.Remove(brickHolder[^1]);
        brickHeight -= 0.3f;
        IsMovable = true;
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
