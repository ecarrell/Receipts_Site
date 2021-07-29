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
        private static List<string> ExemptItems = new List<string> { "Book", "Chocolate", "Pills" };

        /// <summary>
        /// Add applicable taxes to a specific product
        /// </summary>
        /// <param name="subTotal">Product's subtotal</param>
        /// <param name="productName">Product name</param>
        /// <returns></returns>
        public static double GetProductTax (Product item)
        {
            double taxAmount = 0;

            if (!IsTaxExempt(item.Name))
            {
                // Add tax
                taxAmount += item.Price * SalesTaxRate;
            }

            if (IsImportProduct(item.Name))
            {
                // Add import tax
                taxAmount += item.Price * ImportTaxRate;
            }

            if (taxAmount > 0)
                // Round up to the nearest 5 cents
                return Math.Round((Math.Ceiling(taxAmount * 20) / 20), 2);
            else
                return taxAmount;
        }

        /// <summary>
        /// Checks for the product name in the tax exemption list
        /// </summary>
        /// <param name="name">Name of the product</param>
        /// <returns>True if the product name is tax exempt</returns>
        private static bool IsTaxExempt (string name)
        {
            foreach (string exemption in ExemptItems)
            {
                if (name.ToUpper().Contains(exemption.ToUpper()))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks the name for the imported key word
        /// </summary>
        /// <param name="name">Name of the product</param>
        /// <returns>True if the product is imported</returns>
        private static bool IsImportProduct(string name)
        {
            return name.ToUpper().Contains(ImportKeyWord.ToUpper() + " ");
        }
    }
}