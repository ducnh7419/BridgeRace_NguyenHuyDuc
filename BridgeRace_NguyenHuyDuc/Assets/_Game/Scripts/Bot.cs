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
    public Vector3 destination;

    //property tra ve ket qua xem la da toi diem muc tieu hay chua
    public bool IsReachingDestination => Vector3.Distance(TF.position, destination + (TF.position.y - destination.y) * Vector3.up) < 0.2f;


    public List<Brick> Targets { get => targets; set => targets = value; }

    public void ChangeState(IState<Bot> state)
    {
        currentState?.OnExit(this);

        currentState = state;

        currentState?.OnEnter(this);
    }


    protected override void OnInit()
    {
        base.OnInit();
        targets = new();
        Agent.speed = speed * Time.fixedDeltaTime;
        Agent.Warp(spawnLocation);
        ChangeState(new IdleState());

    }

    private void Start()
    {
        OnInit();
    }

    public void UntargetAll()
    {
        targets.Clear();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        direct = Agent.velocity.z > 0 ? Direct.Forward : Direct.Backward;
        currentState?.OnExecute(this);
    }

    //set diem den
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        Agent.SetDestination(destination);
    }


    protected override void ChangePlatform(Collider other)
    {
        base.ChangePlatform(other);
        if (currentPlatform < LevelManager.Ins.GetNumberOfPlatform() - 1)
        {
            UntargetAll();
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new MovingToGoalState());
        }
    }

    /// <summary>
    /// Get randoms target from list targets(Bricks)
    /// </summary>
    public void SetRandomTarget()
    {
        if (targets.Count <= 0) return;
        int rd = Random.Range(0, targets.Count);
        Brick target = targets[rd];
        destination = targets[rd].transform.position;
        SetDestination(destination);
    }

    /// <summary>
    /// Get bricks have the same color
    /// </summary>
    public void GetPLatformBrick()
    {
        List<Brick> bricks = LevelManager.Ins.GetPLatformBricks(currentPlatform);
        for (int i = 0; i < bricks.Count; i++)
        {
            if (ColorByEnum == bricks[i].ColorByEnum)
            {
                targets.Add(bricks[i]);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void StopMoving()
    {
        // StopMoving();
        base.StopMoving();
        Agent.velocity = Vector3.zero;

    }

    public IEnumerator DelayClearDest()
    {
        yield return new WaitForSeconds(1f);
        ClearDestination();
    }

    /// <summary>
    /// Set bot destination to Goal
    /// </summary>
    public void MoveToGoal()
    {
        Transform goal = LevelManager.Ins.GetGoal();
        destination = goal.position;
        SetDestination(destination);
    }

    public void ClearDestination()
    {
        destination = Vector3.zero;
        Agent.ResetPath();
    }

    protected override void AwardPrize()
    {
        base.AwardPrize();
        ChangeState(new CelebrateState());
        if (rank> 0 && rank <= 3)
        {
            Transform place = GetPodiumPlace();
            Agent.Warp(place.position);
            TF.rotation = Quaternion.Euler(Vector3.up * 180);
        }
    }


}
