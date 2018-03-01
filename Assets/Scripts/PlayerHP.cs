using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour {

    [SerializeField]
    private Slider m_HealthSlider;

    [SerializeField]
    private float m_MaxHealth = 100f;
    private float m_CurrentHealth = 100f;




    private void Start()
    {
        if (m_HealthSlider != null)
        {
            m_HealthSlider.maxValue = m_MaxHealth;
        }
    }

    
	
	// Update is called once per frame
	void Update () {
		if(m_CurrentHealth <= 0)
        {
            Debug.Log("player is dead!");
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("restarting game");
            SceneManager.LoadScene(0);
        }
	}

    public void DoDamage(float dmg)
    {
        Debug.Log("player damaged: " + dmg);

        if(m_CurrentHealth > 0)
        {
            m_CurrentHealth -= dmg;

            if (m_HealthSlider != null)
            {
                m_HealthSlider.value = m_CurrentHealth;
            }
        }
    }
}

public enum DamageType
{
    None, Red, Blue, Yellow
}
