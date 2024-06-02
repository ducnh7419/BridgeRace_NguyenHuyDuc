using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrickSearchingArea : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] private Collider areaCollider;
    [SerializeField] Bot bot;
    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Brick")) return;
        Brick brick=Cache.GetBrick(other);
        if(brick.ColorByEnum!=bot.ColorByEnum) return;
        navMeshAgent.SetDestination(brick.transform.position);
        areaCollider.enabled = false;
        if(bot.brickHolder.Count<2){
            StartCoroutine(TurnOnCollider());   
        }
    }

    IEnumerator TurnOnCollider(){
        yield return new WaitForSeconds(2);
        areaCollider.enabled=true;
    }
    
}
