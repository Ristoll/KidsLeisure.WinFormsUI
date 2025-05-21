using AutoFixture;
using AutoMapper;
using KidsLeisure.BLL.DTO;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.BLL.Services;
using KidsLeisure.DAL.Entities;
using Moq;
using System.Linq.Expressions;

namespace KidsLeisure.Tests
{
    public class CustomerServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<CustomerEntity>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _fixture = new Fixture();

            // Заміна поведінки рекурсії, щоб уникнути циклічних помилок AutoFixture
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IRepository<CustomerEntity>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<CustomerEntity>())
                           .Returns(_repositoryMock.Object);

            _customerService = new CustomerService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldAddEntityAndReturnDto()
        {
            // Arrange
            var dto = _fixture.Create<CustomerDto>();
            var entity = _fixture.Create<CustomerEntity>();

            _unitOfWorkMock.Setup(u => u.GetRepository<CustomerEntity>())
                           .Returns(_repositoryMock.Object);

            _mapperMock.Setup(m => m.Map<CustomerEntity>(dto))
                       .Returns(entity);

            _repositoryMock.Setup(r => r.AddAsync(entity))
                           .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                           .ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<CustomerDto>(entity))
                       .Returns(dto);

            // Встановлюємо CurrentCustomer у сервісі перед викликом методу
            _customerService.CurrentCustomer = dto;

            // Act
            var result = await _customerService.CreateCustomerAsync();

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.Equal(dto, result);
        }


        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnMappedList()
        {
            // Arrange
            var entities = _fixture.Create<List<CustomerEntity>>();
            var dtos = _fixture.Create<List<CustomerDto>>();

            _repositoryMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(entities);

            _mapperMock.Setup(m => m.Map<List<CustomerDto>>(entities))
                       .Returns(dtos);

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnMappedDto_WhenEntityFound()
        {
            // Arrange
            var entity = _fixture.Create<CustomerEntity>();
            var dto = _fixture.Create<CustomerDto>();

            _repositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<System.Func<CustomerEntity, bool>>>()))
                           .ReturnsAsync(entity);

            _mapperMock.Setup(m => m.Map<CustomerDto>(entity))
                       .Returns(dto);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(entity.CustomerId);

            // Assert
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnNull_WhenEntityNotFound()
        {
            // Arrange
            _repositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<System.Func<CustomerEntity, bool>>>()))
                           .ReturnsAsync((CustomerEntity?)null);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(123);

            // Assert
            Assert.Null(result);
        }
    }
}
