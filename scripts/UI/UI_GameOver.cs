using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_GameOver : MonoBehaviour
{
    public Text text_Score, txt_MaxScore, txt_diamond;
    public Button nextGame, BackGame;

    private void Awake()
    {
        gameObject.SetActive(false);
        EventCenter.AddListener(EventDefine.Gameover,Show);
        //注册按钮点击事件
        nextGame.onClick.AddListener(OnNextButtonclick);
        BackGame.onClick.AddListener(OnBackButtonclick);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.Gameover, Show);

    }

    private void Show()
    {
        if (GameManager.Instance.GetGameScore() > GameManager.Instance.GetBestScore())
        {
            txt_MaxScore.text = "最高分 " + GameManager.Instance.GetGameScore().ToString();
        }
        else
        {
            txt_MaxScore.text = "最高分 " + GameManager.Instance.GetBestScore().ToString();
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());

        text_Score.text = GameManager.Instance.GetGameScore().ToString();
        //最高分显示
        txt_diamond.text = "+"+ GameManager.Instance.getDiamond().ToString();

        GameManager.Instance.SetAllDiamond(GameManager.Instance.getDiamond());

        gameObject.SetActive(true);

    }
    //再来一局
    private void OnNextButtonclick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.isAgainGame = true;

    }
    //退回主界面
    void OnBackButtonclick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.isAgainGame = false;
    }
}
