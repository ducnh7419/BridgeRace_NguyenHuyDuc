using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : UICanvas
{
    [SerializeField]Button pauseButton;
    // Start is called before the first frame update
    void Start()
    {
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    
    private void OnPauseButtonClicked(){
        Time.timeScale = 0;
        UIManager.Ins.OpenUI<PauseMenu>();
    }
    
}
