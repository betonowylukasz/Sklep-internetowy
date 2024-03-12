using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lista10.Data;
using lista10.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace lista10.Controllers
{
    public class ShopController : Controller
    {
        private readonly MyDbContext _context;

        public ShopController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Shop
        public IActionResult Index(int? categoryId)
        {
            var categories = _context.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            // Pobierz listę towarów dla wybranej kategorii (lub wszystkich towarów, jeśli nie wybrano kategorii)
            var articles = categoryId.HasValue
                ? _context.Article.Include(a => a.Category).Where(a => a.CategoryId == categoryId).ToList()
                : _context.Article.Include(a => a.Category).ToList();

            ViewBag.SelectedCategory = categoryId;

            if (categoryId.HasValue)
            {
                TempData["SelectedCategory"] = categoryId.Value;
            }

            return View(articles);
        }

        // GET: Shop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Shop/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Shop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ExpiryDate,CategoryId,ImagePath")] Article article)
        {
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Shop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // POST: Shop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ExpiryDate,CategoryId,ImagePath")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Shop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Shop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Article.FindAsync(id);
            _context.Article.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }

        public IActionResult AddToCart(int articleId, int? categoryId)
        {
            var ncart = new Cart();
            var cart = ncart.LoadFromCookies(HttpContext);

            // Sprawdź, czy artykuł jest już w koszyku
            cart.AddToCart(articleId);
            cart.SaveToCookies(HttpContext);

            // Przekieruj użytkownika z powrotem do strony towarów
            return RedirectToAction("Index", new { categoryId });
        }

        public IActionResult Add(int articleId)
        {
            var ncart = new Cart();
            var cart = ncart.LoadFromCookies(HttpContext);

            // Sprawdź, czy artykuł jest już w koszyku
            cart.AddToCart(articleId);
            cart.SaveToCookies(HttpContext);

            // Przekieruj użytkownika z powrotem do strony towarów
            return RedirectToAction("Cart");
        }

        public IActionResult RemoveOne(int articleId)
        {
            var ncart = new Cart();
            var cart = ncart.LoadFromCookies(HttpContext);

            // Sprawdź, czy artykuł jest już w koszyku
            cart.RemoveOneFromCart(articleId);
            cart.SaveToCookies(HttpContext);

            // Przekieruj użytkownika z powrotem do strony towarów
            return RedirectToAction("Cart");
        }

        public IActionResult Remove(int articleId)
        {
            var ncart = new Cart();
            var cart = ncart.LoadFromCookies(HttpContext);

            // Sprawdź, czy artykuł jest już w koszyku
            cart.RemoveFromCart(articleId);
            cart.SaveToCookies(HttpContext);

            // Przekieruj użytkownika z powrotem do strony towarów
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            var ncart = new Cart();
            var cart = ncart.LoadFromCookies(HttpContext);
            var articlesInCart = GetArticlesInCart(cart);

            return View(articlesInCart);
        }

        private List<CartItem> GetArticlesInCart(Cart cart)
        {
            var articlesInCart = new List<CartItem>();

            foreach (var item in cart.GetItems())
            {
                var article = _context.Article.Find(item.Key);

                if (article != null)
                {
                    var cartItem = new CartItem
                    {
                        Article = article,
                        Quantity = item.Value
                    };
                    articlesInCart.Add(cartItem);
                }
            }

            return articlesInCart;
        }

        public IActionResult SetUserRole(string role)
        {
            // Zapisz wybraną rolę do ciasteczka
            HttpContext.Response.Cookies.Append("UserRole", role, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true
            });

            ViewBag.UserRole = role;

            // Przekieruj z powrotem do akcji Index
            return RedirectToAction("Index");
        }

        public IActionResult Order()
        {
            // Pobierz informacje o koszyku użytkownika
            var ncart = new Cart();
            var cart = ncart.LoadFromCookies(HttpContext);

            // Przygotuj model zamówienia
            var orderViewModel = new Order
            {
                OrderDetails = GetArticlesInCart(cart),
            };

            return View(orderViewModel);
        }

        [HttpPost]
        public IActionResult SubmitOrder(Order order)
        {
            order.IsConfirmed = true;
            order.ExpiryMagazineDate = DateTime.Now.AddMonths(1);

            //order.Price = order.OrderDetails.Sum(item => (decimal.TryParse(item.Article.Price, out var price) ? price : 0) * item.Quantity);

            // Reszta pól pozostanie uzupełniona na podstawie danych z formularza

            // Zapis zamówienia do bazy danych
            _context.Order.Add(order);
            _context.SaveChanges();

            var ncart = new Cart();
            var cart = ncart.LoadFromCookies(HttpContext);
            cart.Clear();
            cart.SaveToCookies(HttpContext);

            // Przekierowanie do strony potwierdzenia zamówienia
            return RedirectToAction("OrderConfirmation", order);
        }

        public IActionResult OrderConfirmation(Order order)
        {
            return View(order);
        }
    }
}
