using System.Collections.Generic;
using Lib;
using NSubstitute;
using NUnit.Framework;

namespace HolidayTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        [Test]
        public void syncBookOrders_3_orders_only_2_book_order()
        {
            var target = new OrderServiceForTest();
            target.Orders = new List<Order>
                            {
                                new Order() {Type = "Book"},
                                new Order() {Type = "CD"},
                                new Order() {Type = "Book"},
                            };

            var bookDao = Substitute.For<IBookDao>();

            target.BookDao = bookDao;
            target.SyncBookOrders();

            bookDao.Received(2)
                   .Insert(Arg.Is<Order>(order => order.Type == "Book"));

            bookDao.DidNotReceive()
                   .Insert(Arg.Is<Order>(order => order.Type == "CD"));
        }
    }

    public class OrderServiceForTest : OrderService
    {
        private List<Order> _orders;
        private IBookDao _bookDao;

        protected override IBookDao GetBookDao()
        {
            return _bookDao;
        }

        public List<Order> Orders
        {
            set => _orders = value;
        }

        public IBookDao BookDao
        {
            set => _bookDao = value;
        }

        protected override List<Order> GetOrders()
        {
            return _orders;
        }
    }
}