using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Podium : GameUnit
{
   [SerializeField] Collider podiumCollider;

   private void OnTriggerEnter(Collider other) {
        if(other.CompareTag(GlobalConstants.Tag.PLAYER)||other.CompareTag(GlobalConstants.Tag.BOT))    
        podiumCollider.enabled = false;
   }
}
