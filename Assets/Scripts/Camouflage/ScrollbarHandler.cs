using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarHandler : MonoBehaviour
{
    public Scrollbar scrollBar;
    float size;
    private void Awake()
    {
        scrollBar = GetComponent<Scrollbar>();
    }
    private void Start()
    {
        size = scrollBar.size;
         
    }

    private void Update()
    {
        if (scrollBar.size != size)
        {
            scrollBar.value = 0;
            size = scrollBar.size;
        }
    }

}
