using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu: UICanvas
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button levelSelectionButton;
    private void Start(){
        playButton.onClick.AddListener(OnPlayButtonClicked);
        levelSelectionButton.onClick.AddListener(OnLevelSelectionButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        UserDataManager.Ins.LoadGame();
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        Close(0);
    }

    public void OnLevelSelectionButtonClicked(){
        GameManager.Ins.ChangeState(GameManager.State.LevelSelection);
        Close(0);
    }
}
