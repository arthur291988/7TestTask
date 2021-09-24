using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private float bombActiveTime;
    private Collider2D ColliderOfBomb;
    private SpriteRenderer bombSpriteRenderer;
    private Transform bombTransform;

    [SerializeField]
    private GameObject burstEffect;
    [SerializeField]
    private GameObject fadeEffect;

    PigController pigController;
    EnemyController enemyController;

    //Start is called before the first frame update
    //void Start()
    //{
    //}

    private void Awake()
    {
        bombTransform = transform;
        bombSpriteRenderer = GetComponent<SpriteRenderer>();
        bombSpriteRenderer.color = new Color(1, 1, 1, 0.5f);
        ColliderOfBomb = GetComponent<Collider2D>();
    }

    private void assigningOrderInLyerToBomb()
    {
        if (bombTransform.position.y < -3.1f) bombSpriteRenderer.sortingOrder = 8;
        else if (bombTransform.position.y < -1.2f) bombSpriteRenderer.sortingOrder = 6;
        else if (bombTransform.position.y < 0.7f) bombSpriteRenderer.sortingOrder = 4;
        else if (bombTransform.position.y < 2.6f) bombSpriteRenderer.sortingOrder = 2;
        else if (bombTransform.position.y > 2.6f) bombSpriteRenderer.sortingOrder = 0;
    }

    private IEnumerator triggerOfBomb()
    {
        yield return new WaitForSeconds(1.5f);
        bombSpriteRenderer.color = new Color(1, 1, 1, 1);
        ColliderOfBomb.enabled = true;
    }

    private void OnEnable()
    {
        ColliderOfBomb.enabled = false;
        assigningOrderInLyerToBomb();
        StartCoroutine(triggerOfBomb());
        bombActiveTime = 8f;
    }

    private void OnDisable()
    {
        bombSpriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        burstEffect.transform.position = bombTransform.position;
        burstEffect.SetActive(true);
        if (other.CompareTag("Player"))
        {
            pigController = other.gameObject.GetComponent<PigController>();
            StartCoroutine(pigController.frozenTime());
            pigController.enabled = false;
        }
        else
        {
            enemyController = other.gameObject.GetComponent<EnemyController>();
            StartCoroutine(enemyController.frozenTime());
            enemyController.enabled = false;
        }
        Debug.Log("Heeeeey");
    }

    // Update is called once per frame
    void Update()
    {
        if (bombActiveTime > 0)
        {
            bombActiveTime -= Time.deltaTime;
        }
        else {
            gameObject.SetActive(false);
            fadeEffect.transform.position = bombTransform.position;
            fadeEffect.SetActive(true);
        }
    }
}
