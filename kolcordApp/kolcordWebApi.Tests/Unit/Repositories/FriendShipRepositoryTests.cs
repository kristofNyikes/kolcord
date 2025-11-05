using kolcordWebApi.Data;
using kolcordWebApi.Models;
using kolcordWebApi.Models.Enums;
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
    public class FriendShipRepositoryTests : IDisposable
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly AppDbContext _context;
        private readonly FriendshipRepository _repository;

        public FriendShipRepositoryTests()
        {
            _mockUserManager = TestDataGenerator.MockUserManager<ApplicationUser>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new FriendshipRepository(_mockUserManager.Object, _context);
        }

        [Fact]
        public async Task TaskGetByName_UserExists_ReturnsUser()
        {
            var user = TestDataGenerator.CreateTestUser();
            _mockUserManager.Setup(x => x.FindByNameAsync("testuser")).ReturnsAsync(user);

            var result = await _repository.GetByName("testuser");

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetByName_UserNotExists_ReturnsNull()
        {
            _mockUserManager.Setup(x => x.FindByNameAsync("nonexistent")).ReturnsAsync((ApplicationUser)null);

            var result = await _repository.GetByName("nonexistent");

            Assert.Null(result);
        }

        [Fact]
        public async Task AddFriend_ValidUsers_CreatesFriendship()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");

            _mockUserManager.Setup(x => x.FindByNameAsync("user2"))
                .ReturnsAsync(user2);

            var result = await _repository.AddFriend(user1, "user2");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, f => f.UserId == "user1" && f.FriendId == "user2");
            Assert.Contains(result, f => f.UserId == "user2" && f.FriendId == "user1");
        }

        [Fact]
        public async Task AddFriend_FriendNotFound_ReturnsNull()
        {
            var user1 = TestDataGenerator.CreateTestUser();
            _mockUserManager.Setup(x => x.FindByNameAsync("nonexistent")).ReturnsAsync((ApplicationUser)null);

            var result = await _repository.AddFriend(user1, "nonexistent");

            Assert.Null(result);
        }

        [Fact]
        public async Task AddFriend_FriendshipAlreadyExists_ReturnsNull()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");

            _mockUserManager.Setup(x => x.FindByNameAsync("user2"))
                .ReturnsAsync(user2);

            await _repository.AddFriend(user1, "user2");

            var result = await _repository.AddFriend(user1, "user2");

            Assert.Null(result);
        }

        [Fact]
        public async Task SendFriendRequest_ValidUsers_CreatesRequest()
        {
            var sender = TestDataGenerator.CreateTestUser("user1", "sender");
            var receiver = TestDataGenerator.CreateTestUser("user2", "receiver");

            _mockUserManager.Setup(x => x.FindByNameAsync("receiver"))
                .ReturnsAsync(receiver);

            var result = await _repository.SendFriendRequest(sender, "receiver");

            Assert.NotNull(result);
            Assert.Equal(sender.Id, result.SenderId);
            Assert.Equal(receiver.Id, result.ReceiverId);
            Assert.Equal(FriendRequestStatus.Pending, result.FriendRequestStatus);
        }

        [Fact]
        public async Task AcceptFriendRequest_ValidRequest_CreatesFriendship()
        {
            // Arrange
            var sender = TestDataGenerator.CreateTestUser("user1", "sender");
            var receiver = TestDataGenerator.CreateTestUser("user2", "receiver");

            // Add users to context FIRST
            _context.Users.AddRange(sender, receiver);

            var friendRequest = new FriendRequest
            {
                Id = 1,
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                FriendRequestStatus = FriendRequestStatus.Pending
            };
            _context.FriendRequests.Add(friendRequest);
            await _context.SaveChangesAsync();

            // Mock UserManager to return users when FindByNameAsync is called
            _mockUserManager.Setup(x => x.FindByNameAsync("receiver"))
                .ReturnsAsync(receiver);
            _mockUserManager.Setup(x => x.FindByNameAsync("sender"))
                .ReturnsAsync(sender);

            // Act
            var result = await _repository.AcceptFriendRequest(1, receiver);

            // Assert
            Assert.True(result);

            // Verify friendship was created
            var friendships = await _context.Friendships.ToListAsync();
            Assert.Equal(2, friendships.Count); // Two friendship records

            // Verify friend request status was updated
            var updatedRequest = await _context.FriendRequests.FindAsync(1);
            Assert.Equal(FriendRequestStatus.Accepted, updatedRequest.FriendRequestStatus);
        }

        [Fact]
        public async Task AcceptFriendRequest_InvalidRequestId_ReturnsFalse()
        {
            var receiver = TestDataGenerator.CreateTestUser();

            var result = await _repository.AcceptFriendRequest(999, receiver);

            Assert.False(result);
        }

        [Fact]
        public async Task GetFriendRequests_UserHasRequests_ReturnsRequests()
        {
            var user = TestDataGenerator.CreateTestUser("user1", "user1");
            var sender = TestDataGenerator.CreateTestUser("user2", "user2");

            var friendRequest = new FriendRequest
            {
                SenderId = sender.Id,
                ReceiverId = user.Id,
                FriendRequestStatus = FriendRequestStatus.Pending,
                Sender = sender
            };
            _context.FriendRequests.Add(friendRequest);
            await _context.SaveChangesAsync();

            var result = await _repository.GetFriendRequests(user);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(sender.UserName, result[0].Sender.UserName);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
