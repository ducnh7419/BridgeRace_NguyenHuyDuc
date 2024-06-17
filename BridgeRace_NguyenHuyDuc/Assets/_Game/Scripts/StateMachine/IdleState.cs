using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<Bot>
{
    float timer;
    private float randomTime;
    public void OnEnter(Bot bot)
    {
        bot.StopMoving();
        timer=0;
        randomTime=Random.Range(4f, 6f);
    }

    public void OnExecute(Bot bot)
    {
        timer+=Time.fixedDeltaTime;
        if(timer>randomTime){
            bot.ChangeState(new PatrolState());
            
        }
    }

    public void OnExit(Bot bot)
    {
        
    }

}
