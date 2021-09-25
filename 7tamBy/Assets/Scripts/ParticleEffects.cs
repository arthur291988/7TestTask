using System.Collections;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    ParticleSystem effectOfObject;
    ParticleSystem effectOfObjectChild;
    [SerializeField]
    AudioSource bombSound;

    private void Awake()
    {
        effectOfObject = GetComponent<ParticleSystem>();
        effectOfObjectChild = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        effectOfObject.Play();
        effectOfObjectChild.Play();
        bombSound.Play();
        StartCoroutine(turnOffTheObject());
    }

    private IEnumerator turnOffTheObject() {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
