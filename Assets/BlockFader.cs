using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFader : MonoBehaviour
{
    public GameObject blockObject;
    public SpriteRenderer renderer;
    public bool fadeOut = false;
    public bool fadeIn = false;
    //float ft = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
       //if(fadeIn == true)
       // {
       //    // StopCoroutine("FadeIn");
       //     StartCoroutine("FadeIn");
            
       // }
       //else if(fadeOut == true)
       // {
       //      Debug.Log("fadeout called");
       //     //StopCoroutine("FadeOut");
       //     StartCoroutine("FadeOut");

       //     //if (ft >= 0)
       //     //{
       //     //    ft -= 0.1f;
       //     //    Debug.Log("ft: " + ft);
       //     //    Color c = renderer.material.color;
       //     //    c.a = ft;
       //     //    renderer.material.color = c;
       //     //}
       //     //else
       //     //{
       //     //    fadeOut = false;
       //     //    enabled = false;
       //     //}

       // }
       
    }

    public void BeginFadeIn()
    {
        enabled = true;
        fadeIn = true;
        
    }
    public void BeginFadeOut()
    {
        
        enabled = true;
        fadeOut = true;

        StopCoroutine("FadeOut");
        StartCoroutine("FadeOut");
        //ft = 1f;
    }
    IEnumerator FadeOut()
    {
     
        fadeOut = false;
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            
            Color c = renderer.material.color;
            c.a = ft;
            renderer.material.color = c;
            
            yield return null;
        }

        blockObject.SetActive(false);
        enabled = false;
    }

    IEnumerator FadeIn()
    {
        fadeIn = false;
        for (float ft = 1f; ft <= 1; ft += 0.1f)
        {
            Color c = renderer.material.color;
            c.a = ft;
            renderer.material.color = c;
            yield return null;
        }
        enabled = false;
    }
}
