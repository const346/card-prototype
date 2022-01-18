using System;
using System.Collections;
using System.Collections.Generic;
using CardPrototype.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemListView))]
public class CardView : MonoBehaviour
{
    [SerializeField] private Image _avatarImage;
    [SerializeField] private Text _hpText;
    [SerializeField] private Text _mpText;
    [SerializeField] private Text _damageText;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _discriptionText;

    private ItemListView _itemListView;
    private Card _card => _itemListView.DataContext as Card;

    private ReactiveProperty<int> _visualHP;
    private ReactiveProperty<int> _visualMP;
    private ReactiveProperty<int> _visualDamage;

    private void Awake()
    {
        _itemListView = GetComponent<ItemListView>();
        _itemListView.OnDrop.AddListener(OnDrop);
    }

    private void Start()
    {
        _visualHP = new ReactiveProperty<int>(_card.HP.Value);
        _visualMP = new ReactiveProperty<int>(_card.MP.Value);
        _visualDamage = new ReactiveProperty<int>(_card.Damage.Value);

        _card.HP.Select(x => Observable.Interval(TimeSpan.FromSeconds(0.25f)).Take(Math.Abs(_card.HP.Value - _visualHP.Value))
                             .Select(_ => _visualHP.Value + Math.Sign(_card.HP.Value - _visualHP.Value)))
                .Switch()
                .Subscribe(x => _visualHP.Value = x).AddTo(this);

        _card.MP.Select(x => Observable.Interval(TimeSpan.FromSeconds(0.25f)).Take(Math.Abs(_card.MP.Value - _visualMP.Value))
                             .Select(_ => _visualHP.Value + Math.Sign(_card.MP.Value - _visualMP.Value)))
                .Switch()
                .Subscribe(x => _visualMP.Value = x).AddTo(this);

        _card.Damage.Select(x => Observable.Interval(TimeSpan.FromSeconds(0.25f)).Take(Math.Abs(_card.Damage.Value - _visualDamage.Value))
                             .Select(_ => _visualHP.Value + Math.Sign(_card.Damage.Value - _visualDamage.Value)))
                .Switch()
                .Subscribe(x => _visualDamage.Value = x).AddTo(this);

        _visualHP.SubscribeToText(_hpText);
        _visualMP.SubscribeToText(_mpText);
        _visualDamage.SubscribeToText(_damageText);

        _visualHP.Where(hp => hp <= 0)
            .Subscribe(_ => StartCoroutine(ItemDestroy()));

        // ...
        _avatarImage.sprite = _card.Avatar;
        _titleText.text = _card.Title;
        _discriptionText.text = _card.Discription;
    }

    private void OnDrop()
    {
        StartCoroutine(CollapseEffect());
    }

    private IEnumerator ItemDestroy()
    {
        yield return CollapseEffect(true);
        Destroy(gameObject);
    }

    private IEnumerator CollapseEffect(bool inverse = false)
    {
        var rectT = (RectTransform)transform;
        var defaultSize = rectT.sizeDelta;

        for (var i = 0f; i < 1f; i += 0.03f)
        {
            var k = inverse ? 1f - i : i;
            rectT.sizeDelta = new Vector2(defaultSize.x * k, defaultSize.y);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)rectT.parent);

            yield return new WaitForEndOfFrame();
        }

        rectT.sizeDelta = inverse ? new Vector2(0, defaultSize.y) : defaultSize;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)rectT.parent);
    }
}
