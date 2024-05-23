using JProject.Migrations;
using JProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace JProject
{
    public partial class MainWindow : Window
    {
        private readonly JprojectDbContext _dbContext;

        private List<string> quarters;
        public MainWindow()
        {
            quarters = [(DateOnly.Parse("31/03/" + DateTime.Now.Year.ToString()).ToString()),
                        (DateOnly.Parse("30/06/" + DateTime.Now.Year.ToString()).ToString()),
                        (DateOnly.Parse("30/09/" + DateTime.Now.Year.ToString()).ToString()),
                        (DateOnly.Parse("31/12/" + DateTime.Now.Year.ToString()).ToString())];

            InitializeComponent();
            
            _dbContext = new JprojectDbContext();
            updateLists();

        }


        private void AddCategory(object sender, RoutedEventArgs e)
        {
            if (categoryName.Text.Length > 0)
            {
                Category category = new Category();
                category.CategoryName = categoryName.Text;
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                clearAll();
                MessageBox.Show("Successfully added!");
                updateLists();
            } else
            {
                MessageBox.Show("Category name field cannot be empty!");
            }
        }

        private void clearAll()
        {
            CategoryList.Items.Clear();
            RecipientsList.Items.Clear();
            RecipientDataGrid.Items.Clear();
            SelectedRecipient.Items.Clear();
            OrderDataGrid.Items.Clear();
            RecipientTax.Items.Clear();
            Quarter.Items.Clear();
            CategoryDataGrid.Items.Clear();
        }

        
        private void updateLists()
        {
            using (var _dbContext = new JprojectDbContext())
            {
                clearAll();

                TaxLimitLabel.Content += "" + _dbContext.Settings.ToList()[0].LimitNumber;
                TaxRateLabel.Content += "" + _dbContext.Settings.ToList()[0].TaxRate;
                TotalLimit.Text = "" + _dbContext.Settings.ToList()[0].LimitNumber;
                TaxRate.Text = "" + _dbContext.Settings.ToList()[0].TaxRate;

                foreach (var q in quarters)
                    Quarter.Items.Add(q);

                var categories = _dbContext.Categories.ToList();
                foreach (var c in categories)
                    CategoryDataGrid.Items.Add(c);

                var categoryNames = _dbContext.Categories.Select(c => c.CategoryName).ToList();
                for(int i = 0; i < _dbContext.Categories.ToList().Count; i++)
                    CategoryList.Items.Add(categoryNames[i]);

                var recipientsNames = _dbContext.Recipients.Select(c => c.Name).ToList();
                for (int i = 0; i < _dbContext.Recipients.ToList().Count; i++)
                {
                    RecipientsList.Items.Add(recipientsNames[i]);
                    SelectedRecipient.Items.Add(recipientsNames[i]);
                    RecipientTax.Items.Add(recipientsNames[i]);
                }

                var recipients = _dbContext.Recipients.ToList();
                for (int i = 0; i < recipients.Count; i++)
                    RecipientDataGrid.Items.Add(recipients[i]);


                var orders = _dbContext.Orders.ToList();
                for (int i = 0; i < orders.Count; i++)
                {
                    Order order = orders[i];
                    OrderDataGridModel orderDataGridModel = new OrderDataGridModel()
                    {
                        Id = order.id,
                        CategoryName = _dbContext.Categories.Find(order.CategoryId).CategoryName,
                        RecipientName = _dbContext.Recipients.Find(order.RecipientId).Name,
                        Price = order.Price,
                        ShippingPrice = order.ShippingPrice,
                        DeliveryDate = order.DeliveryDate,
                        Comment = order.Comment
                    };
                    OrderDataGrid.Items.Add(orderDataGridModel);
                }
            }
        }

        private void saveRecipient(object sender, RoutedEventArgs e)
        { 
            if (Name.Text.Length > 0 && LastName.Text.Length > 0)
            {
                Recipient recipient = new Recipient();
                recipient.Name = Name.Text;
                recipient.LastName = LastName.Text;
                recipient.PassportNumber = PassportNumber.Text;
                recipient.PersonalNumber = PersonalNumber.Text;
                recipient.PhoneNumber = Phone.Text;
                _dbContext.Recipients.Add(recipient);
                _dbContext.SaveChanges();
                clearAll();
                MessageBox.Show("Successfully added!");
                updateLists();
            } else
            {
                MessageBox.Show("Name and Last Name fields cannot be empty!");
            }
        }



        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^\d]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^\d]([\.][\d]{1,2})?$");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void UpdateLimit(object sender, RoutedEventArgs e)
        {
            if (TotalLimit.Text.Length > 0 && TaxRate.Text.Length > 0)
            {
                Settings settings = _dbContext.Settings.ToList()[0];
                settings.TaxRate = Decimal.Parse(TaxRate.Text);
                settings.LimitNumber = Decimal.Parse(TotalLimit.Text);
                _dbContext.Settings.Update(settings);
                _dbContext.SaveChanges();
                MessageBox.Show("Successfully Saved!");
            } else
            {
                MessageBox.Show("Successfully Saved!");
            }
        }

        private void AddOrder(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            if (CategoryList.SelectedItem != null && RecipientsList.SelectedItem != null 
                && Price.Text.Length > 0 && ShippingPrice.Text.Length > 0)
            {
                Category category = new Category();
                Recipient recipient = new Recipient();
                category = _dbContext.Categories.ToList().ElementAtOrDefault(CategoryList.SelectedIndex);
                recipient = _dbContext.Recipients.ToList().ElementAtOrDefault(RecipientsList.SelectedIndex);
                order.Comment = Comment.Text;
                order.CategoryId = category.Id;
                order.RecipientId = recipient.Id;
                order.Price = Decimal.Parse(Price.Text);
                order.ShippingPrice = Decimal.Parse(ShippingPrice.Text);
                order.DeliveryDate = DateOnly.Parse(DeliveryDate.Text); 
                _dbContext.Orders.Add(order);
                clearAll();
                _dbContext.SaveChanges();
                MessageBox.Show("Successfully Added!");
                updateLists();
            }
            else
            {
                MessageBox.Show("Incorrect Data was provided!");
            }
        }


        private void ChangeRecipient(object sender, RoutedEventArgs e)
        {
            Recipient recipient = (Recipient)RecipientDataGrid.SelectedItem;
            Name.Text = recipient.Name;
            LastName.Text = recipient.LastName;
            PassportNumber.Text = recipient.PassportNumber;
            PersonalNumber.Text = recipient.PersonalNumber;
            Phone.Text = recipient.PhoneNumber;
        }

        private void DeleteRecipient(object sender, RoutedEventArgs e)
        {
            using (var _dbContext = new JprojectDbContext())
            {
                if (RecipientDataGrid.SelectedItem != null)
                {

                    Recipient recipient = (Recipient)RecipientDataGrid.SelectedItem;
                    _dbContext.Recipients.Remove(recipient);
                    foreach (var order in _dbContext.Orders)
                        if (order.RecipientId == recipient.Id) 
                            _dbContext.Orders.Remove(order);
                    _dbContext.SaveChanges();
                    updateLists();
                }
            }
            
        }

        private void updateRecipient(object sender, RoutedEventArgs e)
        {
            if (SelectedRecipient.SelectedItem != null && Name.Text.Length > 0 
                && LastName.Text.Length > 0)
            {
                Recipient recipient = new Recipient();
                recipient = _dbContext.Recipients.ToList().ElementAtOrDefault(SelectedRecipient.SelectedIndex);
                recipient.Name = Name.Text;
                recipient.LastName = LastName.Text;
                recipient.PassportNumber = PassportNumber.Text;
                recipient.PersonalNumber = PersonalNumber.Text;
                recipient.PhoneNumber = Phone.Text;
                _dbContext.Recipients.Update(recipient);
                _dbContext.SaveChanges();
                clearAll();
                MessageBox.Show("Changed!");
                updateLists();
            }
            else
            {
                MessageBox.Show("Incorrect Data!");
            }
        }

        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            OrderDataGridModel order = (OrderDataGridModel)OrderDataGrid.SelectedItem;
            _dbContext.Orders.Remove(_dbContext.Orders.Find(order.Id));
            _dbContext.SaveChanges();
            updateLists();
        }


        private void TaxRecipientsChanged(object sender, SelectionChangedEventArgs e)
        {
            updateTaxGrid();
        }

        private void TaxDateChanged(object sender, SelectionChangedEventArgs e)
        {
            updateTaxGrid();
        }

        private void updateTaxGrid()
        {
            CategoryMessage.Content = "";
            WarningMessage.Content = "The limit was not exceeded";
            WarningMessage.Foreground = new SolidColorBrush(Colors.Black);
            CategoryMessage.Foreground = new SolidColorBrush(Colors.Black);
            OrderTaxGrid.Items.Clear();

            if (RecipientTax.SelectedItem != null && Quarter.SelectedItem != null)
            {
                int selectedQuarterIndex = Quarter.SelectedIndex;
                decimal total = 0;
                decimal limit = _dbContext.Settings.First().LimitNumber;
                List<string> boughtCategories = new List<string>();
                var recipient = _dbContext.Recipients.ElementAtOrDefault(RecipientTax.SelectedIndex);
                var orders = _dbContext.Orders.ToList();
                foreach (var order in orders)
                {
                    if (selectedQuarterIndex == 0
                        ? DateOnly.Parse("01/01/" + DateTime.Now.Year) <= order.DeliveryDate 
                        && order.DeliveryDate <= DateOnly.Parse(quarters[selectedQuarterIndex])
                        : DateOnly.Parse(quarters[selectedQuarterIndex - 1]) <= order.DeliveryDate 
                        && order.DeliveryDate <= DateOnly.Parse(quarters[selectedQuarterIndex]))
                    {
                        var categoryName = _dbContext.Categories.Find(order.CategoryId)?.CategoryName;
                        var recipientName = _dbContext.Recipients.Find(order.RecipientId)?.Name;
                        if (!string.IsNullOrEmpty(categoryName))
                        {
                            var orderData = new OrderDataGridModel
                            {
                                Id = order.id,
                                CategoryName = categoryName,
                                RecipientName = recipientName,
                                Price = order.Price,
                                ShippingPrice = order.ShippingPrice,
                                DeliveryDate = order.DeliveryDate,
                                Comment = order.Comment
                            };
                            boughtCategories.Add(categoryName);
                            total += order.Price + order.ShippingPrice;
                            limit -= order.Price + order.ShippingPrice;
                            OrderTaxGrid.Items.Add(orderData);
                        }
                    }
                }
                if (limit < 0)
                {
                    WarningMessage.Content = "You have exceeded the limit by " + Math.Abs(limit) 
                        + ". You will have to pay " + Math.Abs(limit) / 100 * _dbContext.Settings.First().TaxRate;
                    WarningMessage.Foreground = new SolidColorBrush(Colors.Red);
                }
                if (boughtCategories.Count != boughtCategories.Distinct().Count())
                {
                    CategoryMessage.Content = "You have more than one item of the same category!";
                    CategoryMessage.Foreground = new SolidColorBrush(Colors.Red);
                }
                TotalMessage.Content = "Total is " + total;
            }
        }

        private void DeleteCategory(object sender, EventArgs e)
        {
            using (var _dbContext = new JprojectDbContext())
            {
                Category category = (Category)CategoryDataGrid.SelectedItem;
                _dbContext.Categories.Remove(category);
                foreach (var order in _dbContext.Orders)
                    if (order.CategoryId == category.Id)
                        _dbContext.Orders.Remove(order);
                _dbContext.SaveChanges();
                updateLists();
            }
                
        }


    }
}