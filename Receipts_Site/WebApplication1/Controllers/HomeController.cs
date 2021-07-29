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
            //Output model = CreateOutput(text);
            //return View(model);
            return Json(CreateOutput(text), JsonRequestBehavior.AllowGet);
        }

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
                bool skipLine = false;

                try
                {
                    double.TryParse(s.Substring(s.IndexOf(" at ") + 4, s.Length - (s.IndexOf(" at ") + 4)), out price);
                    Int32.TryParse(s.Substring(0, 1), out quantity);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                    skipLine = true;
                }

                if (skipLine == false)
                    myCart.AddProductToCart(quantity, s.Substring(2, s.IndexOf(" at ") - 1).Trim(), price);
            }

            List<Product> cartProducts;
            cartProducts = myCart.GetAllProducts();
            int productQuantity = 0;

            // Build the output string
            for (int i = 0; i < cartProducts.Count; i++)
            {
                output += cartProducts[i].Name + ": " + myCart.GetSubTotal(cartProducts[i]);

                productQuantity = myCart.GetProductQuantity(cartProducts[i]);

                if (productQuantity > 1)
                    output += " (" + productQuantity + " @ " + cartProducts[i].Price + ")";

                output += "\n";
            }

            output += "Sales Taxes: " + myCart.GetSalesTax() + "\n";
            output += "Total: " + myCart.GetTotal();

            return output;
        }
    }
}