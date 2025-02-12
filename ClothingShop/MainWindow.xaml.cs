using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClothingShop
{
    public partial class MainWindow : Window
    {
        private double budget = 400;
        private List<Item> items = new List<Item>();
        private List<Item> purchasedItems = new List<Item>(); // Store purchased items
        private Item selectedItem;

        public MainWindow()
        {
            InitializeComponent();
            UpdateBudgetDisplay();

            // Initial dummy items
            items.Add(new Item { Name = "Check Skirt", Price = 30, ImageUrl = "https://www.pngarts.com/files/5/Plaid-Skirt-Free-PNG-Image.png", Description = "A cute skirt ૮ ˶ᵔ ᵕ ᵔ˶ ა \n ✿Length: 40cm \n ✿Size: S", Category = "Skirts" });
            items.Add(new Item { Name = "Trench Coat", Price = 105, ImageUrl = "https://pngimg.com/d/coat_PNG3.png", Description = "An Elegant coat for everyone ⸜(｡˃ ᵕ ˂ )⸝♡ \n ✿Length: 80cm \n ✿Size: M", Category = "Outerwears" });
            items.Add(new Item { Name = "Ribbon Blouse", Price = 60, ImageUrl = "https://static.vecteezy.com/system/resources/previews/035/939/486/non_2x/ai-generated-blouse-isolated-on-transparent-background-free-png.png", Description = "A beautiful french style blouse! ˗ˏˋ✩ˎˊ˗ \n ✿Length: 50cm \n ✿Size: M", Category = "Tops" });
            items.Add(new Item { Name = "Pastel Hoodie", Price = 81, ImageUrl = "https://static.vecteezy.com/system/resources/previews/035/438/722/non_2x/ai-generated-purple-hoodie-isolated-on-transparent-background-free-png.png", Description = "A Pastel purple hoodie that makes you warm and comfy.  ૮₍˶ •. • ⑅₎ა ♡ \n ✿Length: 75cm \n ✿Size: S", Category = "Outerwears" });
            items.Add(new Item { Name = "Sparkle Dress", Price = 258, ImageUrl = "https://static.vecteezy.com/system/resources/previews/024/701/132/non_2x/dress-cute-and-beautiful-transparent-background-ai-generated-digital-illustration-png.png", Description = "Blue dress, it sparkls! Perfect for date nights. ˊᗜˋ \n ✿Length: 98cm \n ✿Size: M", Category = "Dresses" });
            items.Add(new Item { Name = "Cool Sneakers", Price = 159, ImageUrl = "https://pics.clipartpng.com/Grey_High_Sneakers_PNG_Clipart-489.png", Description = "Extra cool sneakers! `⎚⩊⎚´ -✧ \n ✿Shoe Size: 36EU", Category = "Shoes" });
            items.Add(new Item { Name = "Pleated Skirt", Price = 42, ImageUrl = "https://www.pngall.com/wp-content/uploads/4/Skirt-PNG-Image.png", Description = "Super Duper cool skirt!! ૮ ˙Ⱉ˙ ა \n ✿Size: L \n ✿Length: 43cm", Category = "Skirts" });


            // Set the initial category filter to "All Categories"
            CategoryComboBox.SelectedIndex = 0;

            // Set the initial item list
            FilterItems();

            ItemList.ItemsSource = items;
            ItemList.SelectionChanged += ItemList_SelectionChanged;
        }

        private void UpdateBudgetDisplay()
        {
            BudgetText.Text = $"Budget: ${budget:F2}";
        }

        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = (Item)ItemList.SelectedItem;
        }

        // Handle the purchase button click
        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (Item)((Button)sender).DataContext; // Get the item associated with the button
            if (budget >= item.Price)
            {
                budget -= item.Price;
                purchasedItems.Add(item);
                UpdateBudgetDisplay();
                MessageBox.Show($"You purchased {item.Name} for ${item.Price:F2}. Remaining budget: ${budget:F2}");
            }
            else
            {
                MessageBox.Show("You don't have enough budget to make this purchase.");
            }
        }

        // Handle double-click on item to open item details
        private void ItemList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (selectedItem != null)
            {
                // Open the ItemDetailsWindow with a callback to handle purchase
                ItemDetailsWindow detailsWindow = new ItemDetailsWindow(selectedItem, budget, OnItemPurchased);
                detailsWindow.Show();
            }
        }

        // This will be called when an item is purchased from the ItemDetailsWindow
        private void OnItemPurchased(Item item)
        {
            if (budget >= item.Price)
            {
                budget -= item.Price;
                purchasedItems.Add(item);
                UpdateBudgetDisplay();
                MessageBox.Show($"You purchased {item.Name} for ${item.Price:F2}. Remaining budget: ${budget:F2}");
            }
        }

        // Create a new item
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Item newItem = new Item();
            CreateEditItemWindow createWindow = new CreateEditItemWindow(newItem, true);
            createWindow.ItemCreated += (s, item) =>
            {
                items.Add(item);
                ItemList.ItemsSource = null;
                ItemList.ItemsSource = items; // Refresh the list
                FilterItems(); // Refresh the filtered list
            };
            createWindow.Show();
        }

        // Edit the selected item
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to edit.");
                return;
            }

            CreateEditItemWindow editWindow = new CreateEditItemWindow(selectedItem, false);
            editWindow.ItemCreated += (s, item) =>
            {
                ItemList.ItemsSource = null;
                ItemList.ItemsSource = items; // Refresh the list
                FilterItems(); // Refresh the filtered list
            };
            editWindow.Show();
        }

        // Delete the selected item
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }

            items.Remove(selectedItem);
            ItemList.ItemsSource = null;
            ItemList.ItemsSource = items; // Refresh the list
            FilterItems(); // Refresh the filtered list
        }

        // Filter items based on selected category
        private void FilterItems()
        {
            string selectedCategory = ((ComboBoxItem)CategoryComboBox.SelectedItem).Content.ToString();

            if (selectedCategory == "All Categories")
            {
                ItemList.ItemsSource = items; // Show all items
            }
            else
            {
                ItemList.ItemsSource = items.Where(i => i.Category == selectedCategory).ToList(); // Show filtered items
            }
        }

        // Handle category selection change
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterItems(); // Refresh the list based on the selected category
        }

        // Navigate to the Checkout page
        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            CheckoutWindow checkoutWindow = new CheckoutWindow(purchasedItems, budget, OnBudgetUpdated);
            checkoutWindow.Show();
        }

        // This function will be called to update the budget in MainWindow
        private void OnBudgetUpdated(double updatedBudget)
        {
            budget = updatedBudget;
            UpdateBudgetDisplay(); // Update the budget display after a change
        }

        // This function saves the list of items to a CSV file
        private void SaveItemsToCsv(string filePath)
        {
            // Create a list to store the CSV lines, starting with the header line
            var csvLines = new List<string>
    {
        "Name;Price;ImageUrl;Description;Category" // CSV header line with semicolons
    };

            // Loop through each item in the items list
            foreach (var item in items)
            {
                // Replace newlines in the description with spaces (simplify the description)
                string description = item.Description.Replace("\n", " ").Replace("\r", " "); // Convert multiline to single line

                // Convert each item's properties to a CSV line, separating them by semicolons
                csvLines.Add($"{item.Name};{item.Price};{item.ImageUrl};{description};{item.Category}");
            }

            // Write all the lines to the file at the specified filePath
            System.IO.File.WriteAllLines(filePath, csvLines);

            
            MessageBox.Show("Items saved to CSV successfully.");
        }





        // This function loads items from a CSV file and populates the items list
        private void LoadItemsFromCsv(string filePath)
        {
            try
            {
                // Read all lines from the CSV file, skipping the header line (we use Skip(1) to ignore the first line)
                var csvLines = System.IO.File.ReadAllLines(filePath).Skip(1);

                // Clear any existing items in the list before loading new ones
                items.Clear();

                // Loop through each line in the CSV file
                foreach (var line in csvLines)
                {
                    // Split the line by semicolons 
                    var columns = line.Split(';');

                    // Check if the line has exactly 5 columns (we expect Name, Price, ImageUrl, Description, and Category)
                    if (columns.Length == 5)
                    {
                        // Create a new Item object using the data from the CSV columns
                        var item = new Item
                        {
                            Name = columns[0],
                            Price = double.Parse(columns[1]),  // Parse the Price from string to double
                            ImageUrl = columns[2],
                            Description = columns[3], // Description is now a single line
                            Category = columns[4]
                        };

                        // Add the new item to the items list
                        items.Add(item);
                    }
                }

                // Refresh the ListView to display the loaded items
                ItemList.ItemsSource = null;
                ItemList.ItemsSource = items;

                // Apply any active category filter (if needed)
                FilterItems();

             
                MessageBox.Show("Items loaded from CSV successfully.");
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"An error occurred while loading items: {ex.Message}");
            }
        }





        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = "csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveItemsToCsv(saveFileDialog.FileName);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = "csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadItemsFromCsv(openFileDialog.FileName);
            }
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; } 
        public string Category { get; set; }
    }
}
