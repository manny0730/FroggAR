using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum PlatformType
{
    Normal,
    FinishLine,
    StartLine
}

[RequireComponent(typeof(Collider))]
public class PlatformManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Material m_Material;
    [SerializeField] private GameObject finishLineIndicator;
    [SerializeField] private GameObject startLineIndicator;

    public PlatformType type = PlatformType.Normal;

    private bool hasBeenVisited = false;

    private string FrogTag = "Frog";
    private Renderer m_Renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        finishLineIndicator.SetActive(false);
    }
    public void setAsFinishLine()
    {
        type = PlatformType.FinishLine;
        finishLineIndicator.SetActive(true);
    }
    public void setAsStartLine()
    {
        type = PlatformType.StartLine;
        startLineIndicator.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(FrogTag))
        {
        return;
        }
        switch (type)
        {
            case PlatformType.StartLine:
                GameManager.Instance.MarkPlatformAsVisited();
                m_Renderer.material = m_Material;
                break;
            case PlatformType.Normal:
                if (!hasBeenVisited)
                {
                    hasBeenVisited = true;
                    GameManager.Instance.MarkPlatformAsVisited();
                    m_Renderer.material = m_Material;
                }
                break;

            case PlatformType.FinishLine:
                GameManager.Instance.ReportFinishLineLanding();
                break;
        }

    }
}
