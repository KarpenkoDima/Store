﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using store;
using store.Contractors;
using store.Messages;
using Store.Contractors;
using Store.Web.Contractors;
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
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly IEnumerable<IWebContractorService> webContractorServices;
        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository, INotificationService notificationService, IEnumerable<IDeliveryService> notificationSedrvices, IEnumerable<IPaymentService> paymentServices, IEnumerable<IWebContractorService> webContractorServices)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.deliveryServices = notificationSedrvices;
            this.paymentServices = paymentServices;
            this.webContractorServices = webContractorServices;
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
        public IActionResult Confirmate(int orderId, string cellPhone, int codeConfirm)
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
            // todo: save cellPhone
            var order = orderRepository.GetById(orderId);
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            HttpContext.Session.Remove(cellPhone);
            var model = new DeliveryModel
            {
                OrderId = orderId,
                Methods = deliveryServices.ToDictionary(service => service.Code, service => service.Title)
            };
            return View("DeliveryMethod", model);
        }

        [HttpPost]
        public IActionResult StartDelivery(int id, string uniqueCode)
        {
            var deliveryService = deliveryServices.Single(service => service.Code == uniqueCode);
            var order = orderRepository.GetById(id);
            var form = deliveryService.CreateForm(order);
            return View("DeliveryStep", form);
        }
        [HttpPost]
        public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var deliveryService = deliveryServices.Single(service => service.Code == uniqueCode);
            var form = deliveryService.MoveNextForm(id, step, values);
            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Delivery = deliveryService.CreateDelivery(form);
                orderRepository.Update(order);

                var model = new DeliveryModel
                {
                    OrderId = id,
                    Methods = paymentServices.ToDictionary(service => service.Code, service => service.Title)
                };
                return View("PaymentMethod", model);
            }
            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult StartPayment(int id, string uniqueCode)
        {
            var paymentService = paymentServices.Single(service => service.Code == uniqueCode);
            var order = orderRepository.GetById(id);
            var form = paymentService.CreateForm(order);
            var webContractorService = webContractorServices.SingleOrDefault(server => server.Code == uniqueCode);
            if (webContractorService!=null)
            {
                return Redirect(webContractorService.GetUri);
            }
            return View("PaymentStep", form);
        }
        [HttpPost]
        public IActionResult NextPayment(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Code == uniqueCode);
            var form = paymentService.MoveNextForm(id, step, values);
            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Payment = paymentService.GetPayment(form);
                orderRepository.Update(order);                
                return View("Finish");
            }
            return View("PaymentStep", form);
        }

        public IActionResult Finish()
        {
            HttpContext.Session.RemoveCart();
            return View();
        }
    }
}
