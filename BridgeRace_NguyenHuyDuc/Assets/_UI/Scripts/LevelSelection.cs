using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelection : UICanvas
{
    [SerializeField] private Button jumpBackward;
    [SerializeField] private List<Button> levelButtons;
    

    // Start is called before the first frame update
    private void Start()
    {
        jumpBackward.onClick.AddListener(()=>GameManager.Ins.GoBackward());  
        for (int i = 0;i<levelButtons.Count;i++){
            levelButtons[i].onClick.AddListener(OnLevelButtonClicked);       
        }
        
    }

    private void OnLevelButtonClicked(){
        string clickedButtonName=EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(clickedButtonName);
        UserDataManager.Ins.LoadGame(Convert.ToInt32(clickedButtonName));
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        Close(0);
    }

    
    
}
