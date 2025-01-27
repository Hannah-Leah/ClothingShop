using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace ClothingShop
{
    public partial class CheckoutWindow : Window
    {
        private List<Item> purchasedItems;
        private double remainingBudget;

        public CheckoutWindow(List<Item> items, double budget)
        {
            InitializeComponent();
            purchasedItems = items;
            remainingBudget = budget;

            // Display the purchased items in the ListBox
            PurchasedItemsList.ItemsSource = purchasedItems;

            // Calculate and display the total
            double total = 0;
            foreach (var item in purchasedItems)
            {
                total += item.Price;
            }

            TotalText.Text = $"Total: ${total:F2}";
        }

        private void ApplyCoupon_Click(object sender, RoutedEventArgs e)
        {
            double total = 0;
            foreach (var item in purchasedItems)
            {
                total += item.Price;
            }

            // Check if the coupon code is correct
            if (CouponTextBox.Text == "hello123")
            {
                double discount = total * 0.2;  // 20% discount
                double finalTotal = total - discount;
                DiscountText.Text = $"Discount Applied: ${discount:F2}. Final Total: ${finalTotal:F2}";
            }
            else
            {
                DiscountText.Text = "Invalid Coupon Code!";
            }
        }

        // Event handler for the save checkout button
        private void SaveCheckout_Click(object sender, RoutedEventArgs e)
        {

            FileStream s = new FileStream(@"C:\Users\Hanie\Downloads\ClothingShopCheckout.txt", FileMode.OpenOrCreate, FileAccess.Write);

            using (StreamWriter writer = new StreamWriter(s, Encoding.Unicode))
            {
                writer.WriteLine("Clothing Shop Checkout");
                writer.WriteLine("========================================");
                writer.WriteLine("Purchased Items:");

                // Write the purchased items and their prices
                double total = 0;
                foreach (var item in purchasedItems)
                {
                    writer.WriteLine($"{item.Name} - ${item.Price:F2}");
                    total += item.Price;
                }

                // Write the total cost
                writer.WriteLine("----------------------------------------");
                writer.WriteLine($"Total: ${total:F2}");

                // Apply discount if coupon is used
                if (CouponTextBox.Text == "hello123")
                {
                    double discount = total * 0.2;  // 20% discount
                    double finalTotal = total - discount;
                    writer.WriteLine($"20% Discount Applied: ${discount:F2}");
                    writer.WriteLine($"Final Total: ${finalTotal:F2}");
                }
                else
                {
                    writer.WriteLine("No discount applied.");
                }
            }

            MessageBox.Show(@"Checkout data has been saved. Go to C:\\Users\\Hanie\\Downloads and look for ClothingShopCheckout.txt ");
        }
        }
    }

