using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFader : MonoBehaviour
{

    public SpriteRenderer renderer;
    public bool fadeOut = false;
    public bool fadeIn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
       if(fadeIn == true)
        {
            StartCoroutine("FadeIn");
        }
       else if(fadeOut == true)
        {
            StartCoroutine("FadeOut");
        }
       
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
