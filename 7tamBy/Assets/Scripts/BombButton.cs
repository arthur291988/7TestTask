using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BombButton : MonoBehaviour
{
    [SerializeField]
    private GameObject [] bomb;
    private Button buttonUI;
    private float reloadTime;
    private Transform pigTransform;

    public void setTheBomb() {
        for (int i = 0; i < bomb.Length; i++)
        {
            if (!bomb[i].activeInHierarchy)
            {
                bomb[i].transform.position = pigTransform.position;
                bomb[i].SetActive(true);
                reloadTime = 5f;
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
