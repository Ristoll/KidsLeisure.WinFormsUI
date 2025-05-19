using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Mappers
{
    public static class CustomerMapper
    {
        public static CustomerDto ToDto(DAL.Entities.CustomerEntity entity)
        {
            return new CustomerDto
            {
                Id = entity.CustomerId,
                NickName = entity.NickName,
                PhoneNumber = entity.PhoneNumber
            };
        }

        public static DAL.Entities.CustomerEntity ToEntity(CustomerDto dto)
        {
            return new DAL.Entities.CustomerEntity
            {
                CustomerId = dto.Id,
                NickName = dto.NickName,
                PhoneNumber = dto.PhoneNumber
            };
        }
    }
}

