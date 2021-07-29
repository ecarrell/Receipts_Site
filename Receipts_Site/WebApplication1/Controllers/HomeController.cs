using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProcessInput (string text)
        {
            return Json(CreateOutput(text), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Main function that handles the processing of the data
        /// </summary>
        /// <param name="input">Data string</param>
        /// <returns>A string for the view</returns>
        public string CreateOutput(string input)
        {
            string[] lines = input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Cart myCart = new Cart();
            double price = 0;
            int quantity = 0;
            string output = "";

            foreach (string s in lines)
            {
                // Format is "{ Quantity } { Product Name } at { Product Price }
                try
                {
                    double.TryParse(s.Substring(s.IndexOf(" at ") + 4, s.Length - (s.IndexOf(" at ") + 4)), out price);
                    Int32.TryParse(s.Substring(0, 1), out quantity);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }

                if (price > 0 && quantity > 0)
                    myCart.AddProductToCart(quantity, s.Substring(1, s.IndexOf(" at ") - 1).Trim(), price);
            }

            List<Product> cartProducts;
            cartProducts = myCart.GetAllProducts();

            if (cartProducts.Count < 1)
                return "No products were entered or there is a problem with the input text.";

            int productQuantity = 0;

            // Build the output string
            for (int i = 0; i < cartProducts.Count; i++)
            {
                output += cartProducts[i].Name + ": " + myCart.GetSubTotalWithTax(cartProducts[i]).ToString("0.00");

                productQuantity = myCart.GetProductQuantity(cartProducts[i]);

                if (productQuantity > 1)
                    output += " (" + productQuantity + " @ " + myCart.GetProductPriceWithTax(cartProducts[i]).ToString("0.00") + ")";

                output += "\n";
            }

            output += "Sales Taxes: " + myCart.GetSalesTax().ToString("0.00") + "\n";
            output += "Total: " + myCart.GetTotal().ToString("0.00");

            return output;
        }
    }
}