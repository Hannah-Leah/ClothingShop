using System;
using System.Windows;
using System.Windows.Controls;

namespace ClothingShop
{
    public partial class CreateEditItemWindow : Window
    {
        public Item Item { get; set; }
        public bool IsNewItem { get; set; }

        // Event to notify the parent window that the item has been created/edited
        public event EventHandler<Item> ItemCreated;

        public CreateEditItemWindow(Item item, bool isNewItem)
        {
            InitializeComponent();

            Item = item;
            IsNewItem = isNewItem;

            // Set the data context for binding
            DataContext = Item;

            // Pre-fill the fields if editing an existing item
            if (!IsNewItem)
            {
                ItemNameTextBox.Text = Item.Name;
                ItemPriceTextBox.Text = Item.Price.ToString();
                ImageUrlTextBox.Text = item.ImageUrl;
                ItemDescriptionTextBox.Text = Item.Description;

                // Set the category based on the existing item
                var categoryComboBoxItem = CategoryComboBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == Item.Category);
                CategoryComboBox.SelectedItem = categoryComboBoxItem;
            }
        }

        // Handle the Save button click
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure the fields are filled out
            if (string.IsNullOrEmpty(ItemNameTextBox.Text) || string.IsNullOrEmpty(ItemPriceTextBox.Text) || string.IsNullOrEmpty(ItemDescriptionTextBox.Text) || string.IsNullOrEmpty(ImageUrlTextBox.Text) || CategoryComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("All fields must be filled out.");
                return;
            }

            // Set the item's properties from the UI fields
            Item.Name = ItemNameTextBox.Text;
            Item.Price = double.Parse(ItemPriceTextBox.Text);
            Item.ImageUrl = ImageUrlTextBox.Text;
            Item.Description = ItemDescriptionTextBox.Text;

            // Get the selected category from the ComboBox
            var selectedCategoryItem = (ComboBoxItem)CategoryComboBox.SelectedItem;
            Item.Category = selectedCategoryItem.Content.ToString();

            // Trigger the ItemCreated event and pass the updated item back to the main window
            ItemCreated?.Invoke(this, Item);

            // Close the window
            this.Close();
        }

        // Handle the Cancel button click
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
