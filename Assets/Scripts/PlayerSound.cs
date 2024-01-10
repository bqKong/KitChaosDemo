using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimeMax = .1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        //相当于每隔0.1秒播一次
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimeMax;

            if (player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootstepSound(transform.position, volume);
            }
        }

    }

}
