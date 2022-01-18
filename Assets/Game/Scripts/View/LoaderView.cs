using System.Collections;
using System.Collections.Generic;
using CardPrototype.Model;
using UnityEngine;
using UnityEngine.UI;

public class LoaderView : MonoBehaviour
{
    [SerializeField] private Text _progressText;
    [SerializeField] private Text _detailText;
    [SerializeField] private RectTransform _base;
    [SerializeField] private GameLoader _gameLoader;

    private void Start()
    {
        UpdateView();
    }

    private void Update()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        var loader = _gameLoader.Loader;

        _progressText.text = $"{loader.Progress}/{loader.Count}";
        _detailText.text = $"{loader.Detail}";
        _base.gameObject.SetActive(!loader.IsDone);
    }
}
