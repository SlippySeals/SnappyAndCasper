using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetFocus : MonoBehaviour
{
    [SerializeField] private GameObject firstSelection;
    private static EventSystem MyEventSystem => FindObjectOfType<EventSystem>();
    private void OnEnable() => MyEventSystem.SetSelectedGameObject(firstSelection);
}
