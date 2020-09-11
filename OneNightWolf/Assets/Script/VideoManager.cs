using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer video;
    public GameObject canvas;
    public CanvasGroup[] canvasGroups;
    public GameObject title;
    public GameObject masterLoading;
    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        title = GameObject.Find("Title");
        canvas.SetActive(false);
        video.loopPointReached += EndReached;
        canvasGroups = canvas.GetComponentsInChildren<CanvasGroup>();
        sprite = Resources.Load("Menu/masterLoading", typeof(Sprite)) as Sprite;
    }

    int GetTime()
    {
        return (int)(video.frameCount / video.frameRate + 0.5f);
    }

    void SkipVideo()
    {
        video.frame = (long)video.frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown&&video!=null)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                bool pd = false;
                if (keyCode == UnityEngine.KeyCode.Return) pd = true;
                if (keyCode == UnityEngine.KeyCode.Space) pd = true;
                if (pd)
                {
                    SkipVideo();
                }
            }
        }

    }

    void cleanAlpha()
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].alpha = 0.0f;
        }
    }

    void updateAlpha()
    {
        bool ok = true;
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            if (canvasGroups[i].alpha < 1.0f)
            {
                canvasGroups[i].alpha += 0.2f;
                ok = false;
            }
            if (canvasGroups[i].alpha > 1.0f)
            {
                canvasGroups[i].alpha = 1.0f;
            }
        }
        if (ok)
        {
            masterLoading.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
               
            title.SetActive(true);
            this.CancelInvoke();
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        
        vp.clip = null;
        canvas.SetActive(true);
        cleanAlpha();
        title.SetActive(false);
        masterLoading.GetComponentInChildren<SpriteRenderer>().sprite = null;
        this.InvokeRepeating("updateAlpha", 0.0f,0.2f);
    }
}
