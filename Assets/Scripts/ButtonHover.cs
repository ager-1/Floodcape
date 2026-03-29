using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private float hoverScale = 1.5f;
    [SerializeField] private float transitionSpeed = 15f; 

    private Vector3 _initialScale;
    private Vector3 _targetScale;

    void Start()
    {
        _initialScale = transform.localScale;
        _targetScale = _initialScale;
    }

    void Update()
    {
      
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetScale = _initialScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetScale = _initialScale;
    }

   
    public void OnPointerDown(PointerEventData eventData)
    {

        _targetScale = _initialScale;

 
        EventSystem.current.SetSelectedGameObject(null);
    }

    void OnDisable()
    {
        transform.localScale = _initialScale;
        _targetScale = _initialScale;
    }
}