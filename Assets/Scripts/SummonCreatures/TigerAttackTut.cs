using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class TigerAttackTut : MonoBehaviour
{
    public float speed = 13;
    public float destroyDelay = 1.75f;
    public float erodeRate = 0.03f;
    public float erodeRefreshRate = 0.01f;
    public float erodeDelay = 1.25f;
    public SkinnedMeshRenderer eroderObject;

    private void Start()
    {
        StartCoroutine(ErodeObject());
        
        Destroy(gameObject, destroyDelay);
    }

    IEnumerator ErodeObject()
    {
        yield return new WaitForSeconds(erodeDelay);

        float t = 0;

        while (t < 1)
        {
            t += erodeRate;
            eroderObject.material.SetFloat("_Erode", t);
            yield return new WaitForSeconds(erodeRefreshRate);
        }
    }
}
