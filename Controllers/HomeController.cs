using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectwerk.Data;
using projectwerk.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace projectwerk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProjectwerkContext _context;
        private bool ordersDeleted = false;

        public HomeController(ILogger<HomeController> logger, ProjectwerkContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginData loginData)
        {
            _logger.LogInformation($"Login attempt for user {loginData.Email} at {DateTime.Now}.");

            var user = _context.Users.FirstOrDefault(u => u.Email == loginData.Email && u.Password == loginData.Password);
            if (user != null)
            {
                if (!user.IsApproved && user.Email != "admin@admin.com")
                {
                    _logger.LogWarning($"User {loginData.Email} is not approved.");
                    ModelState.AddModelError(string.Empty, "Your account is not approved yet.");
                    return View("Index");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                _logger.LogInformation($"User {user.Email} logged in successfully at {DateTime.Now}.");

                return RedirectToAction("Producten");
            }
            else
            {
                _logger.LogWarning($"User with email {loginData.Email} not found.");
                _logger.LogWarning($"Invalid login attempt for user {loginData.Email} at {DateTime.Now}.");
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View("Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult Registratie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registratie(User user)
        {
            user.IsApproved = false; // Ensure new users are not approved by default
            _context.Users.Add(user);
            _context.SaveChanges();

            return View("Index");
        }

        [Authorize]
        public IActionResult Winkelkarretje()
        {
            var orderDetails = _context.OrderDetails.ToList();
            return View(orderDetails);
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            var orderDetails = _context.OrderDetails.ToList();
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
                _context.Orders.Add(order);
            }

            _context.SaveChanges();

            // Save changes to the context to add new orders
            _context.OrderDetails.RemoveRange(_context.OrderDetails);
            _context.SaveChanges();

            return RedirectToAction("Winkelkarretje");
        }

        [HttpPost]
        public IActionResult DeleteSelected(int[] selectedItems)
        {
            var itemsToRemove = _context.OrderDetails.Where(p => selectedItems.Contains(p.Id));
            _context.OrderDetails.RemoveRange(itemsToRemove);
            _context.SaveChanges();

            return RedirectToAction("Winkelkarretje");
        }

        [Authorize]
        public IActionResult Producten()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Increment(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                product.Quantity++;
                _context.SaveChanges();
            }
            return RedirectToAction("Producten");
        }

        [HttpPost]
        public IActionResult Decrement(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null && product.Quantity > 0)
            {
                product.Quantity--;
                _context.SaveChanges();
            }
            return RedirectToAction("Producten");
        }

        [HttpPost]
        public IActionResult AddDataOrderDetails(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null && product.Quantity > 0)
            {
                OrderDetail item = new OrderDetail()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                };

                _context.OrderDetails.Add(item); // Add the new order detail to the context
                product.Quantity = 0; // Reset product quantity
                _context.SaveChanges(); // Save changes to the context
            }

            return RedirectToAction("Producten");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AllOrders()
        {
            var orders = _context.Orders.ToList(); // Retrieve all orders from the database

            var time = DateTime.Now;
            if (time.Hour == 14 && time.Minute == 30 && !ordersDeleted)
            {
                _context.Orders.RemoveRange(_context.Orders);
                _context.SaveChanges();
                ordersDeleted = true;
            }

            return View(orders); // Pass the list of orders to the view
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UserManagement()
        {
            var pendingUsers = _context.Users.Where(u => !u.IsApproved).ToList();
            return View(pendingUsers);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ApproveUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.IsApproved = true;
                _context.SaveChanges();
            }
            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult RejectUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("UserManagement");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageRoles()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignRole(int userId, string role)
        {
            var user = _context.Users.Find(userId);
            if (user != null && user.Email != "admin@admin.com")
            {
                user.Role = role;
                _context.SaveChanges();
            }
            return RedirectToAction("ManageRoles");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUsers()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("DeleteUsers");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser(User user)
        {
            user.IsApproved = true; // Automatically approve new users created by admin
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("UserManagement");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageOrders()
        {
            var orders = _context.Orders.ToList();

            // Ensure default order exists
            if (!orders.Any(o => o.Id == 0))
            {
                orders.Insert(0, new Order
                {
                    Id = 0,
                    Name = "Default Order",
                    Quantity = 1,
                    Price = 0m,
                    OrderPlaced = DateTime.MinValue
                });
            }

            return View(orders);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditOrderQuantity(int id, int quantity)
        {
            _logger.LogInformation($"EditOrderQuantity called with id: {id}, quantity: {quantity}");
            var existingOrder = _context.Orders.Find(id);
            if (existingOrder != null)
            {
                existingOrder.Quantity = quantity;
                _context.SaveChanges();
                _logger.LogInformation("Order quantity updated successfully.");
            }
            else
            {
                _logger.LogWarning($"Order with id: {id} not found.");
            }
            return RedirectToAction("ManageOrders");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteOrder(int id)
        {
            if (id == 0)
            {
                _logger.LogWarning("Attempt to delete default order ignored.");
                return RedirectToAction("ManageOrders");
            }

            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageOrders");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSelectedOrders(int[] selectedOrderIds)
        {
            var ordersToDelete = _context.Orders.Where(o => selectedOrderIds.Contains(o.Id) && o.Id != 0).ToList();
            _context.Orders.RemoveRange(ordersToDelete);
            _context.SaveChanges();
            return RedirectToAction("ManageOrders");
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











