using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerSwapTile : MonoBehaviour {

    [SerializeField]
    private float m_SwapCooldown = 3f;
    private Cooldown m_CurrentSwapCooldown;

    [SerializeField]
    private Text m_CooldownTextField;
    private string m_CdTextTemplate = "Swap: ";

    private AudioSource m_Audio;

    [SerializeField]
    private CursorKeyboard m_CursorKeyboard;

	// Use this for initialization
	void Start () {
        m_Audio = GetComponent<AudioSource>();
        m_CurrentSwapCooldown = new Cooldown(m_SwapCooldown);
        if (m_CooldownTextField != null) m_CooldownTextField.text = m_CdTextTemplate + m_SwapCooldown.ToString("0.0");
	}
	
	// Update is called once per frame
	void Update () {

        if (m_CooldownTextField != null) m_CooldownTextField.text = m_CdTextTemplate + m_CurrentSwapCooldown.GetTimeTillReady().ToString("0.0");

        if (m_CurrentSwapCooldown.IsCooldownReady())
        {
            //if (Input.GetMouseButtonDown(0))
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") )
            {
                int tileLayer = LayerMask.GetMask("TileGround");
                GameObject mouseOverTile;
                GameObject playerTile;

                //get mouse over tile
                //Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Ray mouseRay = Camera.main.ScreenPointToRay(m_CursorKeyboard.GetCursorPosition() );
                Debug.DrawRay(mouseRay.origin, mouseRay.direction*100, Color.cyan, 15f);
                RaycastHit mouseHitInfo;
                if(Physics.Raycast(mouseRay,out mouseHitInfo, 100f, tileLayer ))
                {
                    mouseOverTile = mouseHitInfo.collider.gameObject;

                    //get tile currently standing on
                    Ray playerRay = new Ray(transform.position, transform.up*-1);
                    Debug.DrawRay(mouseRay.origin, mouseRay.direction, Color.cyan, 5f);
                    RaycastHit playerHitInfo;
                    if (Physics.Raycast(playerRay, out playerHitInfo, 100f, tileLayer))
                    {
                        playerTile = playerHitInfo.collider.gameObject;

                        //dont trigger cd/ swap when tile is the same
                        if(playerTile == mouseOverTile)
                        {
                            return;
                        }

                        //play audio
                        m_Audio.PlayOneShot(m_Audio.clip);

                        //swap position of tiles
                        Vector3 tmpPos = mouseOverTile.transform.position;
                        mouseOverTile.transform.position = playerTile.transform.position;
                        playerTile.transform.position = tmpPos;

                        //trigger move graph rescan
                        AstarPath.active.Scan();

                        //trigger cooldown
                        m_CurrentSwapCooldown.RestartCooldown();
                    }
                    else
                    {
                        //report error
                        Debug.LogError("Could not get player standing on tile...!");
                    }

                }
                else
                {
                    //TODO send user feedback that nothing was hit
                    Debug.Log("no tile under mouse");
                }



            }
        }
	}
}
