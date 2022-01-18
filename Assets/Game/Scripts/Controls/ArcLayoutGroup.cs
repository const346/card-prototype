using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Arc Layout Group")]
public class ArcLayoutGroup : LayoutGroup
{
    [SerializeField]
    private float _maxArc = 50;
    [SerializeField]
    private float _angle = 3;
    [SerializeField]
    private float _radius = 450;
    [SerializeField]
    private float _offset = 400;
    [SerializeField]
    private bool _inverse = false;

    public override void CalculateLayoutInputVertical()
    {

    }

    public override void SetLayoutHorizontal()
    {
        SetArcLayout();
    }

    public override void SetLayoutVertical()
    {
        SetArcLayout();
    }

    private void SetArcLayout()
    {
        var activeCount = GetActiveChilds().Count();
        var activeIndex = 0;

        foreach (var child in GetActiveChilds())
        {
            var k = activeCount > 1 ? activeIndex / (activeCount - 1f) : 0.5f;
            if (_inverse)
                k = 1 - k;

            var a = Mathf.Lerp(1, -1, k);
            var n = (activeCount - 1) * _angle;
            var m = Mathf.Min(_maxArc / 2f, n);

            child.localRotation = Quaternion.AngleAxis(a * m, Vector3.forward);

            var anchor = rectTransform.pivot + rectTransform.rect.center;
            var anglePosition = (Vector2)(child.localRotation * Vector2.up * _radius);

            child.anchoredPosition = anchor + anglePosition - Vector2.up * _offset + Vector2.up;

            activeIndex++;
        }
    }

    private IEnumerable<RectTransform> GetActiveChilds()
    {
        for (var i = 0; i < rectChildren.Count; i++)
        {
            var child = rectChildren[i];
            if (child.gameObject.activeSelf)
            {
                yield return child;
            }
        }
    }
}
