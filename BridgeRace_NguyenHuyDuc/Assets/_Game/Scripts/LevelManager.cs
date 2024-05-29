using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins =>ins;
    [SerializeField] private int numbersOfBot = 3;
    public ColorData ColorData;
    [SerializeField] private Brick brick;
    [SerializeField] private Player player;
    [SerializeField] private Bot bot;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Transform characterSpawnLocation;
    [SerializeField] private Transform brickSpawnLocation;
    private List<int> objectColors;
    [SerializeField] CameraFollow cameraFollow;
    


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
        objectColors=RandomColor();
    }

    

    //Generate List where index 0 is color of players and Others is color of bots
    private List<int> RandomColor()
    {
        List<Material> colorsMaterial = ColorData.colorsMaterial;
        List<int> objectColors = new List<int>();
        int playerColor = Random.Range(0, colorsMaterial.Count + 1);
        objectColors.Add(playerColor);
        for (int i = 0; i < numbersOfBot; i++)
        {
            while (true)
            {
                int rdn = Random.Range(0, colorsMaterial.Count + 1);
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
        Debug.Log("Afdgdfgdgd");
        Player playerGO= SimplePool.Spawn<Player>(player,characterSpawnLocation.position,player.transform.rotation);
        playerGO.SetCharacterColor(objectColors[0]);
        characterSpawnLocation.position+=new Vector3(10,0,0);
        playerGO.joystick=joystick;
        cameraFollow.target=playerGO.transform;
        for(int i=1;i<objectColors.Count;i++){
            Bot botGO= SimplePool.Spawn<Bot>(bot,characterSpawnLocation.position,bot.transform.rotation);
            botGO.SetCharacterColor(objectColors[i]);
            characterSpawnLocation.position+=new Vector3(10,0,0);
        }
    }

    //
    public void GenerateBrick(int width, int height){
        float z=brickSpawnLocation.position.z;
        float x=brickSpawnLocation.position.x;
        float y=brickSpawnLocation.position.y;
        for(int i=0;i<width;i++){
            for(int j=0;j<height;j++){
                Brick brickGO=SimplePool.Spawn<Brick>(brick,new Vector3(x,y,z),brick.transform.rotation);
                brickGO.SetBrickColor(objectColors[Random.Range(0,objectColors.Count)]);
                x+=5;
            }
            x=brickSpawnLocation.position.x;
            z+=5;
        }
    }

    public void GenerateLevel(){
        GenerateCharacter();
        GenerateBrick(10,10);
    }



}
