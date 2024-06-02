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
        if (bot.Targets.Count<=0){
            bot.GetPLatformBrick();
        }
        
        
    }

    public void OnExecute(Bot bot)
    {
       bot.StartCoroutine(bot.SetRandomTarget());
    }

    public void OnExit(Bot bot0)
    {

    }

}
