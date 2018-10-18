using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public VideoPlayer player;
    public Texture2D imageHolder;

    public VideoClip video1;
    public VideoClip video2;
    public VideoClip video3;

    public EasyTween mainScreen;
    public EasyTween ButtonsScreen;
    public EasyTween HoldScreen;


    public void PlayVideo_1()
    {
        player.gameObject.SetActive(true);
        player.Stop();
        player.clip = video1;
        player.Play();
        Invoke("OnVideoFinish", (float)player.clip.length);
        messageSystem.Instance.RpcShowHoldScreen();
    }

    public void PlayVideo_2()
    {
        player.gameObject.SetActive(true);
        player.Stop();
        player.clip = video2;
        player.Play();
        Invoke("OnVideoFinish", (float)player.clip.length);
        messageSystem.Instance.RpcShowHoldScreen();
    }

    public void PlayVideo_3()
    {
        player.gameObject.SetActive(true);
        player.Stop();
        player.clip = video3;
        player.Play();
        Invoke("OnVideoFinish", (float)player.clip.length);
        messageSystem.Instance.RpcShowHoldScreen();
    }

    private void OnVideoFinish()
    {
        player.Stop();
        messageSystem.Instance.RpcShowMainScreen();
        player.GetComponent<Renderer>().material.mainTexture = imageHolder;
        //PushVideoLoop
    }

    public void ShowButtonsScreen()
    {
        mainScreen.OpenCloseObjectAnimation();
        ButtonsScreen.OpenCloseObjectAnimation();
    }

    public void ShowHoldScreen()
    {

        if (ButtonsScreen.gameObject.activeInHierarchy)
            ButtonsScreen.OpenCloseObjectAnimation();

        if (mainScreen.gameObject.activeInHierarchy)
            mainScreen.OpenCloseObjectAnimation();

        HoldScreen.OpenCloseObjectAnimation();
    }

    public void ShowMainMenu()
    {
        mainScreen.OpenCloseObjectAnimation();
        HoldScreen.OpenCloseObjectAnimation();
    }

}
