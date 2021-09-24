using System.Collections;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    ParticleSystem effectOfObject;
    ParticleSystem effectOfObjectChild;

    // Start is called before the first frame update
    //void Start()
    //{
    //    effectOfObject = GetComponent<ParticleSystem>();
    //    effectOfObjectChild = GetComponentInChildren<ParticleSystem>();
    //}

    private void Awake()
    {
        effectOfObject = GetComponent<ParticleSystem>();
        effectOfObjectChild = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        effectOfObject.Play();
        effectOfObjectChild.Play();
    }

    private IEnumerator turnOffTheObject() {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
