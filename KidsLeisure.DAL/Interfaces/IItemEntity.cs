namespace KidsLeisure.DAL.Interfaces
{
    public interface IItemEntity
    {
        string Name { get; set; }
        decimal Price { get; set; }
        int GetId();
    }
}
