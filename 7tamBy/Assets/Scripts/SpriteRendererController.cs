using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

enum SpriteType { Left, Right, Up, Down}

public class SpriteRendererController : MonoBehaviour
{
    //public SpriteType currentSprite;
    [SerializeField]
    private SpriteAtlas spriteAtlas;

    private SpriteRenderer spriteRenderer;
    private string lastSprite;

    private string Left = "Left";
    private string Right = "Right";
    private string Up = "Up";
    private string Down = "Down";

    //private void ChengeTheSprite() {
    //    if (currentSprite!= lastSprite)
    //    {
    //        spriteRenderer.sprite = spriteAtlas.GetSprite(currentSprite.ToString());
    //        lastSprite = currentSprite;
    //    }
    //}
    public void ChengeTheSpriteFromPigController(string direction)
    {
        if (lastSprite != direction)
        {
            spriteRenderer.sprite = spriteAtlas.GetSprite(direction);
            lastSprite = direction;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteAtlas.GetSprite(Left);
        lastSprite = Left;
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    ChengeTheSprite();
    //}
}
