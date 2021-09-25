
using UnityEngine;
using UnityEngine.UI;

public class BombButton : MonoBehaviour
{
    [SerializeField]
    private GameObject [] bomb;
    [SerializeField]
    private AudioSource bombSet;
    private Button buttonUI;
    private float reloadTime;
    private Transform pigTransform;

    public void setTheBomb() {
        for (int i = 0; i < bomb.Length; i++)
        {
            if (!bomb[i].activeInHierarchy)
            {
                bombSet.Play();
                bomb[i].transform.position = pigTransform.position;
                bomb[i].SetActive(true);
                reloadTime = 3f;
                buttonUI.interactable = false;
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pigTransform = FindObjectOfType<PigController>().GetComponent<Transform>();
        buttonUI = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
        }
        else if(!buttonUI.interactable) buttonUI.interactable = true;
    }
}
