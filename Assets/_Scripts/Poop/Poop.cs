using System.Collections;
using UnityEngine;

public class Poop : MonoBehaviour
{
    private Animator _anim;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(DestroyAfterTime(30f));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
