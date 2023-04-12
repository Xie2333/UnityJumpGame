using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml.Serialization;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayController : MonoBehaviour
{
    public Transform rayDown,rayLeft,rayRight;


    private bool isLeft = false;
    private ManagerFile file;
    private bool isJump = false;

    private Rigidbody2D Playbody;

    private SpriteRenderer spriteRenderer;

    //����Ƿ��ƶ�
    private bool isMove = false;

    //���߼��Ĳ���
    public LayerMask platformLayer,obstacleLayer;

    public AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        file = ManagerFile.GetManagerFile();
        Playbody = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        //��������Ƥ���¼�
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }
    private void Start()
    {
        ChangeSkin(GameManager.Instance.GetCurrentSelectedSkin());
    }


    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }
    //����Ƥ��
    private void ChangeSkin(int skinIndex)
    {
        spriteRenderer.sprite = file.GameskinSpriteList[skinIndex];
    }

    private void Update()   
    {
        //�ж��Ƿ�����UI,ֱ�ӷ���
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (GameManager.Instance.isGameStart == false ||  
            GameManager.Instance.isGameOver == true||
            GameManager.Instance.isGamePause == true
            ) 
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && isJump==false && nextPlatformLeft != Vector3.zero)
        {
            if(isMove==false) {
                //�㲥����ƶ��¼�
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true; }
            audioSource.PlayOneShot(file.jumpClip);

            isJump = true;
            //�ж������λ��
            Vector3 mousePos = Input.mousePosition;
            //�ж��Ƿ�Ϊ��Ļ��һ��
            if (mousePos.x <= Screen.width / 2)
            {
                isLeft= true;
            }
            else
            {
                isLeft= false;
            }
            Jump();
            EventCenter.Broadcast(EventDefine.CreatePlatform);
        }
        //������������ʱ��
        if (isJump && isRayPlatformLeftOrRight() == true && GameManager.Instance.isGameOver == false)
        {
            audioSource.PlayOneShot(file.hitClip);
            GameObject go = ObjectPool.instance.GetDeathEffect();
            go.SetActive(true);
            go.transform.position = transform.position;
            GameManager.Instance.isGameOver = true;

            spriteRenderer.enabled = false;

            StartCoroutine(DealyshowOver());
        }
        if (Playbody.velocity.y < 0 && isRayPlatform()==false && GameManager.Instance.isGameOver==false)
        {
            audioSource.PlayOneShot(file.fallClip);
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;

            GameManager.Instance.isGameOver = true;
            StartCoroutine(DealyshowOver());

        }
        if(transform.position.y - Camera.main.transform.position.y < -6&&
            GameManager.Instance.isGameOver == false)
        {
            audioSource.PlayOneShot(file.fallClip);

            StartCoroutine(DealyshowOver());

            GameManager.Instance.isGameOver = true;
        }
    }
    //���ý������
    IEnumerator DealyshowOver()
    {
        yield return new WaitForSeconds(1);
        EventCenter.Broadcast(EventDefine.Gameover);
    }

    private GameObject lastHitGo = null;
    //�Ƿ��⵽ƽ̨
    private bool isRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position,Vector2.down, 1f, platformLayer);
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                if(lastHitGo!=hit.collider.gameObject)
                {
                    if (lastHitGo == null)
                    {
                        lastHitGo = hit.collider.gameObject;
                        return true;    
                    }
                    lastHitGo = hit.collider.gameObject;
                    EventCenter.Broadcast(EventDefine.AddScore);
                }
                
                return true;
            }
        }
        return false;   
    }
    //�Ƿ��⵽�ϰ���
    private bool isRayPlatformLeftOrRight()
    {
        RaycastHit2D hitleft = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D hitright = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);
        if (hitleft.collider != null)
        {
            if (hitleft.collider.tag == "obstacle")
            {
                return true;
            }
        }
        if (hitright.collider != null)
        {
            if (hitright.collider.tag == "obstacle")
            {
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        if(isLeft)
        {
            //����������Ҫ����
            transform.localScale = new Vector3(-1,1,1);
            transform.DOMoveX(nextPlatformLeft.x,0.2f);
            transform.DOMoveY(nextPlatformLeft.y+0.8f, 0.15f);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOMoveX(nextPlatformRight.x, 0.2f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
        }
    }
    //��һ��ƽ̨λ��
    private Vector3 nextPlatformLeft, nextPlatformRight;
    //ʹ����ײ���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Platform")
        {
            isJump = false;
            //��ǰƽ̨λ��
            Vector3 newOlatformPos = collision.gameObject.transform.position;
            nextPlatformLeft = new Vector3(newOlatformPos.x - file.nextXPos,
                newOlatformPos.y + file.nextYPos, 0
                );
            nextPlatformRight = new Vector3(newOlatformPos.x + file.nextXPos,
                newOlatformPos.y + file.nextYPos, 0
                );
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "diamond")
        {

            audioSource.PlayOneShot(file.dimondClip);
            EventCenter.Broadcast(EventDefine.getDiamond);
            collision.gameObject.SetActive(false);
            
        }
    }
}
