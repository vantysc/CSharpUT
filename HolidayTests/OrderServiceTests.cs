#region

using System.Collections.Generic;
using System.Linq;
using Lib;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace HolidayTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private IBookDao _bookDao;
        private OrderServiceForTest _target;

        [SetUp]
        public void SetUp()
        {
            _target = new OrderServiceForTest();
            _bookDao = Substitute.For<IBookDao>();
            _target.BookDao = _bookDao;
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
            _bookDao.DidNotReceive()
                    .Insert(Arg.Is<Order>(order => order.Type == orderType));
        }

        private void ShouldInsertOrder(int times, string orderType)
        {
            _bookDao.Received(times)
                    .Insert(Arg.Is<Order>(order => order.Type == orderType));
        }

        private void WhenSyncBookOrders()
        {
            _target.SyncBookOrders();
        }

        private void GivenOrders(params Order[] orders)
        {
            _target.Orders = orders.ToList();
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