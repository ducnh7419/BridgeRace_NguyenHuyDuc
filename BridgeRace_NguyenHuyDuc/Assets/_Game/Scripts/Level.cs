using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    public ColorData ColorData;
    [SerializeField] private int level;
    [SerializeField] private int numberOfPlatforms;
    public List<Platform> Platforms = new();
    private List<int> objectColors;
    [SerializeField] private Transform characterSpawnLocation;
    [SerializeField] private List<Transform> brickSpawnLocation;
    public List<Transform> PodiumPlace;
    [SerializeField] private Brick brick;
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private int numbersOfBot = 3;
    [SerializeField] private Transform goal;

    public int NumberOfPlatforms => numberOfPlatforms;

    public Transform Goal =>goal;

    public int NumbersOfBot =>numbersOfBot;

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
        playerGO.SpawnLocation=characterSpawnLocation.position;
        characterSpawnLocation.position += new Vector3(offset, 0, 0);
        LevelManager.Ins.cameraFollow.Target = playerGO.transform;
        for (int i = 1; i < objectColors.Count; i++)
        {
            Bot botGO = SimplePool.Spawn<Bot>(PoolType.Bot, characterSpawnLocation.position, bot.transform.rotation);
            botGO.SetCharacterColor(objectColors[i]);
            botGO.SpawnLocation=characterSpawnLocation.position;
            characterSpawnLocation.position += new Vector3(offset, 0, 0);
        }
    }

    /// <summary>
    /// Put Brick into platform
    /// </summary>
    /// <param name="platformSize"></param>
    /// <param name="offsets"></param>
    public void GeneratePlatformBricks(int[,] platformSize, float[] offsets)
    {
        for (int i = 0; i < platformSize.GetLength(0); i++)
        {
            Platform platform = new Platform();
            platform.Width = platformSize[i, 0];
            platform.Height = platformSize[i, 1];
            platform.BrickOffsets = offsets[i];
            platform.GenerateBrick(brickSpawnLocation[i], objectColors);
            Platforms.Add(platform);
        }
    }

    /// <summary>
    /// Call When collide with door to enable bricks with the same color on one platform and disable on other
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="color"></param>
    public void EnableLevelPlatform(int platform, int color)
    {
        if (platform < NumberOfPlatforms-1) Platforms[platform].EnablePlatformBrick(color);
        Platforms[platform - 1].DisablePlatformBrick(color);
    }

    public List<Brick> GetPLatformBricks(int platform)
    {
        return Platforms[platform].PlatformBricks;
    }


    public void GenerateLevel(int level)
    {
        int[,] platformSize;
        float[] offsets;
        switch (level)
        {
            case 1:
                GenerateCharacter(10f);
                platformSize = new int[2, 2] { { 10, 10 }, { 8, 5 } };
                offsets = new float[] { 5f, 5f };
                GeneratePlatformBricks(platformSize, offsets);
                break;
            case 2:
                GenerateCharacter(10f);
                platformSize = new int[2, 2] { { 5, 5 }, { 10, 5 } };
                offsets = new float[] { 7f, 5f };
                GeneratePlatformBricks(platformSize, offsets);
                break;
            case 3:
                GenerateCharacter(10f);
                platformSize = new int[4, 2] { { 5, 5 }, { 10, 5 },{3,3},{4,7} };
                offsets = new float[] { 7f, 5f,10f,3f };
                GeneratePlatformBricks(platformSize, offsets);
                break;
        }
        SimplePool.Collect(PoolType.Brick);
        Platforms[0].EnableAllPlatformBrick();
    }

}
