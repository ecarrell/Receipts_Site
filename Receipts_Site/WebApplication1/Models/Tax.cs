using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public static class Tax
    {
        private static double SalesTaxRate = 0.1;
        private static string ImportKeyWord = "Imported";
        private static double ImportTaxRate = 0.05;
        private static List<string> ExemptItems = new List<string> { "Books", "Chocolate", "Pills" };

        /// <summary>
        /// Add applicable taxes to a specific product
        /// </summary>
        /// <param name="subTotal">Product's subtotal</param>
        /// <param name="productName">Product name</param>
        /// <returns></returns>
        public static double GetProductTax (double subTotal, string productName)
        {
            if (!IsTaxExempt(productName))
            {
                // Add tax
                subTotal += subTotal * SalesTaxRate;
            }

            if (IsImportProduct(productName))
            {
                // Add import tax
                subTotal += subTotal * ImportTaxRate;
            }

            // Round to the nearest 5 cents
            return Math.Round((Math.Round(subTotal * 20, MidpointRounding.AwayFromZero) / 20), 1);
        }

        /// <summary>
        /// Checks for the product name in the tax exemption list
        /// </summary>
        /// <param name="name">Name of the product</param>
        /// <returns>True if the product name is tax exempt</returns>
        private static bool IsTaxExempt (string name)
        {
            return ExemptItems.Contains(name);
        }

        /// <summary>
        /// Checks the name for the imported key word
        /// </summary>
        /// <param name="name">Name of the product</param>
        /// <returns>True if the product is imported</returns>
        private static bool IsImportProduct(string name)
        {
            return name.Contains(" " + ImportKeyWord + " ");
        }
    }
}