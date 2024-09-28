namespace RonWeb.API.Models.Shared
{
    public class SelectItem<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
        public SelectItem() { }
        public SelectItem(string text, T value)
        {
            Text = text;
            Value = value;
        }
    }
}
