using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button kickButton;
    [SerializeField] private Button autoKickButton;
    [SerializeField] private Button resetButton;

    public event Action OnKickClicked;
    public event Action OnAutoKickClicked;
    public event Action OnResetClicked;

    private void Awake()
    {
        if (kickButton != null)
        {
            kickButton.gameObject.SetActive(false);
            kickButton.onClick.AddListener(() => OnKickClicked?.Invoke());
        }

        if (autoKickButton != null)
        {
            autoKickButton.onClick.AddListener(() => OnAutoKickClicked?.Invoke());
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(() => OnResetClicked?.Invoke());
        }
    }

    public void SetKickButtonActive(bool active)
    {
        if (kickButton != null && kickButton.gameObject.activeSelf != active)
        {
            kickButton.gameObject.SetActive(active);
        }
    }

    private void OnDestroy()
    {
        if (kickButton != null)
        {
            kickButton.onClick.RemoveAllListeners();
        }

        if (autoKickButton != null)
        {
            autoKickButton.onClick.RemoveAllListeners();
        }

        if (resetButton != null)
        {
            resetButton.onClick.RemoveAllListeners();
        }
    }
}
