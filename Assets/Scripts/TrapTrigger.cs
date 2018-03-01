using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TrapTrigger : MonoBehaviour {

    [SerializeField]
    private DamageType m_TrapDamageType = DamageType.Blue;

    [SerializeField]
    private float m_TrapDamage = 10f;

    private TypedDamage m_DmgMessage = new TypedDamage();

    private AudioSource m_Audio;

    private void Start()
    {
        m_Audio = GetComponent<AudioSource>();
        m_DmgMessage.dmg = m_TrapDamage;
        m_DmgMessage.dmgType = m_TrapDamageType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //play audio
            m_Audio.PlayOneShot(m_Audio.clip);

            other.transform.root.SendMessage("DoDamage", m_DmgMessage);
        }
    }
}
