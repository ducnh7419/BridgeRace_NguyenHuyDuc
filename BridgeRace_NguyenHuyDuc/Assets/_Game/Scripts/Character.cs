
using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;


public class Character : GameUnit
{
    protected int rank;
    public ColorByEnum ColorByEnum;
    protected int currentPlatform;
    protected Vector3 spawnLocation;
    public Vector3 SpawnLocation { get => spawnLocation; set => spawnLocation = value; }
    public int Rank { get => rank; set => rank = value; }

    [SerializeField] protected CharacterController characterController;
    private float brickHeight;
    protected int score;
    public bool IsMovable;
    protected bool isBlockedByDoor;
    protected bool isOnPodium;

    private string currAnim = "idle";
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private ColorData colorData;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] protected float speed;
    [SerializeField] protected Transform raycastPos;
    [SerializeField] public Transform brickBag;
    [SerializeField] private DroppedBrick droppedBrickPrefab;
    [Header("Character Brick")]
    public CharacterBrick characterBrickPrefab;

    public List<CharacterBrick> brickHolder;
    private List<Door> blockedDoors=new();

    protected enum Direct
    {
        None = 0,
        Forward = 1,
        Backward = 2
    }

    protected Direct direct;


    private void Start()
    {
        OnInit();
    }

    private void OnEnable()
    {
        OnInit();
        
    }

    protected virtual void OnInit()
    {
        StopMoving();
        IsMovable = true;
        isBlockedByDoor=false;
        isOnPodium=false;
        brickHeight = 0;
        currentPlatform = 0;
        blockedDoors=new();
        brickHolder = new();
        score = 0;
        rank=0;
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
        ForceEndgame();

    }

    /// <summary>
    /// Move the character to podium
    /// </summary>
    protected virtual void AwardPrize()
    {
        IsMovable = false;
        DropBrick();
        isOnPodium=true;
        if(rank==1){
            ChangeAnim("victory");
        }else if(rank>1&&rank<=3){
            Dance(Random.Range(1, 5));
        }
    }

    protected Transform GetPodiumPlace(){
        return LevelManager.Ins.GetPodiumPlace(rank);
    }

    /// <summary>
    /// Calling once character reaching goal 
    /// </summary>
    private void ForceEndgame(){
        if(GameManager.Ins.CurrState!=GameManager.State.EndGame||rank==1||isOnPodium) return;
        Transform goal=LevelManager.Ins.GetGoal();
        float goalDistance=Vector3.Distance(this.TF.position,goal.position);
        LevelManager.Ins.SetGoalDistances(this,goalDistance);
        LevelManager.Ins.ChangeCameraSpotlight(goal);
        StartCoroutine(DelayAwardPrize());
    }

    private IEnumerator DelayAwardPrize(){
        yield return new WaitForSeconds(1f);
        AwardPrize();
    }

    protected void Dance(int number)
    {
        
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

    private void OnCollisionEnter(Collision other)
    {
        if (!(other.collider.CompareTag(GlobalConstants.Tag.BOT) || other.collider.CompareTag(GlobalConstants.Tag.PLAYER))) return;
        Character character = CacheCollider<Character>.GetCollider(other.collider);
        if (brickHolder.Count > character.brickHolder.Count)
        {
            Kick();
            Debug.Log("Kick");
        }
        else
        {
            DropBrickToTheGround();
        }
    }


    private void DropBrickToTheGround()
    {
        Vector3 pos = TF.position + new Vector3(Random.Range(-3f, 3f), 0.2f, Random.Range(-3f, 3f));
        for (int i = 0; i < brickHolder.Count; i++)
        {
            Quaternion rot = Quaternion.Euler(droppedBrickPrefab.TF.rotation.eulerAngles + new Vector3(droppedBrickPrefab.TF.rotation.y, Random.Range(-180f, 180f), 0));
            DroppedBrick droppedBrick = SimplePool.Spawn<DroppedBrick>(droppedBrickPrefab);
            droppedBrick.SetPosition(pos);
            droppedBrick.SetRotation(rot);
        }
        DropBrick();
    }

    protected void Kick()
    {
        ChangeAnim("kick");
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
    }

    public virtual void Moving()
    {
        ChangeAnim("run");
        IsMovable = true;
    }

    /// <summary>
    /// Drop all character bricks
    /// </summary>
    protected void DropBrick()
    {
        if (Physics.Raycast(raycastPos.position, Vector3.down, out RaycastHit hit, 2f, 1 << 3)){
            if (hit.collider.CompareTag(GlobalConstants.Tag.STAIR)) return;
        }
        for (int i = 0; i < brickHolder.Count; i++)
        {
            SimplePool.Despawn(brickHolder[i]);
        }
        brickHeight = 0;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        CollideWithDoor(other);
        CollideWithBrick(other);
        ReachingDeadZone(other);
        CollideWithGoal(other);
        CollideWithDroppedBrick(other);
    }

    private void CollideWithDroppedBrick(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.DROPPEDBRICK)) return;
        DroppedBrick brick = CacheCollider<DroppedBrick>.GetCollider(other);
        SpawnCharacterBrick();
        SimplePool.Despawn(brick);
        score += 2;
    }

    private void CollideWithGoal(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.PODIUM)) return;

        GameManager.Ins.ChangeState(GameManager.State.EndGame);
        rank=1;
        AwardPrize();
    }



    private void ReachingDeadZone(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.DEADZONE)) return;
        OnInit();
        TF.position = spawnLocation;
    }

    private void CollideWithBrick(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.BRICK)) return;
        Brick brick = CacheCollider<Brick>.GetCollider(other);
        if (brick.ColorByEnum != ColorByEnum) return;
        SpawnCharacterBrick();
        brick.TurnOff();
        score += 2;
    }

    private void CollideWithDoor(Collider other)
    {
        if (!other.CompareTag(GlobalConstants.Tag.DOOR)) return;
        Door door = CacheCollider<Door>.GetCollider(other);   
        if (blockedDoors.Contains(door))
        {
            isBlockedByDoor=true;
            Debug.Log("Blocked");
            return;
        }
        blockedDoors.Add(door);
        ChangePlatform(other);
    }

    protected virtual void ChangePlatform(Collider other)
    {
        ++currentPlatform;
        LevelManager.Ins.EnableLevelPlatform(currentPlatform, (int)ColorByEnum);
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
        score += 4;
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
