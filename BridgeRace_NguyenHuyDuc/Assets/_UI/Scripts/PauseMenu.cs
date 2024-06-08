using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : UICanvas
{
    [SerializeField]private Button resumeButton;
    [SerializeField]private Button retryButton;
    [SerializeField]private Button mainMenuButton;

    private void Start() {
        retryButton.onClick.AddListener(RetryButtonOnClicked);
        mainMenuButton.onClick.AddListener(MainMenuButtonOnClicked);
        resumeButton.onClick.AddListener(ResumeButtonOnClicked);
    }

    public void ResumeButtonOnClicked()
    {
        Time.timeScale=1;
        Close(0);
    }

    public void MainMenuButtonOnClicked()
    {
        GameManager.Ins.ChangeState(GameManager.State.MainMenu);
        LevelManager.Ins.DestroyLevel();
        Close(0);
    }

    public void RetryButtonOnClicked()
    {
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        Close(0);
    }
}
