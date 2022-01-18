using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardPrototype;
using CardPrototype.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField] private Button _changeCardButton;
    [SerializeField] private ListView _handListView;
    [SerializeField] private GameLoader _gameLoader;

    private int _changeIndex = 0;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => _gameLoader.IsLoaded);

        _changeCardButton.onClick.AddListener(OnClickChangeCard);

        var handCards = _gameLoader.Sprites.Select(s => CreateCard(s)).ToList();

        foreach (var card in handCards)
        {
            _handListView.CreateItem(card);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnClickChangeCard()
    {
        var hardCards = _handListView.GetSource<Card>();

        Debug.Log($"{_changeIndex} | {hardCards.Count}");

        if (hardCards.Count == 0)
            return;

        _changeIndex = Mathf.Clamp(_changeIndex, 0, hardCards.Count - 1);
        
        var card = hardCards[_changeIndex];

        var action = Random.Range(0, 3);
        if (action == 0)
            card.HP.Value = Random.Range(-2, 10);
        else if (action == 1)
            card.MP.Value = Random.Range(-2, 10);
        else if (action == 2)
            card.Damage.Value = Random.Range(-2, 10);

        _changeIndex = _changeIndex == hardCards.Count - 1 ?
            0 : _changeIndex + 1;
    }

    private Card CreateCard(Sprite sprite)
    {
        return new Card
        {
            Avatar = sprite,
            Title = $"Title {Random.Range(0, 999)}",
            Discription = $"Discription {Random.Range(0, 999)}",

            HP = new ReactiveProperty<int>(Random.Range(1, 10)),
            MP = new ReactiveProperty<int>(Random.Range(-2, 10)),
            Damage = new ReactiveProperty<int>(Random.Range(-2, 10)),
        };
    }
}
