using Microsoft.AspNetCore.Mvc;
using store;
using Store.Web.Models;
using System.Linq;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                OrderModel model = Map(order);
                return View(model);
            }
            return View("Empty");
        }

        public IActionResult AddItem(int id)
        {
            var book = bookRepository.GetById(id);
            Order order;
            Cart cart;
            if (HttpContext.Session.TryGetCart(out cart))
            {
                order = orderRepository.GetById(cart.OrderId);                
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }
            order.AddItem(book, 1);
            orderRepository.Update(order);
            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;

            /*if (!HttpContext.Session.TryGetCart(out cart))
            {
                cart = new Cart(); ;
            }
            if (cart.Items.ContainsKey(id))
            {
                cart.Items[id]++;
            }
            else
            {
                cart.Items[id] = 1;
            }
            cart.Amount += book.Price;*/
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Book", new { id= id });
        }

        private OrderModel Map(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            var books = bookRepository.GetAllByIds(bookIds);
            var itemModels = from item in order.Items
                             join book in books on item.BookId equals book.Id
                             select new OrderItemModel
                             {
                                 BookId = book.Id,
                                 Title = book.Title,
                                 Author = book.Author,
                                 Price = item.Price,
                                 Count =item.Count
                             };
            return
                new OrderModel
                {
                    Id = 1,
                    Items = itemModels.ToArray(),
                    TotalCount = order.TotalCount,
                    TotalPrice = order.TotalPrice
                };
        }
    }
}
