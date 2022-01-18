using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListView : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemListView _itemTemplate;
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private bool _clearItemsContainer = true;
    [SerializeField] private bool _isDropContainer = false;
    [SerializeField] private bool _isDraggebleItems = false;

    public bool IsDraggebleItems => _isDraggebleItems;

    private void Awake()
    {
        if (_clearItemsContainer)
        {
            ClearAll();
        }
    }

    public void ClearAll()
    {
        for (var i = 0; i < _itemsContainer.childCount; ++i)
        {
            var child = _itemsContainer.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void Rebuild(IList source)
    {
        if (source == null)
        {
            return;
        }

        ClearAll();

        foreach (var itemDataContext in source)
        {
            CreateItem(itemDataContext);
        }
    }

    public void CreateItem(object dataContext)
    {
        var item = Instantiate(_itemTemplate, _itemsContainer);
        item.UpdateView(dataContext);
        item.ListView = this;
    }

    void IDropHandler.OnDrop(PointerEventData data)
    {
        if (_isDropContainer &&
            data.pointerDrag.TryGetComponent<ItemListView>(out var item) &&
            !item.transform.IsChildOf(_itemsContainer))
        {
            item.transform.rotation = Quaternion.identity;
            item.transform.SetParent(_itemsContainer);
            item.transform.SetAsLastSibling();
            item.ListView = this;
        }
    }

    public List<T> GetSource<T>()
    {
        var result = new List<T>();
        for (var i = 0; i < _itemsContainer.childCount; ++i)
        {
            var child = _itemsContainer.GetChild(i);
            if (child.TryGetComponent<ItemListView>(out var item) &&
                item.DataContext is T itemContext)
            {
                result.Add(itemContext);
            }
        }

        return result;
    }
}