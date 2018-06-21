//https://msdn.microsoft.com/en-us/data/dn314429#async
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;

namespace Identity.BLLTest
{
    [TestFixture]
    public class TestExample
    {
        internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            internal TestDbAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestDbAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestDbAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute(expression));
            }

            public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute<TResult>(expression));
            }
        }
        internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
        {
            public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
            { }

            public TestDbAsyncEnumerable(Expression expression)
                : base(expression)
            { }

            public IDbAsyncEnumerator<T> GetAsyncEnumerator()
            {
                return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
            {
                return GetAsyncEnumerator();
            }

            IQueryProvider IQueryable.Provider
            {
                get { return new TestDbAsyncQueryProvider<T>(this); }
            }
        }

        internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestDbAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public void Dispose()
            {
                _inner.Dispose();
            }

            public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_inner.MoveNext());
            }

            public T Current
            {
                get { return _inner.Current; }
            }

            object IDbAsyncEnumerator.Current
            {
                get { return Current; }
            }
        }

        public class BlogService
        {
            private BloggingContext _context;

            public BlogService(BloggingContext context)
            {
                _context = context;
            }

            public Blog AddBlog(string name, string url)
            {
                var blog = _context.Blogs.Add(new Blog { Name = name, Url = url });
                _context.SaveChanges();

                return blog;
            }

            public List<Blog> GetAllBlogs()
            {
                var query = from b in _context.Blogs
                            orderby b.Name
                            select b;

                return query.ToList();
            }

            public async Task<List<Blog>> GetAllBlogsAsync()
            {
                var query = from b in _context.Blogs
                            orderby b.Name
                            select b;

                return await query.ToListAsync();
            }
            public async Task<Blog> GetByIdAsync(int id)
            {
                var role = await _context.Blogs.FirstOrDefaultAsync(r => r.BlogId == id);

                return role;
            }
        }
        public class BloggingContext : DbContext
        {
            public virtual DbSet<Blog> Blogs { get; set; }
            public virtual DbSet<Post> Posts { get; set; }
        }
        public class Blog
        {
            public int BlogId { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }

            public virtual List<Post> Posts { get; set; }
        }
        public class Post
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public int BlogId { get; set; }
            public virtual Blog Blog { get; set; }
        }
        [Test]
        public async Task GetAllBlogsAsync_orders_by_name()
        {

            var data = new List<Blog>
            {
                new Blog { Name = "BBB",BlogId=1 },
                new Blog { Name = "ZZZ",BlogId=3 },
                new Blog { Name = "AAA" ,BlogId=2},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Blog>>();
            mockSet.As<IDbAsyncEnumerable<Blog>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Blog>(data.GetEnumerator()));

            mockSet.As<IQueryable<Blog>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Blog>(data.Provider));

            mockSet.As<IQueryable<Blog>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<BloggingContext>();
            mockContext.Setup(c => c.Blogs).Returns(mockSet.Object);

            var service = new BlogService(mockContext.Object);
            var blogs = await service.GetAllBlogsAsync();
            var bl = await service.GetByIdAsync(1);

            Assert.AreEqual(bl.Name, "BBB");
            Assert.AreEqual(3, blogs.Count);
            Assert.AreEqual("AAA", blogs[0].Name);
            Assert.AreEqual("BBB", blogs[1].Name);
            Assert.AreEqual("ZZZ", blogs[2].Name);
        }
    }
}

/*
 06 Men Viacheslav
 * using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.BLL.Interfaces;
using Auction.BLL.Services;
using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Repositories;
using Auction.DAL.Repositories.Identity;
using Moq;
using NUnit.Framework;
using Microsoft.AspNet.Identity.EntityFramework;
using Auction.BLL.Exceptions;
using Auction.BLL.DTOs;

namespace Auction.BLL.Tests
{
    [TestFixture]
    public class UsersServiceTests
    {
        Mock<IdentityUnitOfWork> uow;
        Mock<AuctionUserManager> userManager;
        Mock<AuctionRoleManager> roleManager;
        IUsersService service;

        [SetUp]
        public void SetUp()
        {
            uow = new Mock<IdentityUnitOfWork>("DefaultConnection");
            userManager = new Mock<AuctionUserManager>(new UserStore<AuctionUser>());
            roleManager = new Mock<AuctionRoleManager>(new RoleStore<AuctionRole>());

            uow.Setup(u => u.UserManager).Returns(userManager.Object);
            uow.Setup(u => u.RoleManager).Returns(roleManager.Object);
            uow.Setup(u => u.SaveAsync()).Returns(Task.Run(() => { }));

            service = new UsersService(uow.Object);
        }

        [Test]
        public void CreateAsync_ProvidingNull_ThrowsArgumentNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateAsync(null));
        }

        [Test]
        public void CreateAsync_ProvidingInvalid_ThrowsUsersManagement()
        {
            Assert.ThrowsAsync<UsersManagementException>(() => service.CreateAsync(new UserDTO() { Email = "123@123@123", Password = "+-" }));
        }

        [Test]
        public void AuthenticateAsync_ProvidingNull_ThrowsArgumentNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => service.AuthenticateAsync(null));
        }

        [Test]
        public void AuthenticateAsync_ProvidingInvalid_ThrowsUsersManagement()
        {
            Assert.ThrowsAsync<UsersManagementException>(() => service.AuthenticateAsync(new UserDTO() { Email = "123@123.123", Password = "Qwe123" }));
        }

        [Test]
        public void GetUserByIdAsync_ProvidingNull_ThrowsArgumentNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetUserByIdAsync(null));
        }

        [Test]
        public void GetUserByIdAsync_ProvidingInvalid_ThrowsUsersManagement()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetUserByIdAsync("1234"));
        }

        [Test]
        public void ChangeUserRoleAsync_ProvidingNullId_ThrowsArgumentNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => service.ChangeUserRoleAsync(null, "1", "2"));
        }

        [Test]
        public void ChangeUserRoleAsync_ProvidingNullRole_ThrowsArgumentNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => service.ChangeUserRoleAsync("id", "1", null));
        }

        [Test]
        public void ChangeUserRoleAsync_ProvidingEmptyRole_ThrowsArgumentNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => service.ChangeUserRoleAsync("id", "", "2"));
        }

        [Test]
        public void ChangeUserRoleAsync_ProvidingInvalidId_ThrowsUsersManagement()
        {
            Assert.ThrowsAsync<UsersManagementException>(() => service.ChangeUserRoleAsync("id", "1", "2"));
        }
    }
}
 */

