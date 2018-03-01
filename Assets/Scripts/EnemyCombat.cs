using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour {


    [SerializeField]
    private float m_MaxHealth = 100f;
    private float m_CurrentHealth = 100f;

    [SerializeField]
    private Slider m_HealthSlider;

    [SerializeField]
    private float m_AttackRange = 0.5f;

    [SerializeField]
    private float m_AttackCooldown = 1f;
    private Cooldown m_CurrentAttackCD;

    [SerializeField]
    private float m_AttackDamage = 10f;


    [SerializeField]
    private DamageType m_ArmorType = DamageType.Blue;


    private Transform m_Player;

    private AudioSource m_Audio;

    private static float[,] dmgEfficencyTable = new float[3, 3]
    {
        {1f, 0.5f, 2f},
        { 2f, 1f, 0.5f},
        {0.5f, 2f, 1f}
    }; 



    // Use this for initialization
    void Start () {
        m_Audio = GetComponent<AudioSource>();
        m_CurrentAttackCD = new Cooldown(m_AttackCooldown);
        GameObject player = GameObject.Find("Player");
        if(player != null)
        {
            m_Player = player.transform;
        }else
        {
            Debug.LogError("did not find player");
        }

        if (m_HealthSlider != null)
        {
            m_HealthSlider.maxValue = m_MaxHealth;
        }
    }
	
	// Update is called once per frame
	void Update () {


        if(m_CurrentHealth <= 0)
        {
            //delete this if 0 health
            GameObject.Destroy(transform.root.gameObject);
            return;
        }

        //if in range attack player

        if (m_CurrentAttackCD.IsCooldownReady())
        {
            float distToPlayer = (m_Player.position - transform.position).magnitude;
            if (distToPlayer <= m_AttackRange)
            {
                //play audio
                m_Audio.PlayOneShot(m_Audio.clip);
                m_Player.root.SendMessage("DoDamage", m_AttackDamage);
                m_CurrentAttackCD.RestartCooldown();
            }
        }

       
	}

    /// <summary>
    /// does dmg to this entity depending on armor types
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="dmgType"></param>
    public void DoDamage(TypedDamage td)
    {
        if (m_CurrentHealth > 0)
        {
            m_CurrentHealth -= td.dmg * GetDamageTypeFactor(td.dmgType, m_ArmorType);
        }

        Debug.Log("slider val: " + m_HealthSlider.value);

        if (m_HealthSlider != null)
        {
            m_HealthSlider.value = m_CurrentHealth;
        }
    }

    public float GetAttackRange()
    {
        return m_AttackRange;
    }

    /// <summary>
    /// returns the damage efficency of given attack type on given armor type
    /// 
    /// 	Red (armor)	Blue (armor)	Yellow (armor)
    ///red    full       half            double
    ///blue   double     full            half
    ///yellow half       double          full
    /// 
    ///  0 if type none
    /// 
    /// </summary>
    /// <param name="dmgType"></param>
    /// <param name="armorType"></param>
    /// <returns></returns>
    public static float GetDamageTypeFactor(DamageType dmgType, DamageType armorType)
    {
        if (dmgType == DamageType.None || armorType == DamageType.None) return 0f;

        float ret = dmgEfficencyTable[(int)dmgType - 1, (int)armorType - 1];
        Debug.Log("dmg: " + dmgType.ToString() + " armor: " + armorType + " " + ret);
        return ret;
    }
}

public struct TypedDamage
{
    public float dmg;
    public DamageType dmgType;
}
