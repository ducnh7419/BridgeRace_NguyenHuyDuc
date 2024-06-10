using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private List<Brick> targets;
    private IState<Bot> currentState;

    [Header("NavMesh Agent")]
    public NavMeshAgent Agent;
    [SerializeField]private Transform goal;

    public Vector3 destination;

    //property tra ve ket qua xem la da toi diem muc tieu hay chua
    public bool IsReachingDestination => Vector3.Distance(TF.position, destination + (TF.position.y - destination.y) * Vector3.up) < 0.2f;

    
    public List<Brick> Targets { get => targets;set=>targets=value; }
    public Transform Goal { get => goal; set => goal = value; }

    public void ChangeState(IState<Bot> state)
    {
        currentState?.OnExit(this);

        currentState = state;

        currentState?.OnEnter(this);
    }
    
    
    protected override void OnInit()
    {
        base.OnInit();
        targets=new();
        Agent.speed=speed*Time.fixedDeltaTime;
        destination=Vector3.zero;
        ChangeState(new IdleState());
        Agent.enabled=true;
    }

    private void Start()
    {
        OnInit();
    }

    public void UntargetAll(){
        targets.Clear();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        direct=Agent.velocity.z>0?Direct.Forward:Direct.Backward;
        currentState?.OnExecute(this);
    }

     //set diem den
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        Agent.SetDestination(destination);
    }


    protected override void ChangePlatform(Collider other){
        base.ChangePlatform(other);
        UntargetAll();
        ChangeState(new PatrolState());
    }

    public void SetRandomTarget()
    {
        if(targets.Count<=0) return;
        int rd = Random.Range(0, targets.Count);
        Brick target = targets[rd];
        destination=targets[rd].transform.position;
        SetDestination(destination);
    }

    public void GetPLatformBrick()
    {
        List<Brick> bricks = new();
        switch (currentPlatform)
        {
            case 1:
                bricks = Platform.platformBrick1;
                break;
            case 2:
                bricks = Platform.platformBrick2;
                break;
        }
        for (int i = 0; i < bricks.Count; i++)
        {
            if (ColorByEnum == bricks[i].ColorByEnum)
            {
                targets.Add(bricks[i]);
            }
        }
    }

    public override void StopMoving()
    {
        // StopMoving();
        base.StopMoving();
        Agent.velocity=Vector3.zero;
        
    }

    public void DisableAI(){
        Agent.enabled=false;
    }

    public void MoveToGoal(){
        destination=Goal.position;
        SetDestination(destination);
    }

    public void ClearDestination(){
        Agent.ResetPath();
        destination=Vector3.zero;
    }

    public override void Moving()
    {
        base.Moving();    
        
    }

    protected override void AwardPrize()
    {
        ChangeState(new CelebrateState());
        if(LevelManager.Ins.Rank==2){
            LevelManager.Ins.ChangeCameraSpotlight(goal);
            GameManager.Ins.CurrentResult=GameManager.GameResult.Lose;
            StartCoroutine(ChangeGameState());
        }
        base.AwardPrize();
        

    }
}
