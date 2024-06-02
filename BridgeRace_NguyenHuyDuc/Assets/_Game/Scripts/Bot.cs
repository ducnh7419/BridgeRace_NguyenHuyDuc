using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private List<Brick> targets;
    private IState<Bot> currentState;
    [SerializeField] int numbersOfRandomTargetBrick = 10;

    [Header("NavMesh Agent")]
    public NavMeshAgent Agent;
    private Transform goal;
    private NavMeshSurface navMeshSurface;

    public Transform Goal { get => goal; set => goal = value; }
    public List<Brick> Targets { get => targets; }

    public void ChangeState(IState<Bot> state)
    {
        currentState?.OnExit(this);

        currentState = state;

        currentState?.OnEnter(this);
    }

    
    protected override void OnInit()
    {
        base.OnInit();
        Agent.speed = speed;
        ChangeState(new IdleState());

    }

    private void Start()
    {
        OnInit();
        Agent.SetDestination(Goal.position);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Debug.Log(isMovable);
        if (!isMovable)
        {
            StopMoving();
            return;
        }
    }

    public IEnumerator SetRandomTarget()
    {
        yield return new WaitForSeconds(1f);
        int rd = Random.Range(0, targets.Count);
        Brick target = targets[rd];
        Agent.SetDestination(target.transform.position);
    }

    public void GetPLatformBrick()
    {
        int currPlatform = Platform.currentPlatform;
        List<Brick> bricks = new List<Brick>();
        switch (currPlatform)
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

    public new void StopMoving()
    {
        // StopMoving();
        Agent.isStopped = true;
    }

}
