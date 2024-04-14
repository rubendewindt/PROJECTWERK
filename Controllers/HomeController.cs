using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectwerk.Data;
using projectwerk.Models;
using System.Diagnostics;


namespace projectwerk.Controllers
{


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private bool ordersDeleted = false;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginData loginData)
        {
            using (var context = new ProjectwerkContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == loginData.Email && u.Password == loginData.Password);
                if (user != null)
                {
                    // User authenticated, redirect to a secure page
                    return RedirectToAction("Producten");
                }
                else
                {
                    // Invalid credentials, return to login page with error message
                    ModelState.AddModelError(string.Empty, "Invalid email or password");
                    return View("Index");
                }
            }
        }


        public IActionResult Registratie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registratie(User user)
        {
            using (ProjectwerkContext context = new ProjectwerkContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return View("Index");
        }


        public IActionResult Winkelkarretje()
        {
            using (var context = new ProjectwerkContext())
            {
                var orderDetails = context.OrderDetails.ToList();
                return View(orderDetails);
            }
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            using (ProjectwerkContext context = new ProjectwerkContext())
            {

                var orderDetails = context.OrderDetails.ToList();
                foreach (var orderDetail in orderDetails)
                {
                    // Create a new Order object using data from OrderDetail
                    Order order = new Order()
                    {
                        Name = orderDetail.Name,
                        Price = orderDetail.Price,
                        Quantity = orderDetail.Quantity,
                        OrderPlaced = DateTime.Now // Set the order placed time to current datetime
                    };

                    // Add the new order to the context
                    context.Orders.Add(order);

                }

                context.SaveChanges();

                // Save changes to the context to add new orders

                context.OrderDetails.RemoveRange(context.OrderDetails);
                context.SaveChanges();

                return RedirectToAction("Winkelkarretje");

            }
        }

        [HttpPost]
        public IActionResult DeleteSelected(int[] selectedItems)
        {

            using (ProjectwerkContext context = new ProjectwerkContext())
            {


                var itemsToRemove = context.OrderDetails.Where(p => selectedItems.Contains(p.Id));
                context.OrderDetails.RemoveRange(itemsToRemove);
                context.SaveChanges();



                return RedirectToAction("Winkelkarretje");
            }
        }

        public IActionResult Producten()
        {

            using (ProjectwerkContext context = new ProjectwerkContext())
            {
                var products = context.Products.ToList();
                return View(products);
            }

        }


        [HttpPost]
        public IActionResult Increment(int id)
        {
            using (var context = new ProjectwerkContext())
            {
                var product = context.Products.Find(id);
                if (product != null)
                {
                    product.Quantity++;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Producten");
        }

        [HttpPost]
        public IActionResult Decrement(int id)
        {

            using (var context = new ProjectwerkContext())
            {
                var product = context.Products.Find(id);
                if (product != null && product.Quantity > 0)
                {
                    product.Quantity--;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Producten");
        }
        [HttpPost]
        public IActionResult AddDataOrderDetails(int id)
        {
            using (var context = new ProjectwerkContext())
            {
                var product = context.Products.Find(id);

                if (product != null && product.Quantity > 0)
                {
                    OrderDetail item = new OrderDetail()
                    {
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = product.Quantity,
                    };

                    context.OrderDetails.Add(item); // Add the new order detail to the context
                    product.Quantity = 0; // Reset product quantity
                    context.SaveChanges(); // Save changes to the context
                }
            }

            return RedirectToAction("Producten");
        }

        public IActionResult AllOrders()
        {
            using (var context = new ProjectwerkContext())
            {
                var orders = context.Orders.ToList(); // Retrieve all orders from the database

                var time = DateTime.Now;
                if (time.Hour == 14 && time.Minute == 30 && !ordersDeleted)
                {
                    context.Orders.RemoveRange(context.Orders);
                    context.SaveChanges();
                    ordersDeleted = true;

                    // Redirect to a JavaScript function that will reload the page

                }

                return View(orders); // Pass the list of orders to the view
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}