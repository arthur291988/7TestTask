
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

    private const float baseTurnPointX = -9.6f;//-9.8f;
    private const float baseTurnPointY = -4.2f;
    private const float XShiftGap = 0.3f;
    private const float baseXDistance = 2.2f;
    private const float baseYDistance = 1.9f;
    private const float gapOfTurnPoint = 0.4f;

    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private AudioSource pigWalk;


    private List<Vector2> turnPoints;

    private Vector2 verticalMovementDirection;
    private Vector2 horizontalMovementDirection;

    [HideInInspector]
    public Transform pigTransform;

    private bool movingHorizontal;

    private void populatingTheListOfTurnPoints() {
        float XangleShift = XShiftGap; //???????????? ??? ?????? ?? X ??? ???????? ?? ??????? ????
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
                    XangleShift += XShiftGap;
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

    private bool checkIfTurnIsAvailable() {
        for (int i =0;i< turnPoints.Count;i++) {
            if (pigTransform.position.x < turnPoints[i].x + gapOfTurnPoint && pigTransform.position.x > turnPoints[i].x - gapOfTurnPoint 
                && pigTransform.position.y < turnPoints[i].y + gapOfTurnPoint && pigTransform.position.y > turnPoints[i].y - gapOfTurnPoint) {
                return true;
            }
        }
        return false;
    }

    private void assignUpAndDownMoveVectors() {
        horizontalMovementDirection = turnPoints[1] - turnPoints[0];
        verticalMovementDirection = turnPoints[9] - turnPoints[0];
    }

    private void assigningOrderInLyerToPig()
    {
        if (pigTransform.position.y < -3.1f) pigSpriteController.spriteRenderer.sortingOrder = 8;
        else if (pigTransform.position.y < -1.2f) pigSpriteController.spriteRenderer.sortingOrder = 6;
        else if (pigTransform.position.y < 0.7f) pigSpriteController.spriteRenderer.sortingOrder = 4;
        else if (pigTransform.position.y < 2.6f) pigSpriteController.spriteRenderer.sortingOrder = 2;
        else if (pigTransform.position.y > 2.6f) pigSpriteController.spriteRenderer.sortingOrder = 0;
    }

    private bool checkIfPigInsideLevelRect(bool horizontal, bool rightOrUp) {
        if (horizontal && rightOrUp) {
            if (pigSpriteController.spriteRenderer.sortingOrder == 8)
            {
                if (pigTransform.position.x < 8f)
                {
                    return true;
                }
            }
            else if (pigSpriteController.spriteRenderer.sortingOrder == 6)
            {
                if (pigTransform.position.x < 8.3f)
                {
                    return true;
                }
            }
            else if (pigSpriteController.spriteRenderer.sortingOrder == 4)
            {
                if (pigTransform.position.x < 8.5f)
                {
                    return true;
                }
            }
            else if (pigSpriteController.spriteRenderer.sortingOrder == 2)
            {
                if (pigTransform.position.x < 8.8f )
                {
                    return true;
                }
            }
            else if (pigSpriteController.spriteRenderer.sortingOrder == 0)
            {
                if (pigTransform.position.x < 9.2f)
                {
                    return true;
                }
            }
        }
        if (horizontal && !rightOrUp)
        {
            if (pigSpriteController.spriteRenderer.sortingOrder == 8)
            {
                if (pigTransform.position.x > -9.6f)
                {
                    return true;
                }
            }
            else if (pigSpriteController.spriteRenderer.sortingOrder == 6)
            {
                if (pigTransform.position.x > -9.3f)
                {
                    return true;
                }
            }
            else if (pigSpriteController.spriteRenderer.sortingOrder == 4)
            {
                if (pigTransform.position.x > -8.8f)
                {
                    return true;
                }
            }
            else if (pigSpriteController.spriteRenderer.sortingOrder == 2)
            {
                if (pigTransform.position.x > -8.5f)
                {
                    return true;
                }
            }
            else if (pigSpriteController.spriteRenderer.sortingOrder == 0)
            {
                if (pigTransform.position.x > -8.2f)
                {
                    return true;
                }
            }
        }
        if (!horizontal && rightOrUp)
        {
            if (pigTransform.position.y < 3.4f)
            {
                return true;
            }
            
        }
        if (!horizontal && !rightOrUp)
        {
            if (pigTransform.position.y > -4.2f)
            {
                return true;
            }
            
        }
        
        return false;
    }

    public void invokeFrozenTime()
    {
        pigSpriteController.spriteRenderer.color = new Color(0.6f, 0.3f,0.1f,1);
        Invoke("frozenTime", 5);
    }

    private void frozenTime()
    {
        this.enabled = true;
        pigSpriteController.spriteRenderer.color = Color.white;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !collision.gameObject.GetComponent<EnemyController>().isFrozen) {
            gameController.endOfGamePigLose();
            transform.position = turnPoints[Random.Range(0, turnPoints.Count)];
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        movingHorizontal = true;
        pigTransform = transform;
        turnPoints = new List<Vector2>();
        populatingTheListOfTurnPoints();
        assignUpAndDownMoveVectors();
        pigSpriteController = GetComponent<SpriteRendererController>();
    }

    // Update is called once per frame
    void Update()
    {
        assigningOrderInLyerToPig(); 
        if (!movingHorizontal && Mathf.Abs(joystickToControl.Horizontal) > Mathf.Abs(joystickToControl.Vertical) && checkIfTurnIsAvailable())
        {
            movingHorizontal = true;
            if (joystickToControl.Horizontal > 0) pigSpriteController.ChengeTheSpriteFromPigController(Right);
            else if (joystickToControl.Horizontal < 0) pigSpriteController.ChengeTheSpriteFromPigController(Left);
            if (Random.Range(0, 3) == 0) pigWalk.Play();
        }
        else if (movingHorizontal && Mathf.Abs(joystickToControl.Horizontal) < Mathf.Abs(joystickToControl.Vertical) && checkIfTurnIsAvailable())
        {

            movingHorizontal = false;
            if (joystickToControl.Vertical > 0) pigSpriteController.ChengeTheSpriteFromPigController(Up);
            else if (joystickToControl.Vertical < 0) pigSpriteController.ChengeTheSpriteFromPigController(Down);
            if (Random.Range(0, 3) == 0) pigWalk.Play();
        }
    }

    private void FixedUpdate()
    {
        if (movingHorizontal)
        {
            if (joystickToControl.Horizontal > 0.3f && checkIfPigInsideLevelRect(true, true))
            {
                transform.Translate(horizontalMovementDirection * 0.03f, Space.World);
                pigSpriteController.ChengeTheSpriteFromPigController(Right);
            }
            else if (joystickToControl.Horizontal < -0.3f && checkIfPigInsideLevelRect(true, false))
            {
                transform.Translate(horizontalMovementDirection * -0.03f, Space.World);
                pigSpriteController.ChengeTheSpriteFromPigController(Left);
            }
        }
        else
        {
            if (joystickToControl.Vertical > 0.3f && checkIfPigInsideLevelRect(false, true))
            {
                transform.Translate(verticalMovementDirection * 0.03f, Space.World);
                pigSpriteController.ChengeTheSpriteFromPigController(Up);
            }
            else if (joystickToControl.Vertical < -0.3f && checkIfPigInsideLevelRect(false, false))
            {
                transform.Translate(verticalMovementDirection * -0.03f, Space.World);
                pigSpriteController.ChengeTheSpriteFromPigController(Down);
            }
        }

        
    }
}
