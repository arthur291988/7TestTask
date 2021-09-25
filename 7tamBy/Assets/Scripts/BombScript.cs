using System.Collections;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private float bombActiveTime;
    private const float bombTime = 10;
    private Collider2D ColliderOfBomb;
    private SpriteRenderer bombSpriteRenderer;
    private Transform bombTransform;

    [SerializeField]
    private GameObject burstEffect;
    [SerializeField]
    private GameObject fadeEffect;
    [SerializeField]
    private GameController gameController;

    PigController pigController;
    EnemyController enemyController;


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
        bombActiveTime = bombTime;
    }

    private void OnDisable()
    {
        bombSpriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        burstEffect.transform.position = bombTransform.position;
        burstEffect.SetActive(true);
        if (collision.CompareTag("Player"))
        {
            pigController = collision.gameObject.GetComponent<PigController>();
            pigController.invokeFrozenTime();
            pigController.enabled = false;
        }
        else
        {
            enemyController = collision.gameObject.GetComponent<EnemyController>();
            if (enemyController.isFarmer) gameController.FarmerIsFrozen = true;
            else gameController.DogIsFrozen = true;
            if (gameController.DogIsFrozen && gameController.FarmerIsFrozen)
            {
                gameController.endOfGamePigWin(); //если оба врага заморожены бомбой то свинья выигрывает 
            }
            else
            {
                enemyController.invokeFrozenTime();
                enemyController.enabled = false;
            }
        }
        gameObject.SetActive(false);
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
