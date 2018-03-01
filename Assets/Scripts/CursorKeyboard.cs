using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorKeyboard : MonoBehaviour {

    [SerializeField]
    Texture2D m_Cursor;

    int m_CursorSizeX = 32;
    int m_CursorSizeY = 32;

    [SerializeField]
    float m_CursorSpeed = 1f;

    int m_CursorPosX = 0;
    int m_CursorPosY = 0;



    // Use this for initialization
    void Start () {
        //Screen.fullScreen = true;
        Cursor.visible = false;

        m_CursorPosY = Screen.height / 2;
        m_CursorPosX = Screen.width / 2;
    }
	
	// Update is called once per frame
	void Update () {
        float hor = Input.GetAxis("TargetHorizontal");
        float ver = Input.GetAxis("TargetVertical");

        m_CursorPosX += Mathf.RoundToInt(hor * m_CursorSpeed);
        m_CursorPosX = Mathf.Min(Mathf.Max(m_CursorPosX, 0), Screen.width);

        m_CursorPosY += Mathf.RoundToInt(ver * m_CursorSpeed);
        m_CursorPosY = Mathf.Min(Mathf.Max(m_CursorPosY, 0), Screen.height);


    }



    private void OnGUI()
    {
        Rect cursorDrawBox = new Rect(m_CursorPosX - (m_CursorSizeX / 2), m_CursorPosY - (m_CursorSizeY / 2), m_CursorSizeX, m_CursorSizeY);
        GUI.DrawTexture(cursorDrawBox, m_Cursor);
    }

    public Vector3 GetCursorPosition()
    {
        return new Vector3(m_CursorPosX,Screen.height - m_CursorPosY, 0);
    }
}
