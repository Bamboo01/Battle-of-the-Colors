using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacterSoundController : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;
    Vector3 lastPosition;
    bool walkedPlayed = false;
    AudioSource walkSoundSource = null;

    void Start()
    {
        lastPosition = transform.position;
    }

    void OnDisable()
    {
        if (walkSoundSource != null)
        {
            walkSoundSource.Stop();
            walkSoundSource.gameObject.SetActive(false);
            walkSoundSource = null;
        }
    }

    void Update()
    {
        if (lastPosition - transform.position == Vector3.zero || !controller.isGrounded)
        {
            if (walkSoundSource != null)
            {
                walkSoundSource.Stop();
                walkSoundSource.gameObject.SetActive(false);
                walkSoundSource = null;
            }
        }
        else
        {
            if (walkSoundSource == null)
            {
                walkSoundSource = SoundManager.Instance.PlaySoundAtPointByName("ClothRun", transform.position, false);
            }
            walkSoundSource.transform.position = transform.position;
        }
        lastPosition = transform.position;
    }
}
