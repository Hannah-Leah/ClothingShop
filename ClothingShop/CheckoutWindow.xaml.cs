using System.Collections.Generic;
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
    }
}
