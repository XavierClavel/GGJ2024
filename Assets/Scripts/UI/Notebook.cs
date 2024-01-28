using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    [SerializeField] private List<NotebookPage> pages;
    private int currentPage = 0;
    public static Notebook instance;
    private static Vector2 posDisplayed = Vector2.zero;
    private static Vector2 posHidden = new Vector2(-1100f, -850f);
    [SerializeField] private RectTransform rectTransform;

    public void Hide()
    {
        rectTransform.DOAnchorPos(posHidden, 1f).SetEase(Ease.InOutQuad);
    }

    public void Show()
    {
        rectTransform.DOAnchorPos(posDisplayed, 1f).SetEase(Ease.InOutQuad);
    }

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
