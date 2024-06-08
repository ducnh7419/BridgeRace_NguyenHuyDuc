using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CelebrateState : IState<Bot>
{
    private float randomTime;

    private float timer;
    public void OnEnter(Bot bot)
    {
        bot.StopMoving();
        bot.ClearDestination();
        bot.DisableAI();
    }

    public void OnExecute(Bot bot)
    {
       
    }

    public void OnExit(Bot bot0)
    {

    }

}
