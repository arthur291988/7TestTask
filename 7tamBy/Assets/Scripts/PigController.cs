using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : MonoBehaviour
{
    [SerializeField]
    private Joystick joystickToControl;

    private SpriteRendererController pigSpriteController;

    private string Left = "Left";
    private string Right = "Right";
    private string Up = "Up";
    private string Down = "Down";

    private const float baseTurnPointX = -9.8f;
    private const float baseTurnPointY = -4.2f;
    private const float baseXDistance = 2.2f;
    private const float baseYDistance = 1.9f;

    public GameObject testSquare;

    private List<Vector2> turnPoints;

    private Vector2 verticalMovementDirection;
    private Vector2 horizontalMovementDirection;

    private void populatingTheListOfTurnPoints() {
        float XangleShift = 0.4f; //используется для сдвига по X при переходе на уровень выше
        float Ypoint = 0;
        float Xpoint = 0;
        for (int i=0;i<5;i++)
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
            for (int j = 0; j < 9; j++) {
                if (j == 0 && i == 0) turnPoints.Add(new Vector2(baseTurnPointX, baseTurnPointY));
                else if (j == 0)
                {
                    Xpoint += XangleShift;
                    turnPoints.Add(new Vector2(Xpoint, Ypoint));
                    XangleShift += 0.4f;
                }
                else
                {
                    turnPoints.Add(new Vector2(Xpoint + baseXDistance, Ypoint));
                    Xpoint += baseXDistance;
                }
            }
        }

        //foreach (Vector2 v in turnPoints)
        //{
        //    Instantiate(testSquare, new Vector3(v.x, v.y, 0), Quaternion.identity);
        //}
    }

    private void assignUpAndDownMoveVectors() {
        horizontalMovementDirection = turnPoints[1] - turnPoints[0];
        verticalMovementDirection = turnPoints[9] - turnPoints[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        turnPoints = new List<Vector2>();
        populatingTheListOfTurnPoints();
        assignUpAndDownMoveVectors();
        pigSpriteController =GetComponent<SpriteRendererController>();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    private void FixedUpdate()
    {
        if (Mathf.Abs(joystickToControl.Horizontal) > Mathf.Abs(joystickToControl.Vertical))
        {
            //transform.Translate(new Vector2(joystickToControl.Horizontal * 0.1f, 0), Space.World);
            if (joystickToControl.Horizontal > 0) {
                transform.Translate(horizontalMovementDirection * 0.03f, Space.World);
            }
            else if (joystickToControl.Horizontal < 0) transform.Translate(horizontalMovementDirection * -0.03f, Space.World);
            if (joystickToControl.Horizontal > 0) pigSpriteController.ChengeTheSpriteFromPigController(Right);
            else if (joystickToControl.Horizontal < 0) pigSpriteController.ChengeTheSpriteFromPigController(Left);
        }
        else if (Mathf.Abs(joystickToControl.Horizontal) < Mathf.Abs(joystickToControl.Vertical))
        {
            //transform.Translate(new Vector2(0, joystickToControl.Vertical * 0.1f), Space.World);
            if (joystickToControl.Vertical > 0)
            {
                transform.Translate(verticalMovementDirection * 0.03f, Space.World);
            }
            else if (joystickToControl.Vertical < 0) transform.Translate(verticalMovementDirection * -0.03f, Space.World);
            if (joystickToControl.Vertical > 0) pigSpriteController.ChengeTheSpriteFromPigController(Up);
            else if (joystickToControl.Vertical < 0) pigSpriteController.ChengeTheSpriteFromPigController(Down);
        }
    }
}
