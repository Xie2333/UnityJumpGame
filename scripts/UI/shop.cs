using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class shop : MonoBehaviour
{
    private ManagerFile file;
    private Transform parent;
    private Text txt_Name;
    private Text txt_Diamond;

    private Button btn_back;

    private Button btn_Select;
    private Button btn_Buy;

    private int currentIndex;

    private void Awake()
    {
        gameObject.SetActive(false);

        parent = transform.Find("ScrollRect/Parent");
        txt_Name = transform.Find("Text").GetComponent<Text>();
        btn_back = transform.Find("back").GetComponent<Button>();
        //注册返回按钮
        btn_back.onClick.AddListener(OnBackButtonClick);

        txt_Diamond = transform.Find("DM/Text_Diamand").GetComponent<Text>();

        btn_Select = transform.Find("btn_Select").GetComponent<Button>();
        //选择按钮注册
        btn_Select.onClick.AddListener(OnSelectButtonClick);

        btn_Buy = transform.Find("btn_Buy").GetComponent<Button>();
        //注册购买按钮
        btn_Buy.onClick.AddListener(OnBuyBUttonClick);

        EventCenter.AddListener(EventDefine.showShop, show);

        file = ManagerFile.GetManagerFile();
        
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.showShop, show);
    }
    private void Start()
    {
        Init();
    }
    //返回按钮
    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.showMain);
        gameObject.SetActive(false);
    }
    //购买按钮点击
    private void OnBuyBUttonClick()
    {
        //获取价格
        int price = int.Parse(btn_Buy.GetComponentInChildren<Text>().text);
        if(price > GameManager.Instance.GetAllDiamond())
        {
            EventCenter.Broadcast(EventDefine.Hint, "钻石不足");
            return;
        }
        GameManager.Instance.SetAllDiamond(-price);
        GameManager.Instance.SetSkinUnlocked(currentIndex);
        parent.GetChild(currentIndex).GetChild(0).GetComponent<Image>().color = Color.white;
    }

    private void Init()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((file.skinSpriteList.Count + 2) * 160, 302);
        for(int i = 0; i < file.skinSpriteList.Count; i++)
        {
            GameObject go = GameObject.Instantiate(file.skinShop, parent);

            //判断当前是否有解锁皮肤
            if (GameManager.Instance.GetSkinUnlocked(i) == false)
            {
                //将子物体颜色设置为灰色
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
        

            go.GetComponentInChildren<Image>().sprite = file.skinSpriteList[i];
            go.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);


            //设置皮肤显示
            parent.transform.localPosition = new Vector3(
                GameManager.Instance.GetCurrentSelectedSkin() * -160, 0, 0);
        }
    }
    private void Update()
    {
        currentIndex = (int)Mathf.Round(parent.transform.localPosition.x / -160);

        if (Input.GetMouseButtonUp(0))
        {
            parent.transform.DOLocalMoveX(currentIndex * -160, 0.2f);

            //parent.transform.localPosition = new Vector3(currentIndex * -160, 0);
        }


        SetItemSize(currentIndex);
        RefreshUI(currentIndex);
    }
    private void SetItemSize(int index)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            if(index == i)
            {
                
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }
    private void RefreshUI(int selectIndex)
    {
        txt_Name.text = file.skinNameList[selectIndex];
        txt_Diamond.text = GameManager.Instance.GetAllDiamond().ToString();

        //未解锁
        if (GameManager.Instance.GetSkinUnlocked(selectIndex) == false)
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            btn_Buy.GetComponentInChildren<Text>().text = file.skinPrice[selectIndex].ToString();

        }
        else
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);
        }
    }


    private void show()
    {
        print("???");
        gameObject.SetActive(true);
    }
    //选择按钮点击
    private void OnSelectButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ChangeSkin, currentIndex);
        GameManager.Instance.SetSelectedSkin(currentIndex);
        EventCenter.Broadcast(EventDefine.showMain);
        gameObject.SetActive(false);
    }
}
