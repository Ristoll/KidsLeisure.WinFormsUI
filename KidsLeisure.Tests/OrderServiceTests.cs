using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using KidsLeisure.BLL.Calculator;
using KidsLeisure.BLL.DTO;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.BLL.Services;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.DAL.Interfaces;
using NSubstitute;
using System.Linq.Expressions;

namespace KidsLeisure.Tests
{
    public class OrderServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerService _customerService;
        private readonly IPriceCalculatorSelector _priceCalculatorSelector;
        private readonly IMapper _mapper;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            _unitOfWork = _fixture.Freeze<IUnitOfWork>();
            _customerService = _fixture.Freeze<ICustomerService>();
            _priceCalculatorSelector = _fixture.Freeze<IPriceCalculatorSelector>();
            _mapper = _fixture.Freeze<IMapper>();

            _orderService = new OrderService(_unitOfWork, _customerService, _priceCalculatorSelector, _mapper);
        }

        [Fact]
        public async Task GetAllItemsAsync_ReturnsAllItems()
        {
            // Arrange
            var repo = Substitute.For<IRepository<AttractionEntity>>();
            var expectedList = _fixture.CreateMany<AttractionEntity>(3).ToList();

            _unitOfWork.GetRepository<AttractionEntity>().Returns(repo);
            repo.GetAllAsync().Returns(expectedList);

            // Act
            var result = await _orderService.GetAllItemsAsync<AttractionEntity>();

            // Assert
            Assert.Equal(expectedList.Count, result.Count);
            Assert.Equal(expectedList, result);
        }

        [Fact]
        public void ClearCurrentOrder_ResetsCurrentOrder()
        {
            // Arrange
            _orderService.CurrentOrder = _fixture.Create<OrderDto>();

            // Act
            _orderService.ClearCurrentOrder();

            // Assert
            Assert.NotNull(_orderService.CurrentOrder);
            Assert.Equal(0, _orderService.CurrentOrder.OrderId);
            Assert.Empty(_orderService.CurrentOrder.Attractions);
            Assert.Empty(_orderService.CurrentOrder.Characters);
            Assert.Empty(_orderService.CurrentOrder.Zones);
            Assert.Null(_orderService.CurrentOrder.Date);
            Assert.Null(_orderService.CurrentOrder.ProgramType);
            Assert.Equal(0, _orderService.CurrentOrder.TotalPrice);
        }

        [Fact]
        public async Task CreateCustomOrderAsync_WhenCustomerExists_CreatesOrder()
        {
            // Arrange
            var customerDto = _fixture.Create<CustomerDto>();
            _customerService.CurrentCustomer.Returns(customerDto);

            var customerEntity = _fixture.Create<CustomerEntity>();
            customerEntity.CustomerId = customerDto.Id;

            var customerRepo = Substitute.For<IRepository<CustomerEntity>>();
            var orderRepo = Substitute.For<IRepository<OrderEntity>>();

            _unitOfWork.GetRepository<CustomerEntity>().Returns(customerRepo);
            _unitOfWork.GetRepository<OrderEntity>().Returns(orderRepo);

            customerRepo.GetAllAsync().Returns(new List<CustomerEntity> { customerEntity });

            var orderEntity = new OrderEntity { OrderId = 1 };
            _mapper.Map<OrderEntity>(_orderService.CurrentOrder).Returns(orderEntity);
            _mapper.Map<OrderDto>(orderEntity).Returns(_fixture.Create<OrderDto>());

            // Act
            await _orderService.CreateCustomOrderAsync();

            // Assert
            await orderRepo.Received(1).AddAsync(orderEntity);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task CreateCustomOrderAsync_WhenCustomerNotFound_Throws()
        {
            // Arrange
            var customerDto = _fixture.Create<CustomerDto>();
            _customerService.CurrentCustomer.Returns(customerDto);

            var customerRepo = Substitute.For<IRepository<CustomerEntity>>();
            _unitOfWork.GetRepository<CustomerEntity>().Returns(customerRepo);
            customerRepo.GetAllAsync().Returns(new List<CustomerEntity>());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.CreateCustomOrderAsync());
        }
        [Fact]
        public async Task CreateBirthdayOrderAsync_CreatesOrderWithDefaultItems()
        {
            // Arrange
            var customerDto = _fixture.Create<CustomerDto>();
            _customerService.CurrentCustomer.Returns(customerDto);

            var orderRepo = Substitute.For<IRepository<OrderEntity>>();
            var attractionRepo = Substitute.For<IRepository<AttractionEntity>>();
            var characterRepo = Substitute.For<IRepository<CharacterEntity>>();
            var zoneRepo = Substitute.For<IRepository<ZoneEntity>>();
            var orderAttractionRepo = Substitute.For<IRepository<OrderAttractionEntity>>();
            var orderCharacterRepo = Substitute.For<IRepository<OrderCharacterEntity>>();
            var orderZoneRepo = Substitute.For<IRepository<OrderZoneEntity>>();

            _unitOfWork.GetRepository<OrderEntity>().Returns(orderRepo);
            _unitOfWork.GetRepository<AttractionEntity>().Returns(attractionRepo);
            _unitOfWork.GetRepository<CharacterEntity>().Returns(characterRepo);
            _unitOfWork.GetRepository<ZoneEntity>().Returns(zoneRepo);
            _unitOfWork.GetRepository<OrderAttractionEntity>().Returns(orderAttractionRepo);
            _unitOfWork.GetRepository<OrderCharacterEntity>().Returns(orderCharacterRepo);
            _unitOfWork.GetRepository<OrderZoneEntity>().Returns(orderZoneRepo);

            var attractions = _fixture.CreateMany<AttractionEntity>(5).ToList();
            var characters = _fixture.CreateMany<CharacterEntity>(3).ToList();
            var zones = _fixture.CreateMany<ZoneEntity>(3).ToList();

            attractionRepo.GetAllAsync().Returns(attractions);
            characterRepo.GetAllAsync().Returns(characters);
            zoneRepo.GetAllAsync().Returns(zones);

            var createdOrderEntity = new OrderEntity
            {
                OrderId = 123,
                ProgramType = ProgramType.Birthday,
                CustomerId = customerDto.Id,
                CustomerName = customerDto.NickName,
                CustomerPhone = customerDto.PhoneNumber
            };

            orderRepo.AddAsync(Arg.Do<OrderEntity>(o => o.OrderId = 123)).Returns(Task.CompletedTask);

            orderRepo.GetByIdWithIncludesAsync(123, true, true, true).Returns(createdOrderEntity);

            var mappedDto = _fixture.Create<OrderDto>();
            _mapper.Map<OrderDto>(createdOrderEntity).Returns(mappedDto);

            // Act
            var result = await _orderService.CreateBirthdayOrderAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mappedDto, result);

            await orderRepo.Received(1).AddAsync(Arg.Any<OrderEntity>());
            await orderAttractionRepo.Received(1)
                .AddRangeAsync(Arg.Is<IEnumerable<OrderAttractionEntity>>(list => list.Count() == 3));

            await orderCharacterRepo.Received(1)
                .AddRangeAsync(Arg.Is<IEnumerable<OrderCharacterEntity>>(list => list.Count() == 2));

            await orderZoneRepo.Received(1)
                .AddRangeAsync(Arg.Is<IEnumerable<OrderZoneEntity>>(list => list.Count() == 2)); 
            await _unitOfWork.Received(3).SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateOrderAsync_UpdatesOrder()
        {
            // Arrange
            var orderEntity = _fixture.Create<OrderEntity>();
            _mapper.Map<OrderEntity>(_orderService.CurrentOrder).Returns(orderEntity);
            _mapper.Map<OrderDto>(orderEntity).Returns(_fixture.Create<OrderDto>());

            var repo = Substitute.For<IRepository<OrderEntity>>();
            _unitOfWork.GetRepository<OrderEntity>().Returns(repo);

            // Act
            await _orderService.UpdateOrderAsync();

            // Assert
            await repo.Received(1).UpdateAsync(orderEntity);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteOrderAsync_DeletesOrder()
        {
            // Arrange
            var repo = Substitute.For<IRepository<OrderEntity>>();
            _unitOfWork.GetRepository<OrderEntity>().Returns(repo);

            _orderService.CurrentOrder.OrderId = 5;

            // Act
            await _orderService.DeleteOrderAsync();

            // Assert
            await repo.Received(1).DeleteAsync(5);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task CalculateOrderPriceAsync_CustomProgram_ReturnsTotalWithExtra()
        {
            // Arrange
            var mockOrderService = Substitute.For<IOrderService>();

            mockOrderService.FindItemByAsync<ZoneEntity>(Arg.Any<Expression<Func<ZoneEntity, bool>>>()!)
                .Returns(Task.FromResult<ZoneEntity?>(new ZoneEntity { ZoneId = 1, Price = 200 }));

            mockOrderService.FindItemByAsync<AttractionEntity>(Arg.Any<Expression<Func<AttractionEntity, bool>>>()!)
                .Returns(Task.FromResult<AttractionEntity?>(new AttractionEntity { AttractionId = 1, Price = 150 }));

            mockOrderService.FindItemByAsync<CharacterEntity>(Arg.Any<Expression<Func<CharacterEntity, bool>>>()!)
                .Returns(Task.FromResult<CharacterEntity?>(new CharacterEntity { CharacterId = 1, Price = 300 }));


            var order = new OrderEntity
            {
                ProgramType = ProgramType.Custom,
                Zones = new List<OrderZoneEntity> { new OrderZoneEntity { ZoneId = 1 } },
                Attractions = new List<OrderAttractionEntity> { new OrderAttractionEntity { AttractionId = 1 } },
                Characters = new List<OrderCharacterEntity> { new OrderCharacterEntity { CharacterId = 1 } }
            };

            var calculator = new CustomProgramPriceCalculator(new Lazy<IOrderService>(() => mockOrderService));

            // Act
            var result = await calculator.CalculatePriceAsync(order);

            // Assert
            Assert.Equal(750, result);
        }

        [Fact]
        public async Task CalculateOrderPriceAsync_DefaultProgram_ReturnsTotal()
        {
            // Arrange
            var mockOrderService = Substitute.For<IOrderService>();

            mockOrderService.FindItemByAsync<ZoneEntity>(Arg.Any<Expression<Func<ZoneEntity, bool>>>()!)
                            .Returns(Task.FromResult<ZoneEntity?>(new ZoneEntity { ZoneId = 2, Price = 150 }));

            mockOrderService.FindItemByAsync<AttractionEntity>(Arg.Any<Expression<Func<AttractionEntity, bool>>>()!)
                            .Returns(Task.FromResult<AttractionEntity?>(new AttractionEntity { AttractionId = 2, Price = 200 }));

            mockOrderService.FindItemByAsync<CharacterEntity>(Arg.Any<Expression<Func<CharacterEntity, bool>>>()!)
                            .Returns(Task.FromResult<CharacterEntity?>(new CharacterEntity { CharacterId = 2, Price = 350 }));

            var order = new OrderEntity
            {
                ProgramType = ProgramType.Birthday,
                Zones = new List<OrderZoneEntity> { new OrderZoneEntity { ZoneId = 2 } },
                Attractions = new List<OrderAttractionEntity> { new OrderAttractionEntity { AttractionId = 2 } },
                Characters = new List<OrderCharacterEntity> { new OrderCharacterEntity { CharacterId = 2 } }
            };

            var calculator = new DefaultPriceCalculator(new Lazy<IOrderService>(() => mockOrderService));

            // Act
            var result = await calculator.CalculatePriceAsync(order);

            // Assert
            Assert.Equal(700, result); // 150 + 200 + 350
        }



        [Fact]
        public void SetOrderTime_SetsDate()
        {
            var dt = DateTime.Now;

            _orderService.SetOrderTime(dt);

            Assert.Equal(dt, _orderService.CurrentOrder.Date);
        }

        [Fact]
        public void SetOrderType_SetsProgramType()
        {
            // Arrange
            var programType = ProgramType.Birthday;
            var dto = _fixture.Create<ProgramTypeDto>();

            _mapper.Map<ProgramTypeDto>(programType).Returns(dto);

            // Act
            _orderService.SetOrderType(programType);

            // Assert
            Assert.Equal(dto, _orderService.CurrentOrder.ProgramType);
        }

        [Fact]
        public void SetTotalPrice_SetsPrice()
        {
            decimal price = 789m;

            _orderService.SetTotalPrice(price);

            Assert.Equal(price, _orderService.CurrentOrder.TotalPrice);
        }

        [Fact]
        public void AddToOrderCollection_AddsCorrectly()
        {
            // Arrange
            var attraction = _fixture.Create<AttractionEntity>();
            var character = _fixture.Create<CharacterEntity>();
            var zone = _fixture.Create<ZoneEntity>();

            // Act
            _orderService.ClearCurrentOrder();

            _orderService.AddToOrderCollection(attraction);
            _orderService.AddToOrderCollection(character);
            _orderService.AddToOrderCollection(zone);

            // Assert
            Assert.Contains(_orderService.CurrentOrder.Attractions, a => a.Id == attraction.AttractionId);
            Assert.Contains(_orderService.CurrentOrder.Characters, c => c.Id == character.CharacterId);
            Assert.Contains(_orderService.CurrentOrder.Zones, z => z.Id == zone.ZoneId);
        }

        [Fact]
        public void AddToOrderCollection_ThrowsOnUnknownType()
        {
            var unknown = Substitute.For<IItemEntity>();

            Assert.Throws<ArgumentException>(() => _orderService.AddToOrderCollection(unknown));
        }

        [Fact]
        public void RemoveFromOrderCollection_RemovesCorrectly()
        {
            // Arrange
            var attractionDto = _fixture.Create<OrderAttractionDto>();
            var characterDto = _fixture.Create<OrderCharacterDto>();
            var zoneDto = _fixture.Create<OrderZoneDto>();

            _orderService.CurrentOrder.Attractions.Add(attractionDto);
            _orderService.CurrentOrder.Characters.Add(characterDto);
            _orderService.CurrentOrder.Zones.Add(zoneDto);

            // Act
            _orderService.RemoveFromOrderCollection(attractionDto);
            _orderService.RemoveFromOrderCollection(characterDto);
            _orderService.RemoveFromOrderCollection(zoneDto);

            // Assert
            Assert.DoesNotContain(_orderService.CurrentOrder.Attractions, a => a.Id == attractionDto.Id);
            Assert.DoesNotContain(_orderService.CurrentOrder.Characters, c => c.Id == characterDto.Id);
            Assert.DoesNotContain(_orderService.CurrentOrder.Zones, z => z.Id == zoneDto.Id);
        }

        [Fact]
        public void RemoveFromOrderCollection_ThrowsOnUnknownType()
        {
            var unknown = Substitute.For<IOrderItemDto>();

            Assert.Throws<ArgumentException>(() => _orderService.RemoveFromOrderCollection(unknown));
        }

        [Fact]
        public async Task FindItemByAsync_ReturnsFoundItem()
        {
            // Arrange
            var repo = Substitute.For<IRepository<AttractionEntity>>();
            _unitOfWork.GetRepository<AttractionEntity>().Returns(repo);

            var expected = _fixture.Create<AttractionEntity>();
            repo.FindAsync(Arg.Any<Expression<Func<AttractionEntity, bool>>>())
                .Returns(Task.FromResult<AttractionEntity?>(expected));

            // Act
            var actual = await _orderService.FindItemByAsync<AttractionEntity>(a => a.AttractionId == expected.AttractionId);

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}