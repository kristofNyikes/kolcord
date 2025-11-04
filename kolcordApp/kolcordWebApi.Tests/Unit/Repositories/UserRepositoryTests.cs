using kolcordWebApi.Data;
using kolcordWebApi.Models;
using kolcordWebApi.Repository;
using kolcordWebApi.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kolcordWebApi.Tests.Unit.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly AppDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _mockUserManager = TestDataGenerator.MockUserManager<ApplicationUser>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new UserRepository(_mockUserManager.Object, _context);
        }

        [Fact]
        public async Task GetByName_UserExists_ReturnsUser()
        {
            var user = TestDataGenerator.CreateTestUser();
            _mockUserManager.Setup(x => x.FindByNameAsync("testuser"))
                .ReturnsAsync(user);

            var result = await _repository.GetByName("testuser");

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
        }

        [Fact]
        public async Task GetByName_UserNotExists_ReturnsNull()
        {
            _mockUserManager.Setup(x => x.FindByNameAsync("nonexistent"))
                .ReturnsAsync((ApplicationUser)null);

            var result = await _repository.GetByName("nonexistent");

            Assert.Null(result);
        }

        [Fact]
        public async Task SearchUsers_UsersExist_ReturnsFilteredUsers()
        {
            var currentUser = TestDataGenerator.CreateTestUser("current", "currentuser");
            var user1 = TestDataGenerator.CreateTestUser("user1", "john");
            var user2 = TestDataGenerator.CreateTestUser("user2", "johnny");
            var user3 = TestDataGenerator.CreateTestUser("user3", "mike");

            _context.Users.AddRange(currentUser, user1, user2, user3);
            await _context.SaveChangesAsync();

            var result = await _repository.SearchUsers("john", "current");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.UserName == "john");
            Assert.Contains(result, u => u.UserName == "johnny");
            Assert.DoesNotContain(result, u => u.UserName == "mike");
            Assert.DoesNotContain(result, u => u.UserName == "currentuser");
        }

        [Fact]
        public async Task SearchUsers_WithFriendshipStatus_ReturnsCorrectStatus()
        {
            var currentUser = TestDataGenerator.CreateTestUser("current", "currentuser");
            var friendUser = TestDataGenerator.CreateTestUser("friend", "frienduser");
            var nonFriendUser = TestDataGenerator.CreateTestUser("nonfriend", "nonfrienduser");

            _context.Users.AddRange(currentUser, friendUser, nonFriendUser);

            _context.Friendships.AddRange(
                new Friendship { UserId = "current", FriendId = "friend", CreatedAt = DateTime.UtcNow },
                new Friendship { UserId = "friend", FriendId = "current", CreatedAt = DateTime.UtcNow }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.SearchUsers("user", "current");

            Assert.NotNull(result);
            var friendResult = result.First(u => u.UserName == "frienduser");
            var nonFriendResult = result.First(u => u.UserName == "nonfrienduser");

            Assert.True(friendResult.isFriend);
            Assert.False(nonFriendResult.isFriend);
        }

        [Fact]
        public async Task SearchUsers_NoMatches_ReturnsEmptyList()
        {
            var currentUser = TestDataGenerator.CreateTestUser("current", "currentuser");
            _context.Users.Add(currentUser);
            await _context.SaveChangesAsync();

            var result = await _repository.SearchUsers("nonexistent", "current");

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchUsers_CaseInsensitive_ReturnsUsers()
        {
            var currentUser = TestDataGenerator.CreateTestUser("current", "currentuser");
            var user1 = TestDataGenerator.CreateTestUser("user1", "JohnDoe");
            var user2 = TestDataGenerator.CreateTestUser("user2", "johnathan");

            _context.Users.AddRange(currentUser, user1, user2);
            await _context.SaveChangesAsync();

            var result = await _repository.SearchUsers("JOHN", "current");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.UserName == "JohnDoe");
            Assert.Contains(result, u => u.UserName == "johnathan");
        }

        [Fact]
        public async Task SearchUsers_LimitTo15Users_ReturnsMax15()
        {
            var currentUser = TestDataGenerator.CreateTestUser("current", "currentuser");
            _context.Users.Add(currentUser);

            for (int i = 1; i <= 20; i++)
            {
                var user = TestDataGenerator.CreateTestUser($"user{i}", $"testuser{i}");
                _context.Users.Add(user);
            }
            await _context.SaveChangesAsync();

            var result = await _repository.SearchUsers("testuser", "current");

            Assert.NotNull(result);
            Assert.Equal(15, result.Count); 
        }

        [Fact]
        public async Task SearchUsers_ExcludesCurrentUser_ReturnsOthers()
        {
            var currentUser = TestDataGenerator.CreateTestUser("current", "currentuser");
            var otherUser = TestDataGenerator.CreateTestUser("other", "otheruser");

            _context.Users.AddRange(currentUser, otherUser);
            await _context.SaveChangesAsync();

            var result = await _repository.SearchUsers("user", "current");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("otheruser", result[0].UserName);
            Assert.DoesNotContain(result, u => u.UserName == "currentuser");
        }


        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
