#region

using System.Collections.Generic;
using System.Linq;
using Lib;
using Moq;
using Moq.Protected;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace HolidayTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IBookDao> _bookDao;
        private Mock<OrderService> _target;

        [SetUp]
        public void SetUp()
        {
            // _target = new OrderServiceForTest();
            // _bookDao = Substitute.For<IBookDao>();
            // _target.BookDao = _bookDao;
            _target = new Mock<OrderService>();
            _bookDao = new Mock<IBookDao>();
            _target.Protected()
                   .Setup<IBookDao>("GetBookDao")
                   .Returns(_bookDao.Object);
        }

        [Test]
        public void syncBookOrders_3_orders_only_2_book_order()
        {
            GivenOrders(
                new Order {Type = "Book"},
                new Order {Type = "CD"},
                new Order {Type = "Book"});

            WhenSyncBookOrders();

            ShouldInsertOrder(2, "Book");
            ShouldNotInsertOrder("CD");
        }

        private void ShouldNotInsertOrder(string orderType)
        {
            // _bookDao.DidNotReceive()
            //         .Insert(Arg.Is<Order>(order => order.Type == orderType)); 
            
            _bookDao
                .Verify(x =>
                            x.Insert(It.Is<Order>(order => order.Type=="CD")), Times.Exactly(0));
        }

        private void ShouldInsertOrder(int times, string orderType)
        {
            // _bookDao.Received(times)
            //         .Insert(Arg.Is<Order>(order => order.Type == orderType));
            _bookDao
                .Verify(x =>
                            x.Insert(It.Is<Order>(order => order.Type=="Book")), Times.Exactly(2));
        }

        private void WhenSyncBookOrders()
        {
            _target.Object.SyncBookOrders();
        }

        private void GivenOrders(params Order[] orders)
        {
            // _target.Orders = orders.ToList();
            _target.Protected()
                   .Setup<List<Order>>("GetOrders")
                   .Returns(orders.ToList);

        }
    }

    public class OrderServiceForTest : OrderService
    {
        private IBookDao _bookDao;
        private List<Order> _orders;

        public List<Order> Orders
        {
            set => _orders = value;
        }

        public IBookDao BookDao
        {
            set => _bookDao = value;
        }

        protected override IBookDao GetBookDao()
        {
            return _bookDao;
        }

        protected override List<Order> GetOrders()
        {
            return _orders;
        }
    }
}