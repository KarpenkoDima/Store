using Microsoft.AspNetCore.Mvc;
using store;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        public CartController(IBookRepository bookRepository, IOrderRepository orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }
        public IActionResult Add(int id)
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
    }
}
