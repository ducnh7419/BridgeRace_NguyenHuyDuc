using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingBridge : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.MoveToNextGoal();
    }

    public void OnExecute(Bot bot)
    {
        Debug.Log("Moving");
        if(!bot.IsMovable){
            bot.StopMoving();
            bot.ChangeState(new PatrolState());
            return;
        }
        if(bot.IsReachingDestination){
            Debug.Log("Reach");
            bot.UntargetAll();
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {

    }

}
