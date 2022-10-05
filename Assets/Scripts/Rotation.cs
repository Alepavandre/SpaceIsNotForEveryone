using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speed = 1f;
    public Material material;
    public float delay = 0.1f;

    private void Start()
    {
        //StartCoroutine("SkyRotator");
    }
    // Update is called once per frame
    void Update()
    {
        int a = Random.Range(5, 10);
        int b = Random.Range(5, 10);
        transform.Rotate(new Vector3(Mathf.Sin(Time.deltaTime + a) / b, 45, Mathf.Cos(Time.deltaTime + b) / a) * Time.deltaTime * speed);
    }

    private IEnumerator SkyRotator()
    {
        int n = 0;
        while (true)
        {
            n++;
            if (n > 360)
            {
                n = 0;
            }
            material.SetInt("_Rotation", n);
            yield return new WaitForSeconds(delay);
        }
    }
}
