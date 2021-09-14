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

    // Start is called before the first frame update
    void Start()
    {
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
            transform.Translate(new Vector2(joystickToControl.Horizontal * 0.1f, 0), Space.World);
            if (joystickToControl.Horizontal > 0) pigSpriteController.ChengeTheSpriteFromPigController(Right);
            else if (joystickToControl.Horizontal < 0) pigSpriteController.ChengeTheSpriteFromPigController(Left);
        }
        else if (Mathf.Abs(joystickToControl.Horizontal) < Mathf.Abs(joystickToControl.Vertical))
        {
            transform.Translate(new Vector2(0, joystickToControl.Vertical * 0.1f), Space.World);
            if (joystickToControl.Vertical > 0) pigSpriteController.ChengeTheSpriteFromPigController(Up);
            else if (joystickToControl.Vertical < 0) pigSpriteController.ChengeTheSpriteFromPigController(Down);
        }
    }
}
