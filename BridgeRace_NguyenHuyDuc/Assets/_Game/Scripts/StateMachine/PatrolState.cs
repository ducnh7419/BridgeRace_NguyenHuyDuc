using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolState : IState<Bot>
{
    private float randomTime;

    private float timer;
    public void OnEnter(Bot bot)
    {
        if (bot.Targets.Count <= 0)
        {
            bot.GetPLatformBrick();
        }
        bot.Moving();
        randomTime=Random.Range(10f,15f);
        bot.SetRandomTarget();
    }

    public void OnExecute(Bot bot)
    {
        bot.Moving();
        timer+=Time.fixedDeltaTime;
        if(!bot.IsReachingDestination) return;         
        if(timer>randomTime){
            bot.ChangeState(new MovingToGoalState());
        }else{
            bot.SetRandomTarget();
        }
    }

    public void OnExit(Bot bot)
    {

    }

}
