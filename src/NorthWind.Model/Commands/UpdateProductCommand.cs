using System.Runtime.Serialization;

namespace NorthWind.Model.Commands
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class UpdateProductCommand : AddProductCommand
    {
        public UpdateProductCommand(int id, string name, bool discounted): base(name, discounted)
        {
            ProductID = id;
        }
    }
}
