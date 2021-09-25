using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public Transform enemyTransform;

    private const float baseTurnPointX = -9.6f;//-9.8f;
    private const float baseTurnPointY = -4.2f;
    private const float XShiftGap = 0.3f;
    private const float baseXDistance = 2.2f;
    private const float baseYDistance = 1.9f;
    private const float frozenTimeFloat = 9;

    private SpriteRenderer enemySpriteRenderer;

    [HideInInspector]
    public List<Vector2> turnPoints;
    private List<Vector2> XBorderPoints;
    private List<Vector2> YBorderPoints;

    private List<Vector2> nextPointsList;

    private Vector2 previousPoint;
    [HideInInspector]
    public Vector2 currentPoint;
    [HideInInspector]
    public Vector2 nextPoint;
    private int indexOfCurrentPoint;

    [SerializeField]
    private GameController gameController;

    [HideInInspector]
    public bool isFarmer;
    [HideInInspector]
    public bool isFrozen;

    [HideInInspector]
    public SpriteRendererController enemySpriteController;

    private float moveSpeed = 0.03f;
    private bool isAngry;
    private string moveDirection;

    private Transform pigTransform;
    private void populatingTheListOfTurnPointsAndBorderPoints()
    {
        float XangleShift = XShiftGap; //используется для сдвига по X при переходе на уровень выше
        float Ypoint = 0;
        float Xpoint = 0;
        for (int i = 0; i < 5; i++)
        {
            Xpoint = baseTurnPointX;
            if (i == 0)
            {
                Ypoint = baseTurnPointY;
            }
            else
            {
                Ypoint = turnPoints[turnPoints.Count - 1].y + baseYDistance;
            }
            for (int j = 0; j < 9; j++)
            {
                if (j == 0 && i == 0)
                {
                    turnPoints.Add(new Vector2(baseTurnPointX, baseTurnPointY));
                    XBorderPoints.Add(new Vector2(baseTurnPointX, baseTurnPointY));
                    YBorderPoints.Add(new Vector2(baseTurnPointX, baseTurnPointY));
                }
                else if (j == 0)
                {
                    Xpoint += XangleShift;
                    turnPoints.Add(new Vector2(Xpoint, Ypoint));
                    XBorderPoints.Add(new Vector2(Xpoint, Ypoint));
                    XangleShift += XShiftGap;
                }
                else
                {
                    turnPoints.Add(new Vector2(Xpoint + baseXDistance, Ypoint));
                    if (i == 0 || i == 4) YBorderPoints.Add(new Vector2(Xpoint + baseXDistance, Ypoint));
                    Xpoint += baseXDistance;
                }

                if (j == 8)
                {
                    XBorderPoints.Add(new Vector2(Xpoint, Ypoint));
                }
            }
        }
    }
    private bool checkIfEnemyStaysAtXBorder(Vector2 currentPos)
    {
        for (int i = 0; i < XBorderPoints.Count; i++) {
            if (currentPos.x == XBorderPoints[i].x) return true;
        }
        return false;
    }
    private bool checkIfEnemyStaysAtYBorder(Vector2 currentPos)
    {
        for (int i = 0; i < YBorderPoints.Count; i++)
        {
            if (currentPos.y == YBorderPoints[i].y) return true;
        }
        return false;
    }

    [SerializeField]
    AudioSource enemySound;

    private void populateRandomeNextMovePoints() {
        if (checkIfEnemyStaysAtXBorder(currentPoint) && checkIfEnemyStaysAtYBorder(currentPoint)) {
            if (currentPoint.x < 0) {
                if (currentPoint.y < 0) {
                    nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 9]);
                    nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 1]);
                }
                if (currentPoint.y > 0)
                {
                    nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 9]);
                    nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 1]);
                }
            }
            if (currentPoint.x > 0)
            {
                if (currentPoint.y < 0)
                {
                    nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 9]);
                    nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 1]);
                }
                if (currentPoint.y > 0)
                {
                    nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 9]);
                    nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 1]);
                }
            }
        }
        else if (checkIfEnemyStaysAtXBorder(currentPoint))
        {
            nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 9]);
            nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 9]);
            if (currentPoint.x < 0)
            {
                nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 1]);
            }
            if (currentPoint.x > 0)
            {
                nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 1]);
            }
        }
        else if (checkIfEnemyStaysAtYBorder(currentPoint))
        {
            nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 1]);
            nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 1]);
            if (currentPoint.y < 0)
            {
                nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 9]);
            }
            if (currentPoint.y > 0)
            {
                nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 9]);
            }
        }
        else
        {
            nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 1]);
            nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 1]);
            nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) + 9]);
            nextPointsList.Add(turnPoints[turnPoints.IndexOf(currentPoint) - 9]);
        }
    }

    private string enemyMoveDirection(Vector2 currentPoint, Vector2 nextPoint) {
        if (currentPoint.x + 1 < nextPoint.x) moveDirection = "Right";
        else if (currentPoint.x + 1 > nextPoint.x) moveDirection = "Left";
        if (currentPoint.y < nextPoint.y) moveDirection = "Up";
        else if (currentPoint.y > nextPoint.y) moveDirection = "Down";
        return moveDirection;
    }

    public void enemyMoveController()
    {
        populateRandomeNextMovePoints();
        nextPoint = nextPointsList[Random.Range(0, nextPointsList.Count)];
        if (previousPoint == nextPoint && Random.Range(0, 3) > 0)
        {
            nextPointsList.Clear();
            enemyMoveController();
            return;
        }
        if (isAngry) enemySpriteController.ChengeTheSpriteFromEnemyController(enemyMoveDirection(currentPoint, nextPoint) + "Angry");
        else enemySpriteController.ChengeTheSpriteFromEnemyController(enemyMoveDirection(currentPoint,nextPoint));
        previousPoint = currentPoint;
        nextPointsList.Clear();
    }

    private void assigningOrderInLyerToEnemy()
    {
        if (enemyTransform.position.y < -3.1f) enemySpriteRenderer.sortingOrder = 8;
        else if (enemyTransform.position.y < -1.2f) enemySpriteRenderer.sortingOrder = 6;
        else if (enemyTransform.position.y < 0.7f) enemySpriteRenderer.sortingOrder = 4;
        else if (enemyTransform.position.y < 2.6f) enemySpriteRenderer.sortingOrder = 2;
        else if (enemyTransform.position.y > 2.6f) enemySpriteRenderer.sortingOrder = 0;
    }

    IEnumerator angryModeController() {
        isAngry = true;
        moveSpeed = 0.06f;
        enemySound.Play();
        enemySpriteController.ChengeTheSpriteFromEnemyController(enemyMoveDirection(currentPoint, nextPoint) + "Angry");
        yield return new WaitForSeconds(4);
        moveSpeed = 0.03f;
        isAngry = false;

    }

    public void invokeFrozenTime()
    {
        if (isFarmer)
        {
            gameController.FarmerIsFrozen = true;
            isFrozen = true;
            enemySpriteController.ChengeTheSpriteFromEnemyController(enemyMoveDirection(currentPoint, nextPoint) + "Dirty");
            Invoke("frozenTime", frozenTimeFloat);
        }
        else
        {
            Invoke("frozenTime", frozenTimeFloat);
            gameController.DogIsFrozen = true;
            isFrozen = true;
            enemySpriteController.ChengeTheSpriteFromEnemyController(enemyMoveDirection(currentPoint, nextPoint) + "Dirty");
        }
    }

    private void frozenTime()
    {
        this.enabled = true;
        if (isFarmer) gameController.FarmerIsFrozen = false;
        else gameController.DogIsFrozen = false;
        isFrozen = false;
        enemySpriteController.ChengeTheSpriteFromEnemyController(enemyMoveDirection(currentPoint, nextPoint));
    }

    private void OnDisable()
    {
        //enemySpriteRenderer.color = Color.white;
        moveSpeed = 0.03f;
        isAngry = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        isFrozen = false;
        isFarmer = name.Contains("Farmer");
        enemySpriteController = GetComponent<SpriteRendererController>();
        moveDirection = "Left";
        isAngry = false;
        pigTransform = FindObjectOfType<PigController>().transform;
        turnPoints = new List<Vector2>();
        YBorderPoints = new List<Vector2>();
        XBorderPoints = new List<Vector2>();
        nextPointsList = new List<Vector2>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyTransform = transform;
        populatingTheListOfTurnPointsAndBorderPoints();
        currentPoint = turnPoints[Random.Range(0, turnPoints.Count)];
        enemyTransform.position = currentPoint;
        enemyMoveController();
    }

    //Update is called once per frame
    void Update()
    {
        assigningOrderInLyerToEnemy();
        if ((pigTransform.position - enemyTransform.position).sqrMagnitude < 4 && !isAngry) {
            StartCoroutine(angryModeController());
        }
    }

    private void FixedUpdate()
    {
        if (currentPoint != nextPoint)
        {
            enemyTransform.position = Vector2.MoveTowards(currentPoint, nextPoint, moveSpeed);
            currentPoint = enemyTransform.position;
        }
        else enemyMoveController();
    }
}
