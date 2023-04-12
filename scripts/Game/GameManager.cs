using System.Collections;
using System.Collections.Generic;


using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    //设置为单例模式
    public static GameManager Instance;

    public bool isGameStart { get; set; }
    public bool isGameOver { get; set; }

    public bool isGamePause { get; set; }


    private int gameScore;

    private GameData data;

    private ManagerFile file;

    public int GetGameScore()
    {
        return gameScore;
    }
    private int gameDiamond;

    public int getDiamond()
    {
        return gameDiamond;
    }
    private void AddDiamond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamond, gameDiamond);
    }

    private void Awake()
    {
        Instance = this;

        //data = new GameData();
        file = ManagerFile.GetManagerFile();

        EventCenter.AddListener(EventDefine.AddScore, AddScore);
        EventCenter.AddListener(EventDefine.PlayerMove, isPLayMove);
        EventCenter.AddListener(EventDefine.getDiamond, AddDiamond);

        //判断是否重新加载
        if (GameData.isAgainGame)
        {
            isGameStart = true;
        }

        InitGameData();
    }
    public bool isPlaymove;
    private void isPLayMove()
    {
        isPlaymove = true;
    }
    private void AddScore()
    {
        if (isGameOver || isGameStart == false || isGamePause) return;
        gameScore++;
        EventCenter.Broadcast(EventDefine.UpdateScoreText, gameScore);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, isPLayMove);
        EventCenter.RemoveListener(EventDefine.getDiamond, AddDiamond);
    }

    private bool isFirstGame;
    private bool isMusicON;
    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            //创建写入流
            //使用using可以自动释放流 否则需要使用 Close 关闭流
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetdiamondCount(diamondCount);
                data.SetbestScoreArr(bestScoreArr);
                data.SetIsFirstGame(isFirstGame);
                data.SetisMusicON(isMusicON);
                data.SetselectSkin(selectSkin);
                data.SetskinUnlocked(skinUnlocked);

                bf.Serialize(fs, data);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);

        }
    }
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void InitGameData()
    {
        Read();
        if (data != null)
        {
            isFirstGame = data.GetIsFirstGame();
        }
        else
        {
            isFirstGame = true;
        }
        //如果第一次开始游戏
        if (isFirstGame)
        {
            isFirstGame = false;
            isMusicON = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[file.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;
            data = new GameData();

            Save();
        }
        else
        {
            isMusicON = data.GetisMusicON();
            bestScoreArr = data.GetbestScoreArr();
            selectSkin = data.GetselectSkin();
            skinUnlocked = data.GetskinUnlocked();
            diamondCount = data.GetdiamondCount();
        }
    }

    //获取皮肤解锁信息
    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }
    //设置皮肤解锁
    public void SetSkinUnlocked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }
    //获取所有的钻石数量
    public int GetAllDiamond()
    {
        return diamondCount;
    }
    //购买钻石后减少钻石
    public void SetAllDiamond(int score)
    {
        diamondCount += score;
        Save();
    }
    //设置当前选中皮肤的方法
    public void SetSelectedSkin(int index)
    {
        selectSkin = index;
        Save();
    }
    //获取当前选择的皮肤
    public int GetCurrentSelectedSkin()
    {
        return selectSkin;
    }
    public void SaveScore(int score)
    {
        if(score > bestScoreArr[0])
        {
            int temp1 = bestScoreArr[0];
            bestScoreArr[0] = score;
            int temp2 = bestScoreArr[1];
            bestScoreArr[1] = temp1;
            int temp3 = bestScoreArr[2];
            bestScoreArr[2] = temp2;
        }else if(score > bestScoreArr[1])
        {
            int temp1 = bestScoreArr[1];
            bestScoreArr[1] = score;
            bestScoreArr[2] = temp1;
        }else if(score > bestScoreArr[2])
        {
            bestScoreArr[2] = score;
        }
        Save();
    }
    public int GetBestScore()
    {
        return bestScoreArr[0];
    }
}
