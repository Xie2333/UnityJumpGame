using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    private Button btn_start;
    private Button btn_shop;
    private Button btn_rank;
    private Button btn_sound;

    private ManagerFile file;

    private void show()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.showMain, show);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);


        file = ManagerFile.GetManagerFile();
        //��ʼ��


        Init();
        
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.showMain, show);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }
    //����Ƥ��ͼ��
    private void ChangeSkin(int index)
    {
        btn_shop.transform.GetChild(0).GetComponent<Image>().sprite = file.skinSpriteList[index];
    }

    private void Start()
    {
        if (GameData.isAgainGame)
        {
            EventCenter.Broadcast(EventDefine.ShowGame);
            gameObject.SetActive(false);     
        }

        ChangeSkin(GameManager.Instance.GetCurrentSelectedSkin());
    }

    //��ʼ��
    private void Init()
    {
        //�󶨰�ť
        btn_start = transform.Find("BTN_start").GetComponent<Button>();
        //��Ӽ���,������OnStartButtonClick
        btn_start.onClick.AddListener(OnStartButtonClick);
        //�󶨰�ť
        btn_shop = transform.Find("BTN/BTN_shop").GetComponent<Button>();
        //��Ӽ���
        btn_shop.onClick.AddListener(OnShopButtonClick);
        //�󶨰�ť
        btn_rank = transform.Find("BTN/BTN_rank").GetComponent<Button>();
        //��Ӽ���
        btn_rank.onClick.AddListener(OnRankButtonClick);
        //�󶨰�ť
        btn_sound = transform.Find("BTN/BTN_sound").GetComponent<Button>();
        //��Ӽ���
        btn_sound.onClick.AddListener(OnSoundButtonClick);


    }
    private void OnStartButtonClick()
    {
        GameManager.Instance.isGameStart= true;
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.ShowGame);
    }
    private void OnShopButtonClick()
    {
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.showShop);
    }
    private void OnRankButtonClick()
    {

    }
    private void OnSoundButtonClick()
    {

    }
}
