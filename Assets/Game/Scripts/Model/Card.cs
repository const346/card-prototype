
using System;
using UniRx;

namespace CardPrototype.Model
{
    public class Card
    {
        public ReactiveProperty<int> HP { get; set; }
        public ReactiveProperty<int> MP { get; set; }
        public ReactiveProperty<int> Damage { get; set; }

        public string Title { get; set; }
        public string Discription { get; set; }
        public UnityEngine.Sprite Avatar { get; set; }
    }
}