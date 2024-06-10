using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : UICanvas
{
    [SerializeField]Button pauseButton;
    [SerializeField]TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    void Update(){
        score.text=UserDataManager.Ins.Score.ToString();
    }

    
    private void OnPauseButtonClicked(){
        Time.timeScale = 0;
        UIManager.Ins.OpenUI<PauseMenu>();
    }
    
}
