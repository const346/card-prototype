
namespace CardPrototype.Model
{
    public class Loader
    {
        public int Progress { get; set; }
        public int Count { get; set; }
        public string Detail { get; set; }

        public bool IsDone => Progress >= Count;
    }
}