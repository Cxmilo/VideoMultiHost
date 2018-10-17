using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public VideoPlayer player;

    public VideoClip video1;
    public VideoClip video2;
    public VideoClip video3;

    public VideoClip loopVideo;
    public EasyTween mainScreen;
    public EasyTween ButtonsScreen;
    public EasyTween HoldScreen;


    private void Start()
    {
        player.clip = loopVideo;
        player.Play();
    }

    public void PlayVideo_1 ()
    {
        player.gameObject.SetActive(true);
        player.Stop();
        player.clip = video1;
        player.Play();
        Invoke("OnVideoFinish", (float)player.clip.length);
        messageSystem.Instance.RpcShowHoldScreen();
    }

   private void OnVideoFinish ()
    {
        player.Stop();
         messageSystem.Instance.RpcShowMainScreen();
        player.clip = loopVideo;
        player.Play();
        //PushVideoLoop
    }

    public void ShowButtonsScreen ()
    {
        mainScreen.OpenCloseObjectAnimation();
        ButtonsScreen.OpenCloseObjectAnimation();
    }

    public void ShowHoldScreen ()
    {
        ButtonsScreen.OpenCloseObjectAnimation();
        HoldScreen.OpenCloseObjectAnimation();
    }

    public void ShowMainMenu ()
    {
        mainScreen.OpenCloseObjectAnimation();
        HoldScreen.OpenCloseObjectAnimation();
    }

}
