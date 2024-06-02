using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    [SerializeField] private int numbersOfBot = 3;
    public ColorData ColorData;
    [SerializeField] private Brick brick;
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private Transform characterSpawnLocation;
    [SerializeField] private Transform brickSpawnLocation1;
    [SerializeField] private Transform brickSpawnLocation2;
    private List<int> objectColors;
    [SerializeField] CameraFollow cameraFollow;
    private int currentPlatform=0;
    [SerializeField]private Transform goalPlatform1;



    public void ChangePlatform(){
        currentPlatform++;
    }

    private void Awake()
    {
        if (ins != null && ins != this)
        {
            Destroy(this);
        }
        else
        {
            ins = this;
        }
        objectColors = RandomColor();
        
    }



    //Generate List where index 0 is color of players and Others is color of bots
    private List<int> RandomColor()
    {
        List<Material> colorsMaterial = ColorData.colorsMaterial;
        List<int> objectColors = new List<int>();
        int playerColor = Random.Range(0, colorsMaterial.Count);
        objectColors.Add(playerColor);
        for (int i = 0; i < numbersOfBot; i++)
        {
            while (true)
            {
                int rdn = Random.Range(0, colorsMaterial.Count);
                if (!objectColors.Contains(rdn))
                {
                    objectColors.Add(rdn);
                    break;
                }
            }
        }
        return objectColors;
    }


    public void GenerateCharacter()
    {
        Player playerGO = SimplePool.Spawn<Player>(player, characterSpawnLocation.position, player.transform.rotation);
        playerGO.SetCharacterColor(objectColors[0]);
        characterSpawnLocation.position += new Vector3(10, 0, 0);
        playerGO.joystick = joystick;
        cameraFollow.Target = playerGO.transform;
        for (int i = 1; i < objectColors.Count; i++)
        {
            Bot botGO = SimplePool.Spawn<Bot>(bot, characterSpawnLocation.position, bot.transform.rotation);
            botGO.SetCharacterColor(objectColors[i]);
            botGO.Goal=goalPlatform1;
            characterSpawnLocation.position += new Vector3(10, 0, 0);
        }
    }

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

    //
    public void GenerateBrick(int width, int height,Transform brickSpawnLocation,ref List<Brick> platformBrick)
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
        brickColors=ShuffleQueue<int>(brickColors);
        float z = brickSpawnLocation.position.z;
        float x = brickSpawnLocation.position.x;
        float y = brickSpawnLocation.position.y;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Brick brickGO = SimplePool.Spawn<Brick>(brick, new Vector3(x, y, z), brick.transform.rotation);
                int color=0;
                if(brickColors.Count > 0){
                    color=brickColors.Dequeue();                               
                    brickGO.SetBrickColor(color);
                }else{
                    color=objectColors[Random.Range(1,objectColors.Count)];
                    brickGO.SetBrickColor(color);
                }
                
                platformBrick.Add(brickGO);
                
                x += 5;
            }
            x = brickSpawnLocation.position.x;
            z += 5;
        }
       

        
        
    }

    public void EnablePlatformBrick(List<Brick> platformBrick,int color){
        for(int i=0;i<platformBrick.Count;i++){
            if(platformBrick[i].ColorByEnum==(ColorByEnum)color){
                SimplePool.Spawn(platformBrick[i]);
            }
        }
    }

    public void DisablePlatformBrick(List<Brick> platformBrick,int color){
        for(int i=0;i<platformBrick.Count;i++){
            if(platformBrick[i].ColorByEnum==(ColorByEnum)color){
                SimplePool.Despawn(platformBrick[i]);
            }
        }
    }

    public void EnableAllPlatformBrick(List<Brick> platformBrick){
        for(int i=0;i<platformBrick.Count;i++){
            SimplePool.Spawn(platformBrick[i]);
        }
    }

    public void GenerateLevel()
    {
        
        GenerateCharacter();         
        GenerateBrick(10, 10,brickSpawnLocation1,ref Platform.platformBrick1);
        GenerateBrick(8, 5,brickSpawnLocation2,ref Platform.platformBrick2);
        SimplePool.Collect(PoolType.Brick);
        EnableAllPlatformBrick(Platform.platformBrick1);
    }



}
