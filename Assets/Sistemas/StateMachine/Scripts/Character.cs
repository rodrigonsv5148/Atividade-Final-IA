using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Image image;

    public Animator Animator => animator;
    public Image Image => image;

    void OnValidate()
    {
        animator = GetComponent<Animator>();
        image = GetComponentInChildren<Image>();
    }
}
