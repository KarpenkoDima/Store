using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using store;
using store.Messages;
using Store.Web.Model;
using Store.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository, INotificationService notificationService)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
        }

        [HttpGet]
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

        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }
            return (order, cart);
        }

        [HttpPost]
        public IActionResult UpdateItem(int bookId, int count)
        {
            var book = bookRepository.GetById(bookId);

            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            order.Get(bookId).Count = count;
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

            return RedirectToAction("Index", "Book", new { id = bookId });
        }

        [HttpPost]
        public IActionResult AddItem(int bookId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            var book = bookRepository.GetById(bookId);
            order.AddOrUpdateItem(book, count);

            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Book", new { id = bookId });

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

       /* public IActionResult RemoveBook(int id)
        {
            var book = bookRepository.GetById(id);
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            order.Get(id).Count--;
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Book", new { id = id });
        }*/

        public IActionResult RemoveItem(int bookId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            order.RemoveItem(bookId);
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Order");
        }
        [HttpPost]
        public IActionResult SendConfirmationCode(int orderId, string cellPhone)
        {
            var order = orderRepository.GetById(orderId);
            var model = Map(order);

            if (!IsValidCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Phone number must been in fromat +381234567890";
                return View("Index", model);
            }
            int code = 1111;
            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);

            return View("Confirmation",
                new ConfirmatiopnModel
                {
                    OrderId = orderId,
                    CellPhone = cellPhone
                });
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            if (cellPhone == null)
            {
                return false;
            }

            cellPhone = cellPhone.Replace(" ", "").Replace("-", "");
            return Regex.IsMatch(cellPhone, @"^\+?\d{12}$");
        }
        [HttpPost]
        public IActionResult StartDelivery(int orderId, string cellPhone, int codeConfirm)
        {
            int? storedCode = HttpContext.Session.GetInt32(cellPhone);
            if (storedCode == null)
            {
                return View("Confirmation", new ConfirmatiopnModel
                {
                    OrderId = orderId,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>
                    {
                        {"code", "Empty confirm code. Repeat again" }
                    }
                });
            }

            if (storedCode != codeConfirm)
            {
                return View("Confirmation",
                    new ConfirmatiopnModel
                    {
                        OrderId = orderId,
                        CellPhone = cellPhone,
                        Errors = new Dictionary<string, string>
                        {
                            {"code", " is defferent from the one sent" }
                        }
                    });
            }

            return View();
        }
    }
}
