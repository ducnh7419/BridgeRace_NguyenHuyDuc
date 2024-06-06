using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class Platform : MonoBehaviour
{

   public static List<Brick> platformBrick1 = new();
   public static List<Brick> platformBrick2 = new();

   public static void EnablePlatformBrick(List<Brick> platformBrick, int color)
   {      
      for (int i = 0; i < platformBrick.Count; i++)
      {
         if (platformBrick[i].ColorByEnum == (ColorByEnum)color)
         {
            
            Brick brickGO=SimplePool.Spawn<Brick>(platformBrick[i],platformBrick[i].transform.position,platformBrick[i].transform.rotation);
            brickGO.SetBrickColor(color);
         }
      }
   }

   public static void DisablePlatformBrick(List<Brick> platformBrick, int color)
   {
      for (int i = 0; i < platformBrick.Count; i++)
      {
         if (platformBrick[i].ColorByEnum == (ColorByEnum)color)
         {
            SimplePool.Despawn(platformBrick[i]);
         }
      }
   }

   public static void EnableAllPlatformBrick(List<Brick> platformBrick)
   {
      for (int i = 0; i < platformBrick.Count; i++)
      {
         SimplePool.Spawn(platformBrick[i]);
      }
   }

   public static void EnablePlatform(int platform,int color){
      switch (platform){
         case 2:
            EnablePlatformBrick(platformBrick2, color);
            DisablePlatformBrick(platformBrick1, color);
            break;
         case 3:
            
            break;
      }
      
   }





}
