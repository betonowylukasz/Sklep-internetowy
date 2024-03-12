using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace lista10.Controllers
{
    public class Cart
    {
        private Dictionary<int, int> _items = new Dictionary<int, int>();

        public void AddToCart(int articleId)
        {
            if (_items.ContainsKey(articleId))
            {
                _items[articleId]++;
            }
            else
            {
                _items[articleId] = 1;
            }
        }

        public bool IsInCart(int articleId)
        {
            return _items.ContainsKey(articleId);
        }

        public void UpdateQuantity(int articleId, int quantity)
        {
            if (_items.ContainsKey(articleId))
            {
                _items[articleId] = quantity;
            }
        }

        public void RemoveOneFromCart(int articleId)
        {
            if (_items[articleId] == 1)
            {
                _items.Remove(articleId);
            }
            else
            {
                _items[articleId]-- ;
            }
        }

        public void RemoveFromCart(int articleId)
        {
            _items.Remove(articleId);
        }

        public Dictionary<int, int> GetItems()
        {
            return _items;
        }

        public void SetItems(Dictionary<int, int> items)
        {
            _items = items;
        }

        public void SaveToCookies(HttpContext context)
        {
            var cartJson = JsonConvert.SerializeObject(_items);
            context.Response.Cookies.Append("cart", cartJson, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true
            });
        }

        public Cart LoadFromCookies(HttpContext context)
        {
            var cart = new Cart();
            var cartJson = context.Request.Cookies["cart"];
            if (!string.IsNullOrEmpty(cartJson))
            {
                try
                {
                    cart._items = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartJson);
                }
                catch (JsonException)
                {
                    // Obsłuż błąd deserializacji, na przykład pozostaw słownik pusty
                }
            }
            return cart;
        }

        public void UpdateQuantityAndRemoveIfZero(int articleId, int quantity)
        {
            if (_items.ContainsKey(articleId))
            {
                if (quantity > 0)
                {
                    _items[articleId] = quantity;
                }
                else
                {
                    _items.Remove(articleId);
                }
            }
        }

        public void Clear ()
        {
            _items.Clear();
        }
    }
}
