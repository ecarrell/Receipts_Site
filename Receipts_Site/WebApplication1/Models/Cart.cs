using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Cart
    {
        private Dictionary<string, int> Quantities { get; set; }
        private Dictionary<string, double> ProductSubTotals { get; set; }
        private Dictionary<string, double> ProductTax { get; set; }
        private List<Product> Products { get; set; }

        public Cart()
        {
            Quantities = new Dictionary<string, int>();
            ProductSubTotals = new Dictionary<string, double>();
            ProductTax = new Dictionary<string, double>();
            Products = new List<Product>();
        }

        #region Public methods

        /// <summary>
        /// Adds a product to the cart or updates the product in the cart
        /// </summary>
        /// <param name="itemName">Name of the product</param>
        /// <param name="quantity">Quantity of the product</param>
        /// <param name="price">Price of the product</param>
        public void AddProductToCart(int quantity, string itemName, double price)
        {
            if (Products.Where(x => x.Name == itemName).Any())
                // Update product
                UpdateProduct(itemName, quantity);
            else
                // Add product
                AddProduct(itemName, quantity, price);
        }

        /// <summary>
        /// Gets the total amount of tax
        /// </summary>
        public double GetSalesTax()
        {
            double salesTax = 0;

            foreach (var item in ProductTax)
            {
                salesTax += item.Value;
            }

            return salesTax;
        }

        /// <summary>
        /// Gets the total price
        /// </summary>
        public double GetTotal()
        {
            double total = 0;

            foreach (var item in ProductSubTotals)
            {
                total += item.Value;
            }

            total += GetSalesTax();
            return total;
        }

        /// <summary>
        /// Gets all the products in the cart
        /// </summary>
        /// <returns>List of products</returns>
        public List<Product> GetAllProducts ()
        {
            return Products;
        }

        /// <summary>
        /// Gets the subtotal for a product
        /// </summary>
        /// <param name="item">Product</param>
        /// <returns>Product's subtotal</returns>
        public double GetSubTotalWithTax (Product item)
        {
            double total = 0;

            if (!(item == null || String.IsNullOrEmpty(item.Name)))
            {
                if (ProductSubTotals.ContainsKey(item.Name))
                    total += ProductSubTotals[item.Name];
                else
                    return total;

                if (ProductTax.ContainsKey(item.Name))
                    total += ProductTax[item.Name];
            }

            return total;
        }

        /// <summary>
        /// Gets the price of a product with tax included
        /// </summary>
        /// <param name="item">Product</param>
        /// <returns>Product's price with tax included</returns>
        public double GetProductPriceWithTax (Product item)
        {
            if (!(item == null || String.IsNullOrEmpty(item.Name)))
                return (ProductTax[item.Name] / (double)Quantities[item.Name]) + item.Price;
            else
                return 0;
        }

        /// <summary>
        /// Gets the quantity for a product
        /// </summary>
        /// <param name="item">Product</param>
        /// <returns>Product's quantity</returns>
        public int GetProductQuantity (Product item)
        {
            if (!(item == null || String.IsNullOrEmpty(item.Name)))
            {
                if (Quantities.ContainsKey(item.Name))
                    return Quantities[item.Name];
                else
                    return 0;
            }
            else
                return 0;
        }

        #endregion
        #region Private methods

        private void AddProduct(string itemName, int quantity, double price)
        {
            Product newProduct = new Product() { Name = itemName, Price = price };
            Products.Add(newProduct);
            SetProductQuantity(newProduct, quantity);
            SetProductSubTotal(newProduct);
            SetProductTax(newProduct, 0);
        }

        private void UpdateProduct(string itemName, int quantity)
        {
            Product product = Products.Where(x => x.Name == itemName).FirstOrDefault();
            SetProductQuantity(product, quantity);
            SetProductSubTotal(product);
            SetProductTax(product, quantity);
        }

        /// <summary>
        /// Sets the subtotal for a product
        /// </summary>
        /// <param name="item">Product</param>
        private void SetProductSubTotal (Product item)
        {
            if (item != null && item.Name != null)
            {
                if (ProductSubTotals.ContainsKey(item.Name))
                    // Update subtotal
                    ProductSubTotals[item.Name] = item.Price * Quantities[item.Name];
                else
                    // Add subtotal
                    ProductSubTotals.Add(item.Name, item.Price);
            }
        }

        /// <summary>
        /// Sets the quantity for a product
        /// </summary>
        /// <param name="item">Product</param>
        /// <param name="quantity">Quantity of the product</param>
        private void SetProductQuantity (Product item, int quantity)
        {
            if (item != null && item.Name != null && quantity > 0)
            {
                if (Quantities.ContainsKey(item.Name))
                    // Update quantity
                    Quantities[item.Name] += quantity;
                else
                    // Add quantity
                    Quantities.Add(item.Name, quantity);
            }
        }

        /// <summary>
        /// Sets the tax for a product
        /// </summary>
        /// <param name="item">Product</param>
        private void SetProductTax (Product item, int quantity)
        {
            if (item != null && item.Price > 0)
            {
                if (ProductTax.ContainsKey(item.Name))
                    // Update tax
                    ProductTax[item.Name] = Tax.GetProductTax(item) * (double)Quantities[item.Name];
                else
                    // Add tax
                    ProductTax.Add(item.Name, Tax.GetProductTax(item));              
            }
        }
        #endregion
    }
}