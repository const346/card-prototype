using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class ItemListView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private string _dragContainerTag = "DragContainer";
    [SerializeField] private Image _targetImage;
    [SerializeField] private Sprite _dragSprite;

    [SerializeField] public UnityEvent OnCancelDrag;
    [SerializeField] public UnityEvent OnDrag;
    [SerializeField] public UnityEvent OnDrop;

    private GameObject _dragContainer;
    private CanvasGroup _canvasGroup;
    private Vector2 _positionOffset;
    private Sprite _normalSprite;
    private Transform _parent;
    private int _subIndex;
    private bool _isDrag;

    public object DataContext { get; private set; }
    public ListView ListView { get; set; }

    public void UpdateView(object dataContext)
    {
        DataContext = dataContext;
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _dragContainer = GameObject.FindWithTag(_dragContainerTag);

        _normalSprite = _targetImage.sprite;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (!ListView.IsDraggebleItems || _isDrag)
            return;

        _isDrag = true;

        _positionOffset = (Vector2)transform.position - eventData.position;

        _parent = transform.parent;
        _subIndex = transform.GetSiblingIndex();

        transform.SetParent(_dragContainer.transform, false);
        _canvasGroup.blocksRaycasts = false;

        if (_targetImage && _dragSprite)
        {
            _targetImage.sprite = _dragSprite;
        }

        OnDrag?.Invoke();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!_isDrag)
            return;


        transform.position = eventData.position + _positionOffset;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (!_isDrag)
            return;

        _isDrag = false;

        if (transform.parent == _dragContainer.transform)
        {
            transform.SetParent(_parent.transform, false);
            transform.SetSiblingIndex(_subIndex);

            OnCancelDrag?.Invoke();
        }
        else
        {
            OnDrop?.Invoke();
        }

        _canvasGroup.blocksRaycasts = true;

        if (_targetImage)
        {
            _targetImage.sprite = _normalSprite;
        }
    }
}