using System.Collections;
using System.Collections.Generic;
using System.Data;
using GlobalEnum;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    public ColorData ColorData;
    public CameraFollow cameraFollow;
    public DynamicJoystick joystick;
    [SerializeField] private List<Level> levels;
    private Level generatedMap;
    private int currentLevel = 0;
    public int Rank = 0;

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

    }

    /// <summary>
    /// Generate new level
    /// </summary>
    public void GenerateLevel()
    {
        DestroyLevel();
        Rank=0;
        currentLevel = UserDataManager.Ins.CurrentLevel;
        generatedMap = Instantiate(levels[currentLevel - 1]);
    }


    /// <summary>
    /// Destroy current level
    /// </summary>
    public void DestroyLevel()
    {
        if (generatedMap == null)
        {
            return;
        }
        Destroy(generatedMap.gameObject);
        SimplePool.CollectAll();
        Platform.ClearPlatformBrickList();
        UIManager.Ins.CloseUI<IngameUI>();
    }


    /// <summary>
    /// Move camera to target position
    /// </summary>
    /// <param name="target"></param>
    public void ChangeCameraSpotlight(Transform target){
        cameraFollow.Target=target;
    }

    /// <summary>
    /// Get the position of each place 
    /// </summary>
    /// <returns></returns>
    public Transform GetPodiumPlace()
    {
        return levels[currentLevel - 1].PodiumPlace[Rank++];
    }

    

}
