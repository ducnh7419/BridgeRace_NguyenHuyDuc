using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public Dictionary<Character,float> goalDistances;

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
        goalDistances=new();

    }

    private void Update(){
        SetRank();
    }

    public int GetNumberOfPlatform(){
        return generatedMap.NumberOfPlatforms;
    }

    /// <summary>
    /// Method to get the distance between character and goal point.Then save it in dict
    /// </summary>
    /// <param name="character"></param>
    /// <param name="goalDistance"></param>
    public void SetGoalDistances(Character character,float goalDistance){
        if(!goalDistances.ContainsKey(character)){
            goalDistances[character]=goalDistance;
        }
    }

    /// <summary>
    /// Generate new level
    /// </summary>
    public void GenerateLevel()
    {
        DestroyLevel();
        currentLevel = UserDataManager.Ins.CurrentLevel;
        generatedMap = Instantiate(levels[currentLevel - 1]);
    }

    public Transform GetGoal(){
        return generatedMap.Goal;
    }

    public void SetRank(){
        if(generatedMap==null) return;
        if(GameManager.Ins.CurrState!=GameManager.State.EndGame||goalDistances.Count<generatedMap.NumbersOfBot) return;
        int rank=2;
        var sortedGoalDistances= goalDistances.OrderBy(x=>x.Value);
        List<Character> characters=sortedGoalDistances.Select(x => x.Key).ToList();
        for(int i=0;i<characters.Count;i++){
            characters[i].Rank=rank;
            rank++;
        }
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
    public Transform GetPodiumPlace(int rank)
    {
        Transform pos= generatedMap.PodiumPlace[rank-1];
        return pos;
    }

    public void EnableLevelPlatform(int platform,int color){
        generatedMap.EnableLevelPlatform(platform,color);
    }

    public List<Brick> GetPLatformBricks(int platform){
        return generatedMap.GetPLatformBricks(platform);
    }

}
