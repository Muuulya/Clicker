using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Animator _animator;

    private bool isOpen = false;
    
    private Camera _mainCamera;

    public void OpenAndCloseMenu()
    {
        // _panel.SetActive(true);
        if (!isOpen)
        {
            _animator.SetTrigger("Open");
            isOpen = true;
        }
        else
        {
            _animator.SetTrigger("Close");
            isOpen = false;
        }
    }

    public void CloseMenu()
    {
        // _panel.SetActive(false);
        _animator.SetTrigger("Open");
    }
}
