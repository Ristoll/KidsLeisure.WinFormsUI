namespace KidsLeisure.Core.Helpers
{
    public class DisplayItem<T>
    {
        public T Entity { get; set; } = default!;
        public string DisplayName { get; set; } = string.Empty;

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
