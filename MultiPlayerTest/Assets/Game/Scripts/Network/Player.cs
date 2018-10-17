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
       GameManager.instance.ButtonsScreen.GetComponent<Button>().onClick.AddListener(delegate { Debug.Log("do something");  CmdPlayVideo_1(); });
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

    public void FixedUpdate()
    {
        if (NetworkServer.active)
        {
            transform.Translate(moveX * moveSpeed, moveY * moveSpeed, 0);
        }
    }
}