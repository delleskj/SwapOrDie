using UnityEngine;
using System.Collections;

/// <summary>
/// a simple cooldown class
/// (lazy updating the cooldown so it doesnt need an update() function)
/// </summary>
public class Cooldown {

    private float m_Cooldown = 0f;
    private float m_CooldownStartTime = 0f;

    /// <summary>
    /// creates a cooldown class with the given cooldown time in seconds
    /// after creating the cooldown will need the given time to be ready for the first time
    /// </summary>
    /// <param name="cooldownTime"></param>
    public Cooldown(float cooldownTime)
    {
        m_Cooldown = cooldownTime;
        m_CooldownStartTime = Time.time;
    }

    /// <summary>
    /// checks if the cooldown timer has passed.
    /// if restart is true the cooldown restarts if it was ready (returned true)
    /// </summary>
    /// <returns></returns>
    public bool IsCooldownReady(bool restart = false)
    {
        float cooldownDelta = Time.time - m_CooldownStartTime;
        if (cooldownDelta >= m_Cooldown)
        {
            if (restart) RestartCooldown();
            return true;
        }

        return false;
    }

    /// <summary>
    /// returns how much time the cooldown still needs
    /// </summary>
    /// <returns></returns>
    public float GetTimeTillReady()
    {
        float cooldownDelta = Time.time - m_CooldownStartTime;
        if (cooldownDelta >= m_Cooldown)
        {
            cooldownDelta = 0f;
        }
        else
        {
            cooldownDelta = m_Cooldown - cooldownDelta;
        }

        return cooldownDelta;
    }

    /// <summary>
    /// manually restarts the cooldown
    /// </summary>
    public void RestartCooldown()
    {
        m_CooldownStartTime = Time.time;
    }

}
