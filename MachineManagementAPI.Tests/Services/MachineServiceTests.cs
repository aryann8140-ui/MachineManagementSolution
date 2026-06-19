using MachineManagementAPI.Models;
using MachineManagementAPI.Repository.Interface;
using MachineManagementAPI.Services;
using Moq;

namespace MachineManagementAPI.Tests.Services
{
    public class MachineServiceTests
    {

        private readonly Mock<IMachineRepo> _mockRepo;
        private readonly MachineService _service;

        public MachineServiceTests()
        {
            _mockRepo = new Mock<IMachineRepo>();
            _service = new MachineService(_mockRepo.Object);
        }


        [Fact]
        public async Task GetAllMachines_ShouldReturnAllMachines()
        {
            var machines = new List<Machine>
            {
                new Machine
                {
                    Id = 1,
                    Name = "Lathe",
                    SerialNumber = "1234",
                    Location = "abc",
                    Status = 0,
                    PurchaseDate = DateTime.Now,
                }
            };

            _mockRepo.Setup(x => x.GetAllMachineAsync())
                     .ReturnsAsync(machines);

            var result = await _service.GetAllMachineAsync();

            Assert.NotNull(result);
        }
    }
    
}