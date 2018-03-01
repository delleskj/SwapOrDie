using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {

    [SerializeField]
    private float m_MoveSpeed = 1f;
   
    private CharacterController m_Charctrl;
    private Animator m_Anim;

    // Use this for initialization
    void Start () {
        m_Charctrl = GetComponent<CharacterController>();
        m_Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //for input feeling change graviy and sensitivity of axis

        Vector3 input = new Vector3(horizontal, 0, vertical);

        float speed = input.magnitude * m_MoveSpeed;
        //m_Anim.SetFloat("Speed", speed);

        m_Charctrl.SimpleMove(input * m_MoveSpeed);
    }
}
