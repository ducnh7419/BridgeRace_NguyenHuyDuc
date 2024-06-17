using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToGoalState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.MoveToGoal();
    }

    public void OnExecute(Bot bot)
    {
        Debug.Log("Moving");
        if(!bot.IsMovable){
            bot.StopMoving();
            bot.ChangeState(new PatrolState());
            return;
        }
       
    }

    public void OnExit(Bot bot)
    {

    }

}
