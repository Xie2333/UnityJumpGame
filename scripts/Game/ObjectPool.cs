using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //设置为单例模式
    public static ObjectPool instance;


    public int initSpawnCount = 5;

    private List<GameObject> normalPlatformList = new List<GameObject>();
    private List<GameObject> commonPlatformList = new List<GameObject>();
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> winterPlatformList = new List<GameObject>();
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    private List<GameObject> spikePlatformRightList = new List<GameObject>();

    private List<GameObject> deathEffectlist = new List<GameObject>();

    private List<GameObject> DiamondList = new List<GameObject>();

    private ManagerFile file;


    private void Awake()
    {
        instance = this;    
        file = ManagerFile.GetManagerFile();
        Init();
    }
    void Init()
    {
        for(int i = 0; i < initSpawnCount; i++)
        {
            SetList(file.normalPlatform, ref normalPlatformList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for(int j = 0; j < file.commonPlatformGroup.Count; j++)
            {
                SetList(file.commonPlatformGroup[j], ref commonPlatformList);

            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < file.grassPlatformGroup.Count; j++)
            {
                SetList(file.grassPlatformGroup[j], ref grassPlatformList);

            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < file.winterPlatformGroup.Count; j++)
            {
                SetList(file.winterPlatformGroup[j], ref winterPlatformList);

            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            SetList(file.spikePlatformLeft, ref spikePlatformLeftList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            SetList(file.spikePlatformRight, ref spikePlatformRightList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            SetList(file.deathEffect, ref deathEffectlist);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            SetList(file.Diamond, ref DiamondList);
        }
    }
    private GameObject SetList(GameObject prefab,ref List<GameObject> addList)
    {
        GameObject go = GameObject.Instantiate(prefab, transform);
        go.SetActive(false);
        addList.Add(go);
        return go;
    }

    //获取方法
    public GameObject GetNormalPlatform()
    {
        for(int i = 0;i<normalPlatformList.Count;i++)
        {
            if (normalPlatformList[i].activeInHierarchy == false)
            {
                return normalPlatformList[i];
            }
        }
        return SetList(file.normalPlatform, ref normalPlatformList);
    }
    //获得组合平台
    public GameObject GetCommonPlatform()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }
        int ran = Random.Range(0, file.commonPlatformGroup.Count);
        return SetList(file.commonPlatformGroup[ran], ref commonPlatformList);
    }
    public GameObject GetGrassPlatform()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        int ran = Random.Range(0, file.grassPlatformGroup.Count);
        return SetList(file.grassPlatformGroup[ran], ref grassPlatformList);
    }
    public GameObject GetWinterPlatform()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }
        int ran = Random.Range(0, file.winterPlatformGroup.Count);
        return SetList(file.winterPlatformGroup[ran], ref grassPlatformList);
    }
    public GameObject GetLeftSpikelPlatform()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }
        return SetList(file.spikePlatformLeft, ref spikePlatformLeftList);
    }
    public GameObject GetRightSpikelPlatform()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }
        return SetList(file.spikePlatformRight, ref spikePlatformRightList);
    }
    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < deathEffectlist.Count; i++)
        {
            if (deathEffectlist[i].activeInHierarchy == false)
            {
                return deathEffectlist[i];
            }
        }
        return SetList(file.deathEffect, ref deathEffectlist);
    }
    public GameObject GetDiamond()
    {
        for (int i = 0; i < DiamondList.Count; i++)
        {
            if (DiamondList[i].activeInHierarchy == false)
            {
                return DiamondList[i];
            }
        }
        return SetList(file.Diamond, ref DiamondList);
    }
}
