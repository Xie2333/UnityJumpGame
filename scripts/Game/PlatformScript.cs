using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlatformScript : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;

    public GameObject obstacles;

    private bool startTimer;
    private float fallTime;
    private Rigidbody2D my_body;

    private void Awake()
    {
        my_body = GetComponent<Rigidbody2D>();
    }
    public void init(Sprite sprite,float fallTime,int obstacleDir)
    {
        my_body.bodyType = RigidbodyType2D.Static;
        this.fallTime= fallTime;
        startTimer = true;
        for(int i = 0;i<spriteRenderers.Length;i++)
        {
            spriteRenderers[i].sprite = sprite;
        }

        if(obstacleDir == 0)//³¯ÓÒ±ß
        {
            if (obstacles != null)
            {
                obstacles.transform.localPosition = new Vector3(
                    -obstacles.transform.localPosition.x, obstacles.transform.localPosition.y,
                    obstacles.transform.localPosition.z
                    );
            }
        }
    }
    private void Update()
    {
        if(GameManager.Instance.isGameStart==false || 
            GameManager.Instance.isGamePause
            ) { return; }

        if(!GameManager.Instance.isPlaymove) { return; }
        
        if(startTimer )
        {
            fallTime -= Time.deltaTime;
            if (fallTime < 0)
            {
                //µôÂä
                startTimer = false;
                if (my_body.bodyType != RigidbodyType2D.Dynamic)
                {
                    my_body.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(DealyHide());
                }
            }
        }
        if(transform.position.y-Camera.main.transform.position.y < -6)
        {
            StartCoroutine(DealyHide());
        }
    }
    //¿ªÆôÐ­³ÌÒþ²ØÎïÌå
    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
