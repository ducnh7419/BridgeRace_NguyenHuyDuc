using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    public ColorData ColorData;
    [SerializeField]private int level;
    private List<int> objectColors;
    [SerializeField] private Transform characterSpawnLocation;
    [SerializeField] private List<Transform> brickSpawnLocation;
    public List<Transform> PodiumPlace;
    private List<Character> characters;
    [SerializeField] private Brick brick;
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private int NumbersOfBot = 3;
    [SerializeField] private Transform goal;
    public int LowestRank => NumbersOfBot+1;

    private void Awake()
    {
        objectColors = RandomColor();
        characters=new();
    }

    private void Start(){
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

    public void GenerateCharacter()
    {
        Player playerGO = SimplePool.Spawn<Player>(PoolType.Player, characterSpawnLocation.position, player.transform.rotation);
        playerGO.SetCharacterColor(objectColors[0]);
        characters.Add(playerGO);
        characterSpawnLocation.position += new Vector3(10, 0, 0);
        LevelManager.Ins.cameraFollow.Target = playerGO.transform;
        for (int i = 1; i < objectColors.Count; i++)
        {
            Bot botGO = SimplePool.Spawn<Bot>(PoolType.Bot, characterSpawnLocation.position, bot.transform.rotation);
            botGO.SetCharacterColor(objectColors[i]);
            botGO.Goal = goal;
            characters.Add(botGO);
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
    public void GenerateBrick(int width, int height, Transform brickSpawnLocation, ref List<Brick> platformBrick)
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
                Brick brickGO = SimplePool.Spawn<Brick>(PoolType.Brick, new Vector3(x, y, z), brick.transform.rotation);
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

                x += 5;
            }
            x = brickSpawnLocation.position.x;
            z += 5;
        }
    }

    public void GenerateLevel(int level)
    {
        GenerateCharacter();
        switch (level)
        {
            case 1:
                GenerateBrick(10, 10, brickSpawnLocation[0], ref Platform.platformBrick1);
                GenerateBrick(8, 5, brickSpawnLocation[1], ref Platform.platformBrick2);
                break;
            case 2:
                break;
        }
        SimplePool.Collect(PoolType.Brick);
        Platform.EnableAllPlatformBrick(Platform.platformBrick1);
    }

    

}
