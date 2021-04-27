using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFader : MonoBehaviour
{

    public SpriteRenderer renderer;
    float alpha = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        Color c = renderer.material.color;
        c.a = alpha;
        renderer.material.color = c;

       
    }

    public void BeginFade()
    {
        enabled = true;
        StartCoroutine("FadeOut");
    }
    IEnumerator FadeOut()
    {
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
