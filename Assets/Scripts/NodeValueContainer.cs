using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeValueContainer : MonoBehaviour
{
    [Header("Point Values")]
    public float speed = 0.1f;

    [Header("Choice Values")]
    public string choiceDescription = "";
    public GameObject choiceRedirect;

    [Header("Animation Values")]
    public AnimationClip anim;
    public float animDuration = 1f;
    public Transform lookAt;
}
