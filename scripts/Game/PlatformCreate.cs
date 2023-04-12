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

    //�����Դ������
    private ManagerFile file;

    //����ƽ̨����
    private int spawnPlatformCount = 5;
    //ƽ̨����λ��
    private Vector3 platformSpawnPosition;
    //�жϷ���
    private bool isLeftSpawn = false;

    private bool spikeSpawnLeft = false;

    //����ƽ̨��λ��
    private Vector3 spikeDirPlatformPos;
    //����ƽ̨���ɵ�����
    private int afterSpawnSpikeSpawnCount = 5;
    //
    private bool isSpike;

    private void Awake()
    {
        //��Ӽ���������ҵ���ƶ�ʱ������·��
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

        //��������
        GameObject go = GameObject.Instantiate(file.PlaygameObject);
        go.transform.position = new Vector3(0, -1.8f, 0);

    }
    //��̱���
    public int milestoneCount = 10;
    //����ʱ��
    public float fallTime;
    public float minFallTime;
    public float multipleFallTime;
    //ƽ̨����ʱ��
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


    //���ƽ̨����
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

    //ȷ��·��
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

    //����ƽ̨
    private void SpawnPlatform()
    {
        int ranDir = Random.Range(0, 2);
        if (spawnPlatformCount >= 1)
        {
            //���ɵ���ƽ̨
            SpawnNormalPlatform(ranDir);
        }else if(spawnPlatformCount == 0)
        {
            //�������ƽ̨
            int ran = Random.Range(0, 3);
            if(ran == 0)
            {
                //����ͨ�����ƽ̨
                SpawnCommonPlatformGroup(ranDir);
            }
            else if (ran == 1)
            {
                //�����������ƽ̨
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
                //���ɶ������ƽ̨
                //�жϵ�ǰ����
                int value = -1;
                if(isLeftSpawn)
                {
                    //�����ұ߷���
                    value = 0;
                }
                else
                {
                    //������߷����
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

        //�жϵ�ǰ����
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

    //������ͨƽ̨
    private void SpawnNormalPlatform(int randir)
    {
        GameObject go = ObjectPool.instance.GetNormalPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite,fallTime ,randir);
        go.SetActive(true);
    }
    //����ͨ�����ƽ̨;
    private void SpawnCommonPlatformGroup(int randir)
    {
        GameObject go = ObjectPool.instance.GetCommonPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite, fallTime, randir);
        go.SetActive(true);
    }
    //���ɲݵ����ƽ̨;
    private void SpawnGrassPlatformGroup(int randir)
    {
        GameObject go = ObjectPool.instance.GetGrassPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite, fallTime, randir);
        go.SetActive(true);
    }
    //���ɶ������ƽ̨;
    private void SpawnWinterPlatformGroup(int randir)
    {
        GameObject go = ObjectPool.instance.GetWinterPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().init(selectPlatformSprite, fallTime, randir);
        go.SetActive(true);
    }
    //���ɶ������ƽ̨;
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
    //���ɶ��Ӻ���ƽ̨�������Լ�����ƽ̨���ɵķ���
    private void SpawnSpickCreate()
    {
        if (afterSpawnSpikeSpawnCount > 0)
        {
            afterSpawnSpikeSpawnCount--;
            for (int i = 0; i < 2; i++){

                GameObject temp = ObjectPool.instance.GetNormalPlatform();
                if (i == 0)//����ԭ�������ƽ̨
                {
                    temp.transform.position = platformSpawnPosition;
                    //������������
                    if (spikeSpawnLeft)
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + file.nextXPos,
                            platformSpawnPosition.y + file.nextYPos, 0
                            );

                    }
                    else//����������ұ�
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - file.nextXPos,
                            platformSpawnPosition.y + file.nextYPos, 0
                            );
                    }
                }
                else//���ɶ��ӷ���ƽ̨
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
