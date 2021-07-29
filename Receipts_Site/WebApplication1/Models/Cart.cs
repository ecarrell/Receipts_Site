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

        #region Public methods

        /// <summary>
        /// Adds a product to the cart or updates the product in the cart
        /// </summary>
        /// <param name="itemName">Name of the product</param>
        /// <param name="quantity">Quantity of the product</param>
        /// <param name="price">Price of the product</param>
        public void AddProductToCart(string itemName, int quantity, double price)
        {
            if (Products.Where(x => x.Name == itemName).Any())
            {
                // Update product
                UpdateProduct(itemName, quantity);
            }
            else
            {
                // Add product
                AddProduct(itemName, quantity, price);
            }
        }

        /// <summary>
        /// Gets the total amount of tax
        /// </summary>
        public double GetTotalTax()
        {
            double totalTax = 0;

            foreach (var item in ProductTax)
            {
                totalTax += item.Value;
            }

            return totalTax;
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

            total += GetTotalTax();
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
        public double GetSubTotal (Product item)
        {
            if (!(item == null || String.IsNullOrEmpty(item.Name)))
            {
                if (ProductSubTotals.ContainsKey(item.Name))
                {
                    return ProductSubTotals[item.Name];
                }
                else
                    return 0;
            }
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
                {
                    return Quantities[item.Name];
                }
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
        }

        private void UpdateProduct(string itemName, int quantity)
        {
            Product product = Products.Where(x => x.Name == itemName).FirstOrDefault();
            SetProductQuantity(product, quantity);
            SetProductSubTotal(product);
        }

        /// <summary>
        /// Sets the subtotal for a product
        /// </summary>
        /// <param name="item">Product</param>
        private void SetProductSubTotal (Product item)
        {
            if (item != null && item.Name != null)
            {
                double productTax;

                if (Quantities.ContainsKey(item.Name))
                {
                    // Update Subtotal
                    ProductSubTotals[item.Name] = Tax.GetProductTax(item.Price * (double)Quantities[item.Name], item.Name);
                }
                else
                {
                    // Add Subtotal
                    productTax = Tax.GetProductTax(item.Price * (double)Quantities[item.Name], item.Name);
                    ProductSubTotals.Add(item.Name, productTax);
                }
            }
        }

        /// <summary>
        /// Sets the quantity for a product
        /// </summary>
        /// <param name="item">Product</param>
        /// <param name="quantity">Quantity of the product</param>
        private void SetProductQuantity (Product item, int quantity)
        {
            if (item != null && item.Name != null && quantity > 1)
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
        private void SetProductTax (Product item)
        {
            if (item != null && item.Price > 0)
            {
                if (ProductTax.ContainsKey(item.Name))
                {
                    // Update tax
                    ProductTax[item.Name] = Tax.GetProductTax(item.Price * (double)Quantities[item.Name], item.Name);
                }
                else
                {
                    // Add tax
                    ProductTax.Add(item.Name, Tax.GetProductTax(item.Price * (double)Quantities[item.Name], item.Name));
                }                
            }
        }
        #endregion
    }
}