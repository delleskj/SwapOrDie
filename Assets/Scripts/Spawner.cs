using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Spawner : MonoBehaviour {

   
    private float m_SpawnTime = 1.5f;
    private SpriteRenderer m_Render;
    private Color m_Color;
    private Color m_StartColor;
    private float m_TimeTillSpawn = 1.5f;

    private bool m_Enable = false;


	// Use this for initialization
	void Start () {
        m_Render = GetComponent<SpriteRenderer>();
	}
	

    public void StartSpawn(float delay, Color color)
    {
        m_SpawnTime = delay;
        m_Color = color;
        m_StartColor = color;
        m_StartColor.a = 0f;

        m_TimeTillSpawn = 0f;

        m_Enable = true;
    }

    void Update()
    {
        if (!m_Enable) return;

        m_TimeTillSpawn += Time.deltaTime;

        Color c = Color.Lerp(m_StartColor, m_Color, m_TimeTillSpawn / m_SpawnTime);

        m_Render.color = c;
        
   
        if(m_TimeTillSpawn >= m_SpawnTime)
        {
            Destroy(gameObject);
        }           
            
    }
}
