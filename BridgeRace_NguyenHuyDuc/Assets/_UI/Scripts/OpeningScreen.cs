using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningScreen : UICanvas
{
    [SerializeField] private Button playButton;
    private void Start(){
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        GameManager.Ins.ChangeState(GameManager.State.MainMenu);
        Close(0);
    }
}
