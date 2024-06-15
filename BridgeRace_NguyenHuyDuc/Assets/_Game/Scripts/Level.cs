using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    public ColorData ColorData;
    [SerializeField] private int level;
    private List<int> objectColors;
    [SerializeField] private Transform characterSpawnLocation;
    [SerializeField] private List<Transform> brickSpawnLocation;
    public List<Transform> PodiumPlace;
    [SerializeField] private Brick brick;
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private int NumbersOfBot = 3;
    [SerializeField] private Transform goal;

    private void Awake()
    {
        objectColors = RandomColor();
    }

    private void Start()
    {
        GenerateLevel(level);
    }

    //Generate List where index 0 is color of players and Others is color of bots
    private List<int> RandomColor()
    {
        List<Material> colorsMaterial = ColorData.colorsMaterial;
        List<int> objectColors = new List<int>();
        int playerColor = Random.Range(1, colorsMaterial.Count);
        objectColors.Add(playerColor);
        for (int i = 0; i < NumbersOfBot; i++)
        {
            while (true)
            {
                int rdn = Random.Range(1, colorsMaterial.Count);
                if (!objectColors.Contains(rdn))
                {
                    objectColors.Add(rdn);
                    break;
                }
            }
        }
        return objectColors;
    }

    /// <summary>
    /// Method to generate character
    /// </summary>
    /// <param name="offset">Khoang cach giua cac ng choi </param>
    public void GenerateCharacter(float offset)
    {
        Player playerGO = SimplePool.Spawn<Player>(PoolType.Player, characterSpawnLocation.position, player.transform.rotation);
        playerGO.SetCharacterColor(objectColors[0]);
        characterSpawnLocation.position += new Vector3(offset, 0, 0);
        LevelManager.Ins.cameraFollow.Target = playerGO.transform;
        for (int i = 1; i < objectColors.Count; i++)
        {
            Bot botGO = SimplePool.Spawn<Bot>(PoolType.Bot, characterSpawnLocation.position, bot.transform.rotation);
            botGO.SetCharacterColor(objectColors[i]);
            botGO.Goal = goal;
            characterSpawnLocation.position += new Vector3(offset, 0, 0);
        }
    }

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
    /// Method to generate platform brick
    /// </summary>
    /// <param name="width">Numbers of horizontal brick</param>
    /// <param name="height">Numbers of Vertical brick</param>
    /// <param name="brickSpawnLocation"></param>
    /// <param name="platformBrick">Where to store created platform brick</param>
    /// <param name="offset">distance between bricks</param>
    public void GenerateBrick(int width, int height, Transform brickSpawnLocation, ref List<Brick> platformBrick,float offset)
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
                if (HasGround(pos)&&!HasWall(pos))
                {
                    Brick brickGO = SimplePool.Spawn<Brick>(PoolType.Brick, pos, brick.transform.rotation);
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
                    platformBrick.Add(brickGO);
                }
                x += offset;
            }
            x = brickSpawnLocation.position.x;
            z += offset;
        }
    }

    /// <summary>
    /// Check if current postion have ground
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool HasGround(Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, .8f,1<<7))
        {
            return true;
        }
        return false;
    }

    private bool HasWall(Vector3 position){
         if (Physics.Raycast(position, Vector3.up, out RaycastHit hit, 1f))
        {
            return true;
        }
        return false;
    }
    public void GenerateLevel(int level)
    {
        
        switch (level)
        {
            case 1:
                GenerateCharacter(10f);
                GenerateBrick(10, 10, brickSpawnLocation[0], ref Platform.platformBrick1,5f);
                GenerateBrick(8, 5, brickSpawnLocation[1], ref Platform.platformBrick2,5f);
                break;
            case 2:
                GenerateCharacter(3f);
                GenerateBrick(4, 20, brickSpawnLocation[0], ref Platform.platformBrick1,3.5f);
                GenerateBrick(10, 20, brickSpawnLocation[1], ref Platform.platformBrick2,3.5f);
                break;
            case 3:
                GenerateCharacter(10f);
                GenerateBrick(5, 5, brickSpawnLocation[0], ref Platform.platformBrick1,7f);
                GenerateBrick(10, 5, brickSpawnLocation[1], ref Platform.platformBrick2,5f);
                break;
        }
        SimplePool.Collect(PoolType.Brick);
        Platform.EnableAllPlatformBrick(Platform.platformBrick1);
    }

}
