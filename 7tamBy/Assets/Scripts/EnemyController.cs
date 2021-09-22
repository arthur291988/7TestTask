using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform enemyTransform;

    private const float baseTurnPointX = -9.6f;//-9.8f;
    private const float baseTurnPointY = -4.2f;
    private const float XShiftGap = 0.3f;
    private const float baseXDistance = 2.2f;
    private const float baseYDistance = 1.9f;
    private const float gapOfTurnPoint = 0.2f;

    private List<Vector2> turnPoints;
    private List<Vector2> XBorderPoints;
    private List<Vector2> YBorderPoints;

    private List<Vector2> nextPointsList;

    private Vector2 verticalMovementDirection;
    private Vector2 horizontalMovementDirection;

    private Vector2 previousPoint;
    private Vector2 currentPoint;
    private Vector2 nextPoint;
    private int indexOfCurrentPoint;

    private const float moveSpeed = 0.03f;

    public GameObject testSquare;
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
                    if (i==0||i==4) YBorderPoints.Add(new Vector2(Xpoint + baseXDistance, Ypoint));
                    Xpoint += baseXDistance;
                }

                if (j == 8)
                {
                    XBorderPoints.Add(new Vector2(Xpoint, Ypoint));
                }
            }
        }
        //foreach (Vector2 v in YBorderPoints)
        //{
        //    Instantiate(testSquare, new Vector3(v.x, v.y, 0), Quaternion.identity);
        //}
        //foreach (Vector2 v in XBorderPoints)
        //{
        //    Instantiate(testSquare, new Vector3(v.x, v.y, 0), Quaternion.identity);
        //}
        //foreach (Vector2 v in turnPoints)
        //{
        //    Instantiate(testSquare, new Vector3(v.x, v.y, 0), Quaternion.identity);
        //}
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
    private void assignUpAndDownMoveVectors()
    {
        horizontalMovementDirection = turnPoints[1] - turnPoints[0];
        verticalMovementDirection = turnPoints[9] - turnPoints[0];
    }

    

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

    private void enemyMoveController()
    {
        populateRandomeNextMovePoints();
        nextPoint = nextPointsList[Random.Range(0, nextPointsList.Count)];
        if (previousPoint == nextPoint && Random.Range(0,3)>0)
        {
            nextPointsList.Clear();
            enemyMoveController();
            return;
        }
        previousPoint = currentPoint;
        nextPointsList.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        turnPoints = new List<Vector2>();
        YBorderPoints = new List<Vector2>();
        XBorderPoints = new List<Vector2>();
        nextPointsList = new List<Vector2>();
        enemyTransform = transform;
        populatingTheListOfTurnPointsAndBorderPoints();
        currentPoint = turnPoints[Random.Range(0, turnPoints.Count)];
        enemyTransform.position = currentPoint;
        //assignUpAndDownMoveVectors();
        enemyMoveController();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

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
