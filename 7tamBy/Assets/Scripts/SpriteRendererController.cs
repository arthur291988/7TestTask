
using UnityEngine;
using UnityEngine.U2D;

enum SpriteType { Left, Right, Up, Down}

public class SpriteRendererController : MonoBehaviour
{
    //public SpriteType currentSprite;
    [SerializeField]
    private SpriteAtlas spriteAtlas;

    public SpriteRenderer spriteRenderer;
    private string lastSprite;

    private string Left = "Left";
    private string Right = "Right";
    private string Up = "Up";
    private string Down = "Down";
    private string LeftAngry = "LeftAngry";
    private string RightAngry = "RightAngry";
    private string UpAngry = "UpAngry";
    private string DownAngry = "DownAngry";
    private string LeftDirty = "LeftDirty";
    private string RightDirty = "RightDirty";
    private string UpDirty = "UpDirty";
    private string DownDirty = "DownDirty";

    public void ChengeTheSpriteFromEnemyController(string direction)
    {
        if (lastSprite != direction)
        {
            spriteRenderer.sprite = spriteAtlas.GetSprite(direction);
            lastSprite = direction;
        }
    }

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
}
