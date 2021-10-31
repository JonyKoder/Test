namespace Vodovoz.DAL.Model.Interface {
    public interface IEntity {
        int ID { get; set; }

        bool Validate();
    }
}
