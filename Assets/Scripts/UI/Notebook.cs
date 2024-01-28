using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    [SerializeField] private List<NotebookPage> pages;
    private int currentPage = 0;
    public static Notebook instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        foreach (var page in pages)
        {
            page.DisplayRecipes(index);
            if (index != 0) page.gameObject.SetActive(false);
            index += page.getSize();
        }
    }

    public void NextPage()
    {
        if (currentPage == pages.Count-1) return;
        pages[currentPage].gameObject.SetActive(false);
        currentPage++;
        pages[currentPage].gameObject.SetActive(true);
    }
    
    public void PreviousPage()
    {
        if (currentPage == 0) return;
        pages[currentPage].gameObject.SetActive(false);
        currentPage--;
        pages[currentPage].gameObject.SetActive(true);
    }

}
