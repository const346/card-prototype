using System.Collections;
using System.Collections.Generic;
using CardPrototype.Model;
using UnityEngine;
using UnityEngine.Networking;

public class GameLoader : MonoBehaviour
{
    public Loader Loader { get; private set; }
    public bool IsLoaded { get; private set; } = false;
    public List<Sprite> Sprites { get; private set; }

    private void Awake()
    {
        Sprites = new List<Sprite>();
        Loader = new Loader();
    }

    IEnumerator Start()
    {
        var count = Random.Range(4, 10);

        Loader.Count = count;
        for (var i = 0; i < count; i++)
        {
            var size = 128;
            var rand = Random.Range(0, 10000000);
            var url = $"https://picsum.photos/seed/{rand}/{size}/{size}";

            Loader.Detail = url;

            Debug.Log($"download image: {url}");
            yield return DownloadImage(url, res => Sprites.Add(res));

            Loader.Progress = i + 1;
        }

        IsLoaded = true;
    }

    IEnumerator DownloadImage(string url, System.Action<Sprite> result)
    {
        using (var request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            var size = new Rect(0, 0, texture.width, texture.height);
            var anchor = new Vector2(texture.width / 2f, texture.height / 2f);
            var sprite = Sprite.Create(texture, size, anchor);

            result.Invoke(sprite);
        }
    }
}