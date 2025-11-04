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
    public class MessageRepositoryTests : IDisposable
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly AppDbContext _context;
        private readonly MessageRepository _repository;

        public MessageRepositoryTests()
        {
            _mockUserManager = TestDataGenerator.MockUserManager<ApplicationUser>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new MessageRepository(_context, _mockUserManager.Object);
        }

        [Fact]
        public async Task GetOrCreateDirectConversation_UserExists_CreatesConversation()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync("user1")).ReturnsAsync(user1);
            _mockUserManager.Setup(x => x.FindByIdAsync("user2")).ReturnsAsync(user2);

            var result = await _repository.GetOrCreateDirectConversation("user1", "user2");

            Assert.NotNull(result);
            Assert.Equal($"{user1.UserName} & {user2.UserName}", result.Name);
            Assert.Equal(2, result.Participants.Count);
            Assert.Contains(result.Participants, p => p.UserId == "user1");
            Assert.Contains(result.Participants, p => p.UserId == "user2");

            Assert.All(result.Participants, p => Assert.NotNull(p.UserName));
        }

        [Fact]
        public async Task GetOrCreateDirectConversation_ConversationExisting()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync("user1")).ReturnsAsync(user1);
            _mockUserManager.Setup(x => x.FindByIdAsync("user2")).ReturnsAsync(user2);

            var firstResult = await _repository.GetOrCreateDirectConversation("user1", "user2");

            var secondResult = await _repository.GetOrCreateDirectConversation("user1", "user2");

            Assert.NotNull(secondResult);
            Assert.Equal(firstResult.Id, secondResult.Id);
            Assert.Equal(firstResult.Name, secondResult.Name);
        }

        [Fact]
        public async Task GetOrCreateDirectConversation_UserNotFound_ThrowsException()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            _context.Users.Add(user1);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync("user1")).ReturnsAsync(user1);
            _mockUserManager.Setup(x => x.FindByIdAsync("nonexistent")).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _repository.GetOrCreateDirectConversation("user1", "nonexistent"));
        }

        [Fact]
        public async Task SendMessage_ValidData_CreatesMessage()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync("user1")).ReturnsAsync(user1);
            _mockUserManager.Setup(x => x.FindByIdAsync("user2")).ReturnsAsync(user2);

            var conversation = await _repository.GetOrCreateDirectConversation("user1", "user2");

            var result = await _repository.SendMessage("user1", conversation.Id, "Hello world!");

            Assert.NotNull(result);
            Assert.Equal("Hello world!", result.Content);
            Assert.Equal("user1", result.SenderId);
            Assert.Equal(conversation.Id, result.ConversationId);
            Assert.Equal("user1", result.SenderName);
        }

        [Fact]
        public async Task GetMessages_ConversationHasMessages_ReturnsMessages()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync("user1")).ReturnsAsync(user1);
            _mockUserManager.Setup(x => x.FindByIdAsync("user2")).ReturnsAsync(user2);

            var conversation = await _repository.GetOrCreateDirectConversation("user1", "user2");
            await _repository.SendMessage("user1", conversation.Id, "Message 1");
            await _repository.SendMessage("user2", conversation.Id, "Message 2");

            var result = await _repository.GetMessages(conversation.Id);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Content == "Message 1");
            Assert.Contains(result, m => m.Content == "Message 2");
        }

        [Fact]
        public async Task GetMessages_WithSkipAndTake_ReturnsPaginatedResults()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync("user1")).ReturnsAsync(user1);
            _mockUserManager.Setup(x => x.FindByIdAsync("user2")).ReturnsAsync(user2);

            var conversation = await _repository.GetOrCreateDirectConversation("user1", "user2");

            for (int i = 1; i <= 5; i++)
            {
                await _repository.SendMessage("user1", conversation.Id, $"Message {i}");
            }

            var result = await _repository.GetMessages(conversation.Id, skip: 2, take: 2);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetMessage_MessageExists_MarksAsRead()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync("user1")).ReturnsAsync(user1);
            _mockUserManager.Setup(x => x.FindByIdAsync("user2")).ReturnsAsync(user2);

            var conversation = await _repository.GetOrCreateDirectConversation("user1", "user2");
            var messageResult = await _repository.SendMessage("user1", conversation.Id, "Test message");

            var result = await _repository.GetMessage(messageResult.Id);

            Assert.NotNull(result);
            Assert.True(result.IsRead);
            Assert.Equal("Test message", result.Content);
        }

        [Fact]
        public async Task GetMessage_MessageNotFound_ReturnsNull()
        {
            var result = await _repository.GetMessage(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserConversation_UserHasConversations_ReturnsConversations()
        {
            var user1 = TestDataGenerator.CreateTestUser("user1", "user1");
            var user2 = TestDataGenerator.CreateTestUser("user2", "user2");
            var user3 = TestDataGenerator.CreateTestUser("user3", "user3");

            _context.Users.AddRange(user1, user2, user3);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => _context.Users.Find(id));

            await _repository.GetOrCreateDirectConversation("user1", "user2");
            await _repository.GetOrCreateDirectConversation("user1", "user3");

            var result = await _repository.GetUserConversation("user1");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Contains(c.Participants, p => p.UserId == "user1"));
        }

        [Fact]
        public async Task CreateGroupConversation_ValidData_CreatesGroup()
        {
            var creator = TestDataGenerator.CreateTestUser("creator", "creator");
            var member1 = TestDataGenerator.CreateTestUser("member1", "member1");
            var member2 = TestDataGenerator.CreateTestUser("member2", "member2");

            _context.Users.AddRange(creator, member1, member2);
            await _context.SaveChangesAsync();

            var memberIds = new List<string> { "creator", "member1", "member2" };

            var result = await _repository.CreateGroupConversation("creator", "Test Group", memberIds);

            Assert.NotNull(result);
            Assert.Equal("Test Group", result.Name);
            Assert.Equal(3, result.Participants.Count);
            Assert.Contains(result.Participants, p => p.UserId == "creator" && p.Role == ParticipantRole.Owner);
            Assert.Contains(result.Participants, p => p.UserId == "member1" && p.Role == ParticipantRole.Member);
        }

        [Fact]
        public async Task CreateGroupConversation_UserNotFound_ThrowsException()
        {
            var creator = TestDataGenerator.CreateTestUser("creator", "creator");
            _context.Users.Add(creator);
            await _context.SaveChangesAsync();

            var memberIds = new List<string> { "creator", "nonexistent" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _repository.CreateGroupConversation("creator", "Test Group", memberIds));
        }


        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
