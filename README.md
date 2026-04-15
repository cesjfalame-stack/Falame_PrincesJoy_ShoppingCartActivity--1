# Falame_PrincesJoy_ShoppingCartActivity-

Project Overview
This is my Shopping Cart System made with C#. I used classes and objects to manage products and an array to handle the cart. The program checks for stock, validates user input, and handles discounts automatically.

Key Features
When it comes to the cart, theres a check for duplicates. So if you try to add the same item again, it doesnt make a new entry. It just bumps up the quantity on the existing one. That keeps things cleaner.
The cart itself is limited to 10 unique items. I set it that way to keep it simple, not sure if thats the best number though.
On discounts, if the total comes to 5000 pesos or higher, it automatically takes off 10 percent

AI Usage in This Project
I used AI a few times during the project when I got stuck. One issue was the receipt showing “{grandTotal}” instead of the actual value, which I fixed by using C# string interpolation with $.
I also used AI to check if I missed anything in the requirements, and I realized I needed to adjust the discount rule so it only applies at ₱5,000. Another prompt helped me handle duplicate items in a fixed-size cart array.
After that, I updated my code: fixed the receipt display, corrected the discount threshold, moved stock checking and deduction into methods in the Product class, and added validation to limit the cart to 10 unique items.
