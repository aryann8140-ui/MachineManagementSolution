using MachineManagementAPI.Models;
using MachineManagementAPI.Repository.Interface;
using MachineManagementAPI.Services;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MachineManagementAPI.Dtos;
using Xunit.Sdk;
using System.Net.Mail;

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
            Assert.Single(result);

            _mockRepo.Verify(x=> x.GetAllMachineAsync(),Times.Once);
        }


        [Fact]
        public async Task GetAllMachines_ShouldReturnException_WhenMachineIsNotThere()
        {
            IEnumerable<Machine>? machines = null;

             _mockRepo.Setup(x => x.GetAllMachineAsync())
                     .ReturnsAsync(machines);
            await Assert.ThrowsAsync<ArgumentNullException>(()=>
            _service.GetAllMachineAsync()); 

           _mockRepo.Verify(x=> x.GetAllMachineAsync(),Times.Once);
 
        }

        [Fact]
        public async Task GetAllMachines_ShouldReturnNoMachine_WhenMachineListIsEmpty()
        {
            var machines = new List<Machine>();

            _mockRepo.Setup(x => x.GetAllMachineAsync())
                     .ReturnsAsync(machines);

            var result = await _service.GetAllMachineAsync();

            Assert.NotNull(result);
            Assert.Empty(result);

            _mockRepo.Verify(x => x.GetAllMachineAsync(), Times.Once);
        }




        [Fact]
        public async Task GetMachineById_ShowArgumentException_WhenMachineIdIsInvalid()
        {
            var machines = new Machine
            {
                    Id = -1,
                    Name = "Lathe",
                    SerialNumber = "1234",
                    Location = "abc",
                    Status = 0,
                    PurchaseDate = DateTime.Now,
                };

                _mockRepo.Setup(x=>x.GetMachineByIdAsync(machines.Id))
                            .ReturnsAsync(machines);

                await Assert.ThrowsAsync<ArgumentException>(()=>
                _service.GetMachineByIdAsync(machines.Id)
                );     

                _mockRepo.Verify(x=>x.GetMachineByIdAsync(machines.Id),Times.Never);  
            
        }


        [Fact]
        public async Task GetMachineById_ShowArgumentNullException_WhenMachineisNull()
        {
            int Id = 1;
            Machine? machine = null;
            _mockRepo.Setup(x=>x.GetMachineByIdAsync(Id))
            .ReturnsAsync(machine);

            await Assert.ThrowsAsync<ArgumentNullException>(()=>
            _service.GetMachineByIdAsync(Id));

            _mockRepo.Verify(x=>x.GetMachineByIdAsync(Id),Times.Once);
        }

    
    
    [Fact]
    public async Task GetMachineById_ReturnMachine_WhenValidIdIsPassed()
        {
           var machine = new Machine{
                     Id = 1,
                    Name = "Lathe",
                    SerialNumber = "1234",
                    Location = "abc",
                    Status = 0,
                    PurchaseDate = DateTime.Now,
            };

            _mockRepo.Setup(x=>x.GetMachineByIdAsync(machine.Id))
            .ReturnsAsync(machine);

            var result = await _service.GetMachineByIdAsync(machine.Id);

            Assert.NotNull(result);

            _mockRepo.Verify(x=>x.GetMachineByIdAsync(machine.Id), Times.Once);

        }


        [Fact]
        public async Task CreateMachine_Successfully_whenCorrectMachineIsPassed()
        {
            var createDto = new CreateMachineDto
            {
                Name = "Lathe",
                SerialNumber = "1234",
                Location = "abc",
                Status = 0,
            };

            _mockRepo.Setup(x => x.CreateMachineAsync(It.IsAny<Machine>()))
                .Returns(Task.CompletedTask);

            await _service.CreateMachineAsync(createDto);

            _mockRepo.Verify(x => x.CreateMachineAsync(It.IsAny<Machine>()), Times.Once);
        }


          [Fact]
        public async Task CreateMachine_Unsuccessfull_whenArgumentExceptionIsThere()
        {
            var createDto = new CreateMachineDto
            {
                Name = "Lathe",
                SerialNumber = "   ",
                Location = "",
                Status = 0,
            };

            _mockRepo.Setup(x => x.CreateMachineAsync(It.IsAny<Machine>()))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ArgumentException>(()=>
            _service.CreateMachineAsync(createDto));
            _mockRepo.Verify(x => x.CreateMachineAsync(It.IsAny<Machine>()), Times.Never);
        }

        [Fact]
        public async Task UpdateMachine_GiveArgumentException_WhenIdIsInvalid()
        {
            var dto = new UpdateMachineDto
            {
                    Id = -1,
                    Name = "Lathe",
                    SerialNumber = "1234",
                    Location = "abc",
                    Status = 0,
            };

            _mockRepo.Setup(x => x.UpdateMachineAsync(It.IsAny<Machine>()))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ArgumentException>(()=>
                _service.UpdateMachineAsync(dto.Id, dto));

            _mockRepo.Verify(x => x.UpdateMachineAsync(It.IsAny<Machine>()), Times.Never);
        }

        [Fact]
        public async Task UpdateMachine_GiveKeyNotFoundException_WhenMachineIsNotThere()
        {
             var id = 5;

            var dto = new UpdateMachineDto
            {
                  Name = "Updated Machine",
                  SerialNumber = "SN001",
                  Location = "Delhi",
                  Status = 0
            };

                 _mockRepo.Setup(x => x.GetMachineByIdAsync(id))
                 .ReturnsAsync((Machine?)null);

             await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                 _service.UpdateMachineAsync(id, dto));

                _mockRepo.Verify(x => x.GetMachineByIdAsync(id), Times.Once);
                _mockRepo.Verify(x => x.UpdateMachineAsync(It.IsAny<Machine>()), Times.Never);
       
       
        }           
    }
}