
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characters;

    [SerializeField]
    private GameObject playAgainButton;

    [SerializeField]
    private GameObject quitGameButton;

    [SerializeField]
    private AudioSource pigShot;

    private Image pigPictureOnReplayButton;

    public bool FarmerIsFrozen;
    public bool DogIsFrozen;

    private EnemyController enemyController;

    [SerializeField]
    Sprite pigWin;
    [SerializeField]
    Sprite pigFail;


    public void endOfGamePigWin() {
        foreach (GameObject go in characters)
        {
            go.SetActive(false);
            if (go.GetComponent<EnemyController>())
            {
                enemyController = go.GetComponent<EnemyController>();
                enemyController.currentPoint = enemyController.turnPoints[Random.Range(0, enemyController.turnPoints.Count)];
                enemyController.nextPoint = enemyController.currentPoint;
                enemyController.enemyTransform.position = enemyController.currentPoint;
                enemyController.enemySpriteController.ChengeTheSpriteFromEnemyController("Left");
                enemyController.isFrozen = false;
            }
        }
        pigPictureOnReplayButton.sprite = pigWin;
        playAgainButton.SetActive(true);
    }
    public void endOfGamePigLose()
    {
        foreach (GameObject go in characters) {
            go.SetActive(false);
            if (go.GetComponent<EnemyController>())
            {
                enemyController = go.GetComponent<EnemyController>();
                enemyController.currentPoint = enemyController.turnPoints[Random.Range(0, enemyController.turnPoints.Count)];
                enemyController.nextPoint = enemyController.currentPoint;
                enemyController.enemyTransform.position = enemyController.currentPoint;
                enemyController.enemySpriteController.ChengeTheSpriteFromEnemyController("Left");
                enemyController.isFrozen = false;
                pigShot.Play();
            }
        }
        pigPictureOnReplayButton.sprite = pigFail;
        playAgainButton.SetActive(true);
    }

    public void restartTheGame() {
        FarmerIsFrozen = false;
        DogIsFrozen = false;
        foreach (GameObject go in characters)
        {
            go.SetActive(true);
        }
        playAgainButton.SetActive(false);
    }

    public void quitTheGame() {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        FarmerIsFrozen = false;
        DogIsFrozen = false;
        pigPictureOnReplayButton = playAgainButton.transform.GetChild(1).GetComponent<Image>();
    }

}
