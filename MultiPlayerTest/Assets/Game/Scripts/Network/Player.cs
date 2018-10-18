using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    private int moveX = 0;
    private int moveY = 0;
    private float moveSpeed = 0.2f;
    private bool isDirty = false;

    private void Start()
    {
        Button[] buttons = GameManager.instance.ButtonsScreen.GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(delegate { CmdPlayVideo_1(); });
        buttons[1].onClick.AddListener(delegate { CmdPlayVideo_2(); });
        buttons[2].onClick.AddListener(delegate { CmdPlayVideo_3(); });


    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // input handling for local player only
        int oldMoveX = moveX;
        int oldMoveY = moveY;

        moveX = 0;
        moveY = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveX -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveX += 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveY += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveY -= 1;
        }
        if (moveX != oldMoveX || moveY != oldMoveY)
        {
            CmdMove(moveX, moveY);
        }
    }

    [Command]
    public void CmdMove(int x, int y)
    {
        //moveX = x;
        //moveY = y;
        //isDirty = true;

        GameManager.instance.PlayVideo_1();
    }

    [Command]
    public void CmdPlayVideo_1()
    {
        GameManager.instance.PlayVideo_1();
    }

    [Command]
    public void CmdPlayVideo_2()
    {
        GameManager.instance.PlayVideo_2();
    }

    [Command]
    public void CmdPlayVideo_3()
    {
        GameManager.instance.PlayVideo_3();
    }

}