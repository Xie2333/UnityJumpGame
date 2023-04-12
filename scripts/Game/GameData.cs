using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static bool isAgainGame = false;

    private bool isFirstGame;

    private bool isMusicON;

    private int[] bestScoreArr;

    private int selectSkin;

    private bool[] skinUnlocked;

    private int diamondCount;

    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }
    public void SetisMusicON(bool isMusicON)
    {
        this.isMusicON = isMusicON;
    }
    public void SetbestScoreArr(int[] bestScoreArr)
    {
        this.bestScoreArr = bestScoreArr;
    }
    public void SetselectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }
    public void SetskinUnlocked(bool[] skinUnlocked)
    {
        this.skinUnlocked = skinUnlocked;
    }
    public void SetdiamondCount(int diamondCount)
    {
        this.diamondCount = diamondCount;
    }

    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }
    public bool GetisMusicON()
    {
        return isMusicON;
    }
    public int[] GetbestScoreArr()
    {
        return bestScoreArr;
    }
    public int GetselectSkin()
    {
        return selectSkin;
    }
    public bool[] GetskinUnlocked()
    {
        return skinUnlocked;
    }
    public int GetdiamondCount()
    {
        return diamondCount;
    }

}
