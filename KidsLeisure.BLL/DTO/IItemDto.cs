using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsLeisure.BLL.DTO
{
    public interface IItemDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
