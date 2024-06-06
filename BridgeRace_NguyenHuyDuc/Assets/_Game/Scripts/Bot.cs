using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private List<Brick> targets=new();
    private IState<Bot> currentState;

    [Header("NavMesh Agent")]
    public NavMeshAgent Agent;
    [SerializeField]private List<Transform> goal;
    private NavMeshSurface navMeshSurface;

    public Vector3 destination;

    //property tra ve ket qua xem la da toi diem muc tieu hay chua
    public bool IsReachingDestination => Vector3.Distance(TF.position, destination + (TF.position.y - destination.y) * Vector3.up) < 0.2f;

    public List<Transform> Goal { get => goal;set => goal = value;  }
    public List<Brick> Targets { get => targets;set=>targets=value; }

    public void ChangeState(IState<Bot> state)
    {
        currentState?.OnExit(this);

        currentState = state;

        currentState?.OnEnter(this);
    }
    
    
    protected override void OnInit()
    {
        base.OnInit();
        Agent.speed=speed*Time.fixedDeltaTime;
        ChangeState(new IdleState());
       

    }

    private void Start()
    {
        OnInit();
    }

    public void UntargetAll(){
        targets.Clear();
        Debug.Log(currentPlatform);
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

    

    public void SetRandomTarget()
    {
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
        Agent.isStopped=true;
    }

    public void MoveToNextGoal(){
        destination=goal[currentPlatform-1].position;
        SetDestination(destination);
        Debug.Log(goal[currentPlatform-1].position);
    }

    public override void Moving()
    {
        base.Moving();    
        Agent.isStopped=false;
    }

    protected override void AwardPrize()
    {
        StopMoving();
        base.AwardPrize();

    }
}
