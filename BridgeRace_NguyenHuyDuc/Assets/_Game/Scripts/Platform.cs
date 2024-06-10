using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class Platform : MonoBehaviour
{

   public static List<Brick> platformBrick1 = new();
   public static List<Brick> platformBrick2 = new();

   /// <summary>
   /// Function to Enable bricks which have the same color
   /// </summary>
   /// <param name="platformBrick"></param>
   /// <param name="color"></param>
   public static void EnablePlatformBrick(List<Brick> platformBrick, int color)
   {      
      for (int i = 0; i < platformBrick.Count; i++)
      {
         if (platformBrick[i].ColorByEnum == (ColorByEnum)color)
         {
            
            Brick brickGO=SimplePool.Spawn<Brick>(platformBrick[i],platformBrick[i].transform.position, platformBrick[i].transform.rotation);
            brickGO.SetBrickColor(color);
         }
      }
   }

   /// <summary>
   /// Function to Disable bricks which have the same color
   /// </summary>
   /// <param name="platformBrick"></param>
   /// <param name="color"></param>
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

   /// <summary>
   /// Function to enable all brick in platform
   /// </summary>
   /// <param name="platformBrick"></param>
   public static void EnableAllPlatformBrick(List<Brick> platformBrick)
   {
      for (int i = 0; i < platformBrick.Count; i++)
      {
         Brick brickGO=SimplePool.Spawn<Brick>(platformBrick[i],platformBrick[i].transform.position, platformBrick[i].transform.rotation);
         brickGO.SetBrickColor((int)platformBrick[i].ColorByEnum);
      }
   }

   /// <summary>
   /// Generate bricks on one platform on disable its on these others
   /// </summary>
   /// <param name="platform"></param>
   /// <param name="color"></param>
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

   /// <summary>
   /// Delete all elements in platform brick list
   /// </summary>
   public static void ClearPlatformBrickList(){
      platformBrick1.Clear();
      platformBrick2.Clear();
   }





}
