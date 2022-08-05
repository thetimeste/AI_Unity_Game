using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    //gene colour
    public float r=0;
    public float g=0;
    public float b=0;
    public int scale=0;
    bool dead = false;
    int trialTime = 10;
    int generation = 1;
    public float deathTime = 0;
    SpriteRenderer personSpriteRenderer;
    Collider2D personCollider;

    private void Awake()
    {
        personSpriteRenderer = GetComponent<SpriteRenderer>();
        personCollider = GetComponent<Collider2D>();
    }

    public void ChooseRandomScale()
    {
        scale = Random.Range(20, 70);
        SetScale();
    }

    public void SetScale()
    {
        gameObject.transform.localScale = new Vector3(scale,scale,1);
    }
    public void ChooseRandomColor()
    {
        r = Random.Range(0.0f, 1.0f);
        g = Random.Range(0.0f, 1.0f);
        b = Random.Range(0.0f, 1.0f);
        SetColor();
    }

    public void SetColor()
    {
        personSpriteRenderer.color = new Color(r, g, b);
    }
    private void OnMouseDown()
    {
        dead = true;
        personSpriteRenderer.enabled = false;
        personCollider.enabled = false;
    }

    private void Update()
    {
        if (!dead)
        {
            deathTime += Time.deltaTime;
        }
       
    }
}
