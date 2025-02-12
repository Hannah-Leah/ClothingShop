using System.IO;
using System.Text;
using System.Windows;

namespace ClothingShop
{
    public partial class CheckoutWindow : Window
    {
        private List<Item> purchasedItems;
        private double remainingBudget;
        private Action<double> updateBudgetCallback; // Callback to update the budget

        // Pass the callback from MainWindow to update the budget
        public CheckoutWindow(List<Item> items, double budget, Action<double> updateBudget)
        {
            InitializeComponent();
            purchasedItems = items;
            remainingBudget = budget;
            updateBudgetCallback = updateBudget; // Save the callback

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

        // Delete the selected purchased item and update budget in MainWindow
        private void DeletePurchasedItem_Click(object sender, RoutedEventArgs e)
        {
            if (PurchasedItemsList.SelectedItem == null)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }

            // Remove the selected item from the purchasedItems list
            var selectedItem = (Item)PurchasedItemsList.SelectedItem;
            purchasedItems.Remove(selectedItem);

            // Update the remaining budget
            remainingBudget += selectedItem.Price; // Add the price of the removed item back to the budget
            updateBudgetCallback(remainingBudget); // Call the callback to update the MainWindow's budget

            // Refresh the ListBox to reflect the changes
            PurchasedItemsList.ItemsSource = null;
            PurchasedItemsList.ItemsSource = purchasedItems;

            // Optionally, update the total cost after deletion
            double total = 0;
            foreach (var item in purchasedItems)
            {
                total += item.Price;
            }
            TotalText.Text = $"Total: ${total:F2}";
        }
    }
}
