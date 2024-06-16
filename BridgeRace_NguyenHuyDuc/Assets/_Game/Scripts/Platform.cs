using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class Platform
{
   public List<Brick> PlatformBricks = new();
   private float brickOffsets;
   private int width;
   private int height;
   public float BrickOffsets { get => brickOffsets; set => brickOffsets = value; }
   public int Width { get => width; set => width = value; }
   public int Height { get => height; set => height = value; }

   /// <summary>
   /// Shuffle colors queue
   /// </summary>
   /// <param name="queue"></param>
   /// <typeparam name="T"></typeparam>
   /// <returns></returns>
   public Queue<T> ShuffleQueue<T>(Queue<T> queue)
   {
      // Convert queue to a list
      List<T> list = new List<T>(queue);

      // Shuffle the list using Fisher-Yates algorithm
      int n = list.Count;
      System.Random random = new System.Random();
      for (int i = n - 1; i > 0; i--)
      {
         int j = random.Next(0, i + 1);
         T temp = list[i];
         list[i] = list[j];
         list[j] = temp;
      }

      // Convert the list back to a queue
      Queue<T> shuffledQueue = new Queue<T>(list);
      return shuffledQueue;
   }

   /// <summary>
   /// Generate brick on Platform
   /// </summary>
   /// <param name="brickSpawnLocation"></param>
   /// <param name="objectColors"></param>
   public void GenerateBrick(Transform brickSpawnLocation, List<int> objectColors)
   {
      int totalBrick = width * height;
      Queue<int> brickColors = new Queue<int>();

      for (int i = 0; i < objectColors.Count; i++)
      {
         for (int j = 0; j < totalBrick / objectColors.Count; j++)
         {
            brickColors.Enqueue(objectColors[i]);
         }
      }
      brickColors = ShuffleQueue<int>(brickColors);
      float z = brickSpawnLocation.position.z;
      float x = brickSpawnLocation.position.x;
      float y = brickSpawnLocation.position.y;
      for (int i = 0; i < width; i++)
      {
         for (int j = 0; j < height; j++)
         {
            Vector3 pos = new(x, y, z);
            if (HasGround(pos) && !HasWall(pos))
            {
               Brick brickGO = SimplePool.Spawn<Brick>(PoolType.Brick);
               brickGO.TF.position = pos;
               int color = 0;
               if (brickColors.Count > 0)
               {
                  color = brickColors.Dequeue();
                  brickGO.SetBrickColor(color);
               }
               else
               {
                  color = objectColors[Random.Range(1, objectColors.Count)];
                  brickGO.SetBrickColor(color);
               }
               PlatformBricks.Add(brickGO);
            }
            x += brickOffsets;
         }
         x = brickSpawnLocation.position.x;
         z += brickOffsets;
      }
   }

   /// <summary>
   /// Check if current postion have ground
   /// </summary>
   /// <param name="position"></param>
   /// <returns></returns>
   private bool HasGround(Vector3 position)
   {
      if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, .8f, 1 << 7))
      {
         return true;
      }
      return false;
   }

   private bool HasWall(Vector3 position)
   {
      if (Physics.Raycast(position, Vector3.up, out RaycastHit hit, 1f))
      {
         return true;
      }
      return false;
   }

   /// <summary>
   /// Function to Enable bricks which have the same color
   /// </summary>
   /// <param name="color"></param>
   public void EnablePlatformBrick(int color)
   {
      for (int i = 0; i < PlatformBricks.Count; i++)
      {
         if (PlatformBricks[i].ColorByEnum == (ColorByEnum)color)
         {

            Brick brickGO = SimplePool.Spawn<Brick>(PlatformBricks[i], PlatformBricks[i].transform.position, PlatformBricks[i].transform.rotation);
            brickGO.SetBrickColor(color);
            PlatformBricks[i] = brickGO;
         }
      }
   }

   /// <summary>
   /// Function to Disable bricks which have the same color
   /// </summary>
   /// <param name="color"></param>
   public void DisablePlatformBrick(int color)
   {
      for (int i = 0; i < PlatformBricks.Count; i++)
      {
         if (PlatformBricks[i].ColorByEnum == (ColorByEnum)color)
         {
            PlatformBricks[i].Despawn();
         }
      }
   }

   /// <summary>
   /// Function to enable all brick in platform
   /// </summary>
   public void EnableAllPlatformBrick()
   {
      for (int i = 0; i < PlatformBricks.Count; i++)
      {
         Brick brickGO = SimplePool.Spawn<Brick>(PlatformBricks[i], PlatformBricks[i].transform.position, PlatformBricks[i].transform.rotation);
         brickGO.SetBrickColor((int)PlatformBricks[i].ColorByEnum);
         PlatformBricks[i] = brickGO;
      }
   }



   // /// <summary>
   // /// Delete all elements in platform brick list
   // /// </summary>
   // public void ClearPlatformBrickList(){
   //    platformBrick1.Clear();
   //    platformBrick2.Clear();
   // }





}
