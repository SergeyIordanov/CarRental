using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using CarRental.Auth.BLL.DTO;
using CarRental.Auth.BLL.Infrastructure;
using CarRental.Auth.BLL.Services;
using CarRental.Auth.DAL.EF;
using CarRental.Auth.DAL.Identity;
using CarRental.Auth.DAL.Interfaces;
using CarRental.Auth.DAL.Repositories;
using CarRental.Entities.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.Auth.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        [Test, TestCaseSource(typeof(TestData), nameof(TestData.WrongCreateUserTestCases))]
        public void WrongUserCreationTest(UserDTO userDto)
        {
            //Arrange
            var user = new ApplicationUser
            {
                Email = "ok",
                Roles = { new IdentityUserRole { UserId = "test", RoleId = "test" } },
                ClientProfile = new ClientProfile { Name = "test" }
            };

            var mockStore = new Mock<IUserStore<ApplicationUser>>();
            mockStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            mockStore.As<IUserEmailStore<ApplicationUser>>().Setup(x => x.FindByEmailAsync("existed")).Returns(Task.FromResult((ApplicationUser)null));
            mockStore.As<IUserPasswordStore<ApplicationUser>>();
            mockStore.As<IUserRoleStore<ApplicationUser>>().Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            mockStore.As<IUserRoleStore<ApplicationUser>>().Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
               .Returns(Task.FromResult((IList<string>)new List<string>()));
            mockStore.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var mockDbSet = new Mock<DbSet<ClientProfile>>(MockBehavior.Strict);
            mockDbSet.Setup(x => x.Add(It.IsAny<ClientProfile>())).Returns(new ClientProfile());

            var userManager = new ApplicationUserManager(mockStore.Object);

            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.UserManager).Returns(userManager);

            //Act
            var userService = new UserService(mock.Object);
            OperationDetails result = userService.Create(userDto);

            //Assert
            Assert.That(result.Succedeed, Is.False);
        }

        [Test]
        public void GetAllReturnsUsersList()
        {
            //Arrange

            var mockStore = new Mock<IUserStore<ApplicationUser>>();
            mockStore.As<IQueryableUserStore<ApplicationUser>>();
            var userManager = new ApplicationUserManager(mockStore.Object);
            
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.UserManager).Returns(userManager);

            //Act
            var userService = new UserService(mock.Object);
            var result = userService.GetAll();
            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<UserDTO>)));
        }

        [Test]
        [TestCase("test")]
        public void SuccsessfulGetReturnsUser(string id)
        {
            //Arrange
            var user = new ApplicationUser {Id = id, Email = "ok", Roles = { new IdentityUserRole { UserId = id, RoleId = "test"}},
                ClientProfile = new ClientProfile {Name = "test"}};
            var mockStore = new Mock<IUserStore<ApplicationUser>>();
            mockStore.Setup(x => x.CreateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockStore.Setup(x => x.FindByIdAsync(id)).Returns(Task.FromResult(user));
            mockStore.As<IQueryableUserStore<ApplicationUser>>();
            var userManager = new ApplicationUserManager(mockStore.Object);

            var mockRoleStore = new Mock<RoleStore<ApplicationRole>>();
            var roleManager = new ApplicationRoleManager(mockRoleStore.Object);

            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.UserManager).Returns(userManager);
            mock.Setup(a => a.RoleManager).Returns(roleManager);

            //Act
            mock.Object.UserManager.Create(user);
            var userService = new UserService(mock.Object);
            var result = userService.Get(id);
            //Assert
            Assert.That(result, Is.TypeOf(typeof(UserDTO)));
        }

        [Test]
        [TestCase("")]
        [TestCase("notexisted")]
        [TestCase(null)]
        public void WrongGetReturnsUser(string id)
        {
            //Arrange
            var user = new ApplicationUser
            {
                Id = "test",
                Email = "ok",
                Roles = { new IdentityUserRole { UserId = "test", RoleId = "test" } },
                ClientProfile = new ClientProfile { Name = "test" }
            };
            var mockStore = new Mock<IUserStore<ApplicationUser>>();
            mockStore.Setup(x => x.CreateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockStore.As<IQueryableUserStore<ApplicationUser>>();
            var userManager = new ApplicationUserManager(mockStore.Object);

            var mockRoleStore = new Mock<RoleStore<ApplicationRole>>();
            var roleManager = new ApplicationRoleManager(mockRoleStore.Object);

            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.UserManager).Returns(userManager);
            mock.Setup(a => a.RoleManager).Returns(roleManager);

            //Act
            mock.Object.UserManager.Create(user);
            var userService = new UserService(mock.Object);
            var result = userService.Get(id);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.SuccessfulSetRoleTestCases))]
        public void SuccessfullSetRoleTest(UserDTO userDto, string role)
        {
            //Arrange
            var user = new ApplicationUser
            {
                Email = userDto.Email,
                Roles = { new IdentityUserRole { UserId = "test", RoleId = "test" } }
            };
            var mockStore = new Mock<IUserStore<ApplicationUser>>();
            mockStore.Setup(x => x.CreateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            mockStore.As<IQueryableUserStore<ApplicationUser>>();
            mockStore.As<IUserRoleStore<ApplicationUser>>().Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            mockStore.As<IUserRoleStore<ApplicationUser>>().Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).Returns(Task.FromResult((IList<string>)new List<string>()));
            mockStore.As<IUserEmailStore<ApplicationUser>>().Setup(x => x.FindByEmailAsync(userDto.Email)).Returns(Task.FromResult(user));
            var userManager = new ApplicationUserManager(mockStore.Object);

            var mockRoleStore = new Mock<RoleStore<ApplicationRole>>();
            var roleManager = new ApplicationRoleManager(mockRoleStore.Object);

            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.UserManager).Returns(userManager);
            mock.Setup(a => a.RoleManager).Returns(roleManager);

            //Act
            mock.Object.UserManager.Create(user);
            var userService = new UserService(mock.Object);
            var result = userService.SetRole(userDto, role);
            //Assert
            Assert.That(result.Succedeed, Is.True);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.WrongSetRoleTestCases))]
        public void WrongSetRoleTest(UserDTO userDto, string role)
        {
            //Arrange
            var user = new ApplicationUser
            {
                Email = userDto?.Email,
                Roles = { new IdentityUserRole { UserId = "test", RoleId = "test" } }
            };
            var mockStore = new Mock<IUserStore<ApplicationUser>>();
            mockStore.Setup(x => x.CreateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            mockStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            mockStore.As<IQueryableUserStore<ApplicationUser>>();
            mockStore.As<IUserRoleStore<ApplicationUser>>().Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            mockStore.As<IUserRoleStore<ApplicationUser>>().Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).Returns(Task.FromResult((IList<string>)new List<string>()));
            mockStore.As<IUserEmailStore<ApplicationUser>>().Setup(x => x.FindByEmailAsync(userDto == null ? null : userDto.Email)).Returns(Task.FromResult(user));
            var userManager = new ApplicationUserManager(mockStore.Object);

            var mockRoleStore = new Mock<RoleStore<ApplicationRole>>();
            var roleManager = new ApplicationRoleManager(mockRoleStore.Object);

            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.UserManager).Returns(userManager);
            mock.Setup(a => a.RoleManager).Returns(roleManager);

            //Act
            mock.Object.UserManager.Create(user);
            var userService = new UserService(mock.Object);
            var result = userService.SetRole(userDto, role);
            //Assert
            Assert.That(result.Succedeed, Is.False);
        }
    }
}
