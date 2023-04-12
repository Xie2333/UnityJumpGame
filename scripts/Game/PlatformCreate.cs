using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlatformGroupType
{
    Grass,
    Winter
}
public class PlatformCreate : MonoBehaviour
{
    public Vector3 startSpawnPOs;

    //获得资源管理类
    private ManagerFile file;

    //生成平台数量
    private int spawnPlatformCount = 5;
    //平台生成位置
    private Vector3 platformSpawnPosition;
    //判断反向
    private bool isLeftSpawn = false;

    private bool spikeSpawnLeft = false;

    //钉子平台的位置
    private Vector3 spikeDirPlatformPos;
    //钉子平台生成的数量
    private int afterSpawnSpikeSpawnCount = 5;
    //
    private bool isSpike;

    private void Awake()
    {
        //添加监听，当玩家点击移动时，生成路径
        EventCenter.AddListener(EventDefine.CreatePlatform,DecidePath);

        file = ManagerFile.GetManagerFile();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.CreatePlatform, DecidePath);
    }
    private void Start()
    {
        RandomPlatformTheme();
        platformSpawnPosition = startSpawnPOs;
        for(int i = 0; i < 5; i++)
        {
            DecidePath();
        }

        //生成人物
        GameObject go = GameObject.Instantiate(file.PlaygameObject);
        go.transform.position = new Vector3(0, -1.8f, 0);

    }
    //里程碑数
    public int milestoneCount = 10;
    //掉落时间
    public float fallTime;
    public float minFallTime;
    public float multipleFallTime;
    //平台掉落时间
    private void UpdateFallTime()
    {
        if(GameManager.Instance.GetGameScore() > milestoneCount)
        {
            milestoneCount *= 2;
            fallTime *= multipleFallTime;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
    private void Update()
    {
        if (GameManager.Instance.isGameStart && GameManager.Instance.isGameOver == false
            &&GameManager.Instance.isGamePause== false
            )
        {
            UpdateFallTime();
        }
        

    }


    //随机平台主题
    private Sprite selectPlatformSprite;
    private PlatformGroupType groupType;
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(1,file.platformThemeSpriteList.Count);
        selectPlatformSprite = file.platformThemeSpriteList[ran];
        if(ran == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }

    }

    //确定路径
    private void DecidePath()
    {
        if (isSpike)
        {
            SpawnSpickCreate();
            return;
        }
        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }

    //生成平台
    private void SpawnPlatform()
    {
        int ranDir = Random.Range(0, 2);
        if (spawnPlatformCount >= 1)
        {
            //生成单个平台
            SpawnNormalPlatform(ranDir);
        }else if(spawnPlatformCount == 0)
        {
            //生成组合平台
            int ran = Random.Range(0, 3);
            if(ran == 0)
            {
                //生成通用组合平台
                SpawnCommonPlatformGroup(ranDir);
            }
            else if (ran == 1)
            {
                //生成主题组合平台
                switch (groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(ranDir);
                        break; 
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranDir);
                        break;
                    default: break;
                }
            }
            else
            {
                //生成钉子组合平台
                //判断当前方向
                int value = -1;
                if(isLeftSpawn)
                {
                    //生成右边方向
                    value = 0;
                }
                else
                {
                    //生成左边反向的
                    value = 1;
                }
                SpawnSpikePlatformGroup(value);

                isSpike= true;

                afterSpawnSpikeSpawnCount = 4;

                if (spikeSpawnLeft)
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x - 1.65f,
                        platformSpawnPosition.y + file.nextYPos, 0
                        );
                }
                else
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x + 1.65f,
                        platformSpawnPosition.y + file.nextYPos, 0
                        );
                }
            }
        }
        int ranDiamond = Random.Range(0, 10);
        if(ranDiamond == 6 && GameManager.Instance.isPlaymove)
        {
            GameObject go = ObjectPool.instance.GetDiamond();
            go.transform.position = new Vector3(platformSpawnPosition.x,platformSpawnPosition.y + 0.5f, 0);
            go.SetActive(true);
        }

        //判断当前方向
        if(isLeftSpawn)
        {
            platformSpawnPosition.x -= file.nextXPos;
        }
        else
        {
            platformSpawnPosition.x += file.nextXPos;
        }
        platformSpawnPosition.y += file.nextYPos;
    }

    //生成普通平台
    private void SpawnNormalPlatform(int randir)
    {
        GameObject go = ObjectPool.instance.GetNormalPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite,fallTime ,randir);
        go.SetActive(true);
    }
    //生成通用组合平台;
    private void SpawnCommonPlatformGroup(int randir)
    {
        GameObject go = ObjectPool.instance.GetCommonPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite, fallTime, randir);
        go.SetActive(true);
    }
    //生成草地组合平台;
    private void SpawnGrassPlatformGroup(int randir)
    {
        GameObject go = ObjectPool.instance.GetGrassPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite, fallTime, randir);
        go.SetActive(true);
    }
    //生成冬季组合平台;
    private void SpawnWinterPlatformGroup(int randir)
    {
        GameObject go = ObjectPool.instance.GetWinterPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite, fallTime, randir);
        go.SetActive(true);
    }
    //生成钉子组合平台;
    private void SpawnSpikePlatformGroup(int dir)
    {
        GameObject go = null;
        if (dir == 0)
        {
            spikeSpawnLeft = false;
            go = ObjectPool.instance.GetRightSpikelPlatform();
        }
        else{
            spikeSpawnLeft = true;
            go = ObjectPool.instance.GetLeftSpikelPlatform();
        }
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite, fallTime, dir);
        go.SetActive(true);
    }
    //生成钉子后续平台方法，以及后续平台生成的反向
    private void SpawnSpickCreate()
    {
        if (afterSpawnSpikeSpawnCount > 0)
        {
            afterSpawnSpikeSpawnCount--;
            for (int i = 0; i < 2; i++){

                GameObject temp = ObjectPool.instance.GetNormalPlatform();
                if (i == 0)//生成原来方向的平台
                {
                    temp.transform.position = platformSpawnPosition;
                    //如果钉子在左边
                    if (spikeSpawnLeft)
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + file.nextXPos,
                            platformSpawnPosition.y + file.nextYPos, 0
                            );

                    }
                    else//如果钉子在右边
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - file.nextXPos,
                            platformSpawnPosition.y + file.nextYPos, 0
                            );
                    }
                }
                else//生成钉子方向平台
                {
                    temp.transform.position = spikeDirPlatformPos;
                    if (spikeSpawnLeft)
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x - file.nextXPos,
                            spikeDirPlatformPos.y + file.nextYPos, 0
                            );
                    }
                    else
                    {                    
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + file.nextXPos,
                            spikeDirPlatformPos.y + file.nextYPos, 0
                            );
                    }
                }
                temp.GetComponent<PlatformScript>().init(selectPlatformSprite, fallTime, 1);
                temp.SetActive(true);
            }
        }
        else
        {
            isSpike = false;
            DecidePath();
        }
    }
}
