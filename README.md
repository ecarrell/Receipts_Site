# Receipts Site

### Created by Ethan Carrell

Repo Link: https://github.com/ecarrell/Receipts_Site.git

## Summary:
A basic website that takes input for shopping baskets and returns receipts in the format shown below, calculating
all taxes and totals correctly.

"# Product Name at Price"  
1 Chocolate bar at 0.85  
1 Imported bottle of perfume at 27.99  

#### You will need to download jquery for the website to function properly. I used npm manager in Visual Studio to accomplish this.  
```
npm install jquery
```


## Code design:
Website delivers the input via an ajax call (provided by jQuery) to the controller. The controller has a separate 
function that processes and validates all of the input. Once the input is validated, a cart instance is created 
and modified via the Cart class. The Cart class stores information about the list of products in the cart, 
each product's running tax, product quantity, and the product running subtotal (untaxed). The list of products 
is a list of Product class instances. The Product class has a Name and Price. The Cart class has several public 
methods that facilitate information gathering for the controller when the output is being constructed. The Cart 
class uses the Tax class (static type) to calculate the sales tax and import tax when applicable. Once the 
controller is done building the output string then the original method that was called returns the string as 
JSON. The ajax success callback function then feeds the output into the Output textarea.

#### Input assumptions:
1. The quantity is < 10
2. The "at" word is lowercase and has a leading and trailing space.
3. The "Imported" word has a trailing space.

#### Input validation response:
The input line is skipped if the quantity or price cannot be parsed. This allows the other products to be
individually processed without any relation to another product in the input.

#### Product assumptions:
The code is designed to add a product to all Cart class structures with an "all or nothing" strategy. Either
all of the product information exists (therefore is added/modified in structures) or the product is ignored.
