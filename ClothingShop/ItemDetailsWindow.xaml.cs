using System;
using System.Windows;

namespace ClothingShop
{
    public partial class ItemDetailsWindow : Window
    {
        private Item item;
        private double budget;
        private Action<Item> onPurchase;

        public ItemDetailsWindow(Item item, double budget, Action<Item> onPurchase)
        {
            InitializeComponent();
            this.item = item;
            this.budget = budget;
            this.onPurchase = onPurchase;

            // Set the item details
            ItemImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(item.ImageUrl));
            ItemNameText.Text = item.Name;
            ItemPriceText.Text = $"Price: ${item.Price:F2}";
            ItemDescriptionText.Text = item.Description ?? "No description available.";
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (budget >= item.Price)
            {
                // Execute the purchase and pass the item to the calling window
                onPurchase(item);
                this.Close();
            }
            else
            {
                MessageBox.Show("You don't have enough budget to purchase this item!");
            }
        }
    }
}
