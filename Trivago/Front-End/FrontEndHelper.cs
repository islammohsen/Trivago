﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Trivago.Models;

namespace Trivago.Front_End
{
    static class FrontEndHelper
    {
        private static Trivago.MainWindow mainWindow;
        private static Trivago.Admin_window adminWindow;

        public static void SetMainWindow(MainWindow window)
        {
            mainWindow = window;
        }

        public static Trivago.MainWindow GetMainWindow()
        {
            return (Trivago.MainWindow)App.Current.MainWindow;
        }

        public static void SetAdminWindow(Admin_window window)
        {
            adminWindow = window;
        }

        public static Admin_window GetAdminWindow()
        {
            return adminWindow;
        }
        
        public static Button CreateButton(double width, double height, string content)
        {
            Button obj = new Button
            {
                Width = width,
                Height = height,
                Content = content,
                Background = new SolidColorBrush(Color.FromRgb(0, 127, 175)),
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                FontSize = 20,
                Cursor = Cursors.Hand
            };
            obj.MouseEnter += button_MouseEnter;
            obj.MouseLeave += button_MouseLeave;
            return obj;
        }

        private static void button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Background = new SolidColorBrush(Color.FromRgb(0, 127, 175));
        }

        private static void button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Background = new SolidColorBrush(Color.FromRgb(0, 95, 127));
        }
        
        public static void CreateReservePopupWindow(RoomsListShowCanvas roomListShowCanvas, int index)
        {
            DataModels database = DataModels.GetInstance();

            Window popup = new Window
            {
                Width = 350,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Confirm Booking"
            };

            Grid popupGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255))
            };
            popup.Content = popupGrid;

            StackPanel reservePopupStackPanel = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Margin = new Thickness(10, 10, 10, 10)
            };
            popupGrid.Children.Add(reservePopupStackPanel);

            Grid numberofGuestsGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };
            reservePopupStackPanel.Children.Add(numberofGuestsGrid);

            Label numberofGuestsLabel = new Label
            {
                Content = "Number of Guests : ",
                FontSize = 22
            };
            Grid.SetColumn(numberofGuestsLabel, 0);
            numberofGuestsGrid.Children.Add(numberofGuestsLabel);

            TextBox numberofGuestTextBox = new TextBox
            {
                Width = 50,
                FontSize = 22
            };
            Grid.SetColumn(numberofGuestTextBox, 1);
            numberofGuestsGrid.Children.Add(numberofGuestTextBox);

            Label mealLabel = new Label
            {
                Content = "Meal Plan : " + roomListShowCanvas.GetSelectedMealPlan(index).name,
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            reservePopupStackPanel.Children.Add(mealLabel);

            Label websiteLabel = new Label
            {
                Content = "Website : " + roomListShowCanvas.GetSelectedWebsite(index).name,
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            reservePopupStackPanel.Children.Add(websiteLabel);

            Label priceLabel = new Label
            {
                Content = "Price : ",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            reservePopupStackPanel.Children.Add(priceLabel);

            Calendar datePicker = new Calendar
            {
                SelectionMode = CalendarSelectionMode.SingleRange,
                Margin = new Thickness(0, 10, 0, 0)
            };
            datePicker.DisplayDateStart = DateTime.Today;
            List<Booking> bookings = database.GetRoomBookings(roomListShowCanvas.GetSelectedRoom(index));
            foreach (Booking booking in bookings)
            {
                datePicker.BlackoutDates.Add(new CalendarDateRange(booking.startDate, booking.endDate));
            }
            reservePopupStackPanel.Children.Add(datePicker);

            Button confirmButton = FrontEndHelper.CreateButton(80, 40, "Confirm");
            confirmButton.Margin = new Thickness(0, 10, 0, 0);
            reservePopupStackPanel.Children.Add(confirmButton);

            popup.Owner = GetMainWindow();
            popup.ShowDialog();
        }

        public static void CreateAddWebsitePopupWindow()
        {
            
            Window popup = new Window
            {
                Width = 330,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Add Website"
            };

            StackPanel stackPanel = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Margin = new Thickness(10, popup.Height/4, 10, 10)
            };

            Grid popupGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width =GridLength.Auto},
                        new ColumnDefinition { Width =GridLength.Auto}
                    },
                RowDefinitions =
                {
                    new RowDefinition{ Height = GridLength.Auto},
                    new RowDefinition{ Height = GridLength.Auto}
                }
            };
            popup.Content = stackPanel;
            stackPanel.Children.Add(popupGrid);

            //
            Label websiteNameLabel = new Label
            {
                Content = "Website Link : ",
                FontSize = 17
            };
            Grid.SetColumn(websiteNameLabel, 0);
            Grid.SetRow(websiteNameLabel, 0);
            popupGrid.Children.Add(websiteNameLabel);
            
            //
            Label websiteRatingLabel = new Label
            {
                Content = "Website Rating : ",
                FontSize = 17
            };
            Grid.SetColumn(websiteRatingLabel, 0);
            Grid.SetRow(websiteRatingLabel, 1);
            popupGrid.Children.Add(websiteRatingLabel);
            
            //
            TextBox websiteNameTextBox = new TextBox
            {
                Width = 150,
                Height = 20,
                FontSize = 15
            };
            Grid.SetColumn(websiteNameTextBox, 1);
            Grid.SetRow(websiteNameTextBox, 0);
            popupGrid.Children.Add(websiteNameTextBox);

            //
            TextBox websiteRatingTextBox = new TextBox
            {
                Width = 150,
                Height = 20,
                FontSize = 15
            };
            Grid.SetColumn(websiteRatingTextBox, 1);
            Grid.SetRow(websiteRatingTextBox, 1);
            popupGrid.Children.Add(websiteRatingTextBox);

            
            Button addWebsiteConfirmButton = FrontEndHelper.CreateButton(80, 40, "Add");
            addWebsiteConfirmButton.Margin = new Thickness(0, 10, 0, 0);

            List<TextBox> data = new List<TextBox>();
            data.Add(websiteNameTextBox);
            data.Add(websiteRatingTextBox);

            addWebsiteConfirmButton.Tag = data;
            addWebsiteConfirmButton.Click += AddWebsiteConfirmButton_Click;
            stackPanel.Children.Add(addWebsiteConfirmButton);
                        

            popup.Owner = GetAdminWindow();
            popup.ShowDialog();
        }
        
        public static void CreateUpdateWebsitePopupWindow(Website website)
        {

            Window popup = new Window
            {
                Width = 330,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Update Website"
            };

            StackPanel stackPanel = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Margin = new Thickness(10, popup.Height / 4, 10, 10)
            };

            Grid popupGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width =GridLength.Auto},
                        new ColumnDefinition { Width =GridLength.Auto}
                    },
                RowDefinitions =
                {
                    new RowDefinition{ Height = GridLength.Auto},
                    new RowDefinition{ Height = GridLength.Auto}
                }
            };
            popup.Content = stackPanel;
            stackPanel.Children.Add(popupGrid);

            //
            Label websiteNameLabel = new Label
            {
                Content = "Website Link : ",
                FontSize = 17
            };
            Grid.SetColumn(websiteNameLabel, 0);
            Grid.SetRow(websiteNameLabel, 0);
            popupGrid.Children.Add(websiteNameLabel);

            //
            Label websiteNameLabel2 = new Label
            {
                Content = website.name,
                FontSize = 15
            };
            Grid.SetColumn(websiteNameLabel2, 1);
            Grid.SetRow(websiteNameLabel2, 0);
            popupGrid.Children.Add(websiteNameLabel2);

            //
            Label websiteRatingLabel = new Label
            {
                Content = "Website Rating : ",
                FontSize = 17
            };
            Grid.SetColumn(websiteRatingLabel, 0);
            Grid.SetRow(websiteRatingLabel, 1);
            popupGrid.Children.Add(websiteRatingLabel);

            //
            TextBox websiteRatingTextBox = new TextBox
            {
                Width = 150,
                Height = 20,
                FontSize = 15,
                Text = website.rating.ToString()
            };
            Grid.SetColumn(websiteRatingTextBox, 1);
            Grid.SetRow(websiteRatingTextBox, 1);
            popupGrid.Children.Add(websiteRatingTextBox);


            Button updateWebsiteConfirmButton = FrontEndHelper.CreateButton(80, 40, "Update");
            updateWebsiteConfirmButton.Margin = new Thickness(0, 10, 0, 0);

            List<Object> data = new List<Object>();
            data.Add(website);
            data.Add(websiteRatingTextBox);
            updateWebsiteConfirmButton.Tag = data;

            updateWebsiteConfirmButton.Click += UpdateWebsiteConfirmButton_Click;
            stackPanel.Children.Add(updateWebsiteConfirmButton);


            popup.Owner = GetAdminWindow();
            popup.ShowDialog();
        }
        
        public static void CreateUpdateOfferPopupWindow(Offer offer)
        {
            Window popup = new Window
            {
                Width = 330,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Update Website"
            };

            StackPanel stackPanel = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Margin = new Thickness(10, popup.Height / 4, 10, 10)
            };

            Grid popupGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width =GridLength.Auto},
                        new ColumnDefinition { Width =GridLength.Auto}                    },
                RowDefinitions =
                {
                    new RowDefinition{ Height = GridLength.Auto},
                    new RowDefinition{ Height = GridLength.Auto},
                    new RowDefinition{ Height = GridLength.Auto},
                    new RowDefinition{ Height = GridLength.Auto}
                }
            };
            popup.Content = stackPanel;
            stackPanel.Children.Add(popupGrid);

            //
            Label websiteLabel = new Label
            {
                Content = "website: ",
                FontSize = 17
            };
            Grid.SetColumn(websiteLabel, 0);
            Grid.SetRow(websiteLabel, 0);
            popupGrid.Children.Add(websiteLabel);

            //
            Label websiteNameLabel2 = new Label
            {
                Content = offer.website.name,
                FontSize = 15
            };
            Grid.SetColumn(websiteNameLabel2, 1);
            Grid.SetRow(websiteNameLabel2, 0);
            popupGrid.Children.Add(websiteNameLabel2);

            //
            Label hotelLabel = new Label
            {
                Content = "Hotel: ",
                FontSize = 17
            };
            Grid.SetColumn(hotelLabel, 0);
            Grid.SetRow(hotelLabel, 1);
            popupGrid.Children.Add(hotelLabel);

            //
            Label hotelLabel2 = new Label
            {
                Content = offer.room.hotel.name,
                FontSize = 17
            };
            Grid.SetColumn(hotelLabel2, 1);
            Grid.SetRow(hotelLabel2, 1);
            popupGrid.Children.Add(hotelLabel2);

            //
            Label roomNumberLabel = new Label
            {
                Content = "Room Number: ",
                FontSize = 17
            };
            Grid.SetColumn(roomNumberLabel, 0);
            Grid.SetRow(roomNumberLabel, 2);
            popupGrid.Children.Add(roomNumberLabel);
            
            //
            Label roomNumberLabel2 = new Label
            {
                Content = offer.room.number.ToString(),
                FontSize = 17
            };
            Grid.SetColumn(roomNumberLabel2, 1);
            Grid.SetRow(roomNumberLabel2, 2);
            popupGrid.Children.Add(roomNumberLabel2);
            
            //
            Label priceLabel = new Label
            {
                Content = "Price ",
                FontSize = 17
            };
            Grid.SetColumn(priceLabel, 0);
            Grid.SetRow(priceLabel, 3);
            popupGrid.Children.Add(priceLabel);

            //
            TextBox priceTextBox = new TextBox
            {
                Width = 150,
                Height = 20,
                FontSize = 15,
                Text = offer.price.ToString()
            };
            Grid.SetColumn(priceTextBox, 1);
            Grid.SetRow(priceTextBox, 3);
            popupGrid.Children.Add(priceTextBox);


            Button updateOfferConfirmButton = FrontEndHelper.CreateButton(80, 40, "Update");
            updateOfferConfirmButton.Margin = new Thickness(0, 10, 0, 0);

            List<Object> data = new List<Object>();
            data.Add(offer);
            data.Add(priceTextBox);
            updateOfferConfirmButton.Tag = data;

            updateOfferConfirmButton.Click += UpdateOfferConfirmButton_Click; ;
            stackPanel.Children.Add(updateOfferConfirmButton);

            popup.Owner = GetAdminWindow();
            popup.ShowDialog();
        }

        private static void UpdateOfferConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DataModels database = DataModels.GetInstance();
            Button button = (Button)sender;
            List<object> data = (List<Object>)button.Tag;
            Offer offer = (Offer)data[0];
            TextBox priceBox = (TextBox)data[1];

            int price = int.Parse(priceBox.Text.ToString());


            bool done = database.UpdateRoomPrice(offer.room, offer.website, price);

            if (done)
                MessageBox.Show("Offer Updated successfully !");
            else
                MessageBox.Show("Couldn't update offer");
                
        }

        public static void createAddOfferPopupWindow()
        {
            DataModels database = DataModels.GetInstance();
            Window popup = new Window
            {
                Width = 330,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Add Offer"
            };

            StackPanel stackPanel = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Margin = new Thickness(10, popup.Height / 4, 10, 10)
            };

            Grid popupGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width =GridLength.Auto},
                        new ColumnDefinition { Width =GridLength.Auto}
                    },
                RowDefinitions =
                {
                    new RowDefinition{ Height = GridLength.Auto}, // Website combo
                    new RowDefinition{ Height = GridLength.Auto}, // Hotel Combo
                    new RowDefinition{ Height = GridLength.Auto}, // Room # Combo
                    new RowDefinition{ Height = GridLength.Auto} // Price TextBox
                }
            };
            popup.Content = stackPanel;
            stackPanel.Children.Add(popupGrid);


            //
            Label websiteLabel = new Label
            {
                Content = "Website: ",
                FontSize = 17
            };
            Grid.SetColumn(websiteLabel, 0);
            Grid.SetRow(websiteLabel, 0);
            popupGrid.Children.Add(websiteLabel);

            //
            Label hotelLicenceLabel = new Label
            {
                Content = "Hotel: ",
                FontSize = 17
            };
            Grid.SetColumn(hotelLicenceLabel, 0);
            Grid.SetRow(hotelLicenceLabel, 1);
            popupGrid.Children.Add(hotelLicenceLabel);

            //
            Label roomNumberLabel = new Label
            {
                Content = "Room number: ",
                FontSize = 17
            };
            Grid.SetColumn(roomNumberLabel, 0);
            Grid.SetRow(roomNumberLabel, 2);
            popupGrid.Children.Add(roomNumberLabel);

            //
            Label offerPriceLabel = new Label
            {
                Content = "Offer Price: ",
                FontSize = 17
            };
            Grid.SetColumn(offerPriceLabel, 0);
            Grid.SetRow(offerPriceLabel, 3);
            popupGrid.Children.Add(offerPriceLabel);


            //
            ComboBox websiteCombo = new ComboBox
            {
                Width = 150,
                Height = 30,
                FontSize = 15
            };
            Grid.SetColumn(websiteCombo, 1);
            Grid.SetRow(websiteCombo, 0);
            websiteCombo.ItemsSource = database.GetAllWebsites();
            websiteCombo.SelectedIndex = 0;
            popupGrid.Children.Add(websiteCombo);

            //
            ComboBox hotelLicenceCombo = new ComboBox
            {
                Width = 150,
                Height = 30,
                FontSize = 15
            };
            Grid.SetColumn(hotelLicenceCombo, 1);
            Grid.SetRow(hotelLicenceCombo, 1);
            hotelLicenceCombo.ItemsSource = database.GetAllHotels();
            hotelLicenceCombo.SelectedIndex = 0;
            hotelLicenceCombo.SelectionChanged += HotelLicenceCombo_SelectionChanged;
            popupGrid.Children.Add(hotelLicenceCombo);

            //
            ComboBox roomNumberCombo = new ComboBox
            {
                Width = 150,
                Height = 30,
                FontSize = 15
            };
            Grid.SetColumn(roomNumberCombo, 1);
            Grid.SetRow(roomNumberCombo, 2);

            roomNumberCombo.ItemsSource = database.GetHotelRooms(((Hotel)hotelLicenceCombo.SelectedItem).licenseNumber);
            roomNumberCombo.SelectedIndex = 0;
            popupGrid.Children.Add(roomNumberCombo);
            hotelLicenceCombo.Tag = roomNumberCombo;

            //
            TextBox offerPriceTextBox = new TextBox
            {
                Width = 150,
                Height = 20,
                FontSize = 15
            };
            Grid.SetColumn(offerPriceTextBox, 1);
            Grid.SetRow(offerPriceTextBox, 3);
            popupGrid.Children.Add(offerPriceTextBox);

            Button addOfferConfirmButton = FrontEndHelper.CreateButton(80, 40, "Add");
            addOfferConfirmButton.Margin = new Thickness(0, 10, 0, 0);

            List<Object> data = new List<Object>();
            data.Add(websiteCombo);
            data.Add(hotelLicenceCombo);
            data.Add(roomNumberCombo);
            data.Add(offerPriceTextBox);

            addOfferConfirmButton.Tag = data;
            addOfferConfirmButton.Click += AddOfferConfirmButton_Click; ;
            stackPanel.Children.Add(addOfferConfirmButton);

            popup.Owner = GetAdminWindow();
            popup.ShowDialog();
        }

        private static void HotelLicenceCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataModels database = DataModels.GetInstance();
            ComboBox hotelLicenceCombo = (ComboBox)sender;
            ComboBox roomNumberCombo =(ComboBox)hotelLicenceCombo.Tag;
            roomNumberCombo.ItemsSource = database.GetHotelRooms(((Hotel)hotelLicenceCombo.SelectedItem).licenseNumber);
            roomNumberCombo.SelectedIndex = 0;
        }

        private static void AddOfferConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DataModels database = DataModels.GetInstance();
            Button button = (Button)sender;
            List<object> data= (List<object>)button.Tag;
            ComboBox websiteCombo = (ComboBox)data[0];
            ComboBox HotelCombo = (ComboBox)data[1];
            ComboBox RoomCombo = (ComboBox)data[2];
            TextBox priceText = (TextBox)data[3];

            Website website = (Website)websiteCombo.SelectedItem;
            Room room = (Room)RoomCombo.SelectedItem;
            int price = int.Parse(priceText.Text);

            bool done = database.AddOffer(new Offer(website, room, price));

            if (done)
                MessageBox.Show("Offer Added successfully !");
            else
                MessageBox.Show("Couldn't add offer");
        }

        private static void UpdateWebsiteConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            List<object> data = (List<object>)button.Tag;
            Website website= (Website)data[0];
            TextBox websiteRatingTextBox =(TextBox)data[1];

            string websiteRating = websiteRatingTextBox.Text;
            if (!BackEndHelper.IsNumber(websiteRating))
            {
                MessageBox.Show("Rating must be a number");
                return;
            }
            int newRating = int.Parse(websiteRating);

            website.rating = newRating;
            DataModels database = DataModels.GetInstance();
            
           
            bool done = database.UpdateWebsite(website);
            if(done)
                MessageBox.Show("Website Updated successfully !");
            else
                MessageBox.Show("Couldn't update website");
           
        }

        private static void AddWebsiteConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            List<TextBox> data = (List<TextBox>)button.Tag;
            string websiteName = data[0].Text;
            string websiteRating = data[1].Text;

            if(!BackEndHelper.IsNumber(websiteRating))
            {
                MessageBox.Show("Rating must be a number");
                return;
            }
            int rating = int.Parse(websiteRating);
            DataModels database = DataModels.GetInstance();
            Website website = new Website(websiteName, rating);
            bool done = database.AddWebsite(website);
            if (done)
                MessageBox.Show("Website Added successfully !");
            else
                MessageBox.Show("Couldn't add website");
        }

        public static void CreateAddRoomPopupWindow()
        {
            double windowWidth = 500;
            double itemsWidth = 150;

            Window popup = new Window
            {
                Width = windowWidth,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Add Room"
            };

            StackPanel addDataStackPanel = new StackPanel
            {
                Width = windowWidth,
                Margin = new Thickness(20, 0.15 * popup.Height, 0, 0)
            };
            popup.Content = addDataStackPanel;

            Grid addDataGrid = new Grid
            {
                Width = windowWidth,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                }
            };
            addDataStackPanel.Children.Add(addDataGrid);

            //add hotel licence number
            Label hotelLabel = new Label
            {
                Content = "Hotel : ",
                FontSize = 22
            };
            Grid.SetRow(hotelLabel, 0);
            Grid.SetColumn(hotelLabel, 0);
            addDataGrid.Children.Add(hotelLabel);

            ComboBox hotelComboBox = new ComboBox
            {
                FontSize = 22,
                Width = itemsWidth
            };
            List<Hotel> hotels = DataModels.GetInstance().GetAllHotels();
            foreach(Hotel hotel in hotels)
            {
                ComboBoxItem hotelItem = new ComboBoxItem
                {
                    Content = hotel
                };
                hotelComboBox.Items.Add(hotelItem);
            }
            if (hotelComboBox.Items.Count > 0)
                hotelComboBox.SelectedIndex = 0;
            Grid.SetRow(hotelComboBox, 0);
            Grid.SetColumn(hotelComboBox, 1);
            addDataGrid.Children.Add(hotelComboBox);

            //add room number
            Label roomNumberLabel = new Label
            {
                Content = "Room Number : ",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(roomNumberLabel, 1);
            Grid.SetColumn(roomNumberLabel, 0);
            addDataGrid.Children.Add(roomNumberLabel);

            TextBox roomNumberTextBox = new TextBox
            {
                FontSize = 22,
                Width = itemsWidth,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(roomNumberTextBox, 1);
            Grid.SetColumn(roomNumberTextBox, 1);
            addDataGrid.Children.Add(roomNumberTextBox);

            //add room type
            Label roomTypeLabel = new Label
            {
                Content = "Room Type : ",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(roomTypeLabel, 2);
            Grid.SetColumn(roomTypeLabel, 0);
            addDataGrid.Children.Add(roomTypeLabel);

            ComboBox roomTypeComboBox = new ComboBox
            {
                Width = itemsWidth,
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(roomTypeComboBox, 2);
            Grid.SetColumn(roomTypeComboBox, 1);
            addDataGrid.Children.Add(roomTypeComboBox);

            List<RoomType> roomTypes = DataModels.GetInstance().GetAllRoomTypes();
            for (int j = 0; j < roomTypes.Count; j++)
            {
                RoomType roomType = roomTypes[j];
                ComboBoxItem roomTypeItem = new ComboBoxItem
                {
                    Content = roomType
                };
                roomTypeComboBox.Items.Add(roomTypeItem);
            }
            if (roomTypeComboBox.Items.Count > 0)
                roomTypeComboBox.SelectedIndex = 0;

            //add room photo
            Label roomPhotoLabel = new Label
            {
                Content = "Room Photo : ",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(roomPhotoLabel, 3);
            Grid.SetColumn(roomPhotoLabel, 0);
            addDataGrid.Children.Add(roomPhotoLabel);

            Button browseRoomPhotoButton = CreateButton(itemsWidth, hotelLabel.Height, "Browse");
            browseRoomPhotoButton.Margin = new Thickness(0, 10, 0, 0);
            browseRoomPhotoButton.Click += BrowseRoomPhotoButton_Click;
            browseRoomPhotoButton.Tag = "";
            Grid.SetRow(browseRoomPhotoButton, 3);
            Grid.SetColumn(browseRoomPhotoButton, 1);
            addDataGrid.Children.Add(browseRoomPhotoButton);

            Button addRoomButton = CreateButton(100, 50, "Add");
            addRoomButton.Margin = new Thickness(170, 30, 0, 0);
            addRoomButton.HorizontalAlignment = HorizontalAlignment.Left;
            addRoomButton.Click += AddRoomButton_Click;
            List<object> tags = new List<object>();
            tags.Add(hotelComboBox);
            tags.Add(roomNumberTextBox);
            tags.Add(roomTypeComboBox);
            tags.Add(browseRoomPhotoButton);
            tags.Add(popup);
            addRoomButton.Tag = tags;
            addDataStackPanel.Children.Add(addRoomButton);
            popup.ShowDialog();
        }

        private static void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Button addRoomButton = (Button)sender;
            List<object> objects = (List<object>)addRoomButton.Tag;
            ComboBox hotelComboBox = (ComboBox)objects[0];
            TextBox roomNumberTextBox = (TextBox)objects[1];
            ComboBox roomTypeComboBox = (ComboBox)objects[2];
            Button browseRoomPhotoButton = (Button)objects[3];
            Window popup = (Window)objects[4];
            DataModels database = DataModels.GetInstance();

            if(hotelComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a hotel");
                return;
            }
            Hotel hotel = (Hotel)((ComboBoxItem)hotelComboBox.SelectedItem).Content;

            int roomNumber;
            if(!int.TryParse(roomNumberTextBox.Text, out roomNumber))
            {
                MessageBox.Show("Please enter a valid room number");
                return;
            }

            if(roomTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a room type");
                return;
            }
            RoomType roomType = (RoomType)((ComboBoxItem)roomTypeComboBox.SelectedItem).Content;

            if((string)browseRoomPhotoButton.Tag == "")
            {
                MessageBox.Show("Please select a photo path");
                return;
            }
            CustomImage roomImage = new CustomImage((string)browseRoomPhotoButton.Tag);

            Room room = new Room(roomNumber, hotel, roomType, roomImage, new List<RoomView>());
            if(database.AddRoom(room) == true)
            {
                MessageBox.Show("Added");
                if (adminWindow.currentCanvas != null)
                    adminWindow.currentCanvas.Hide();
                adminWindow.InitializeRoomsCanvas(database.GetAllRooms());
                popup.Close();
                return;
            }
            else
            {
                MessageBox.Show("please enter a valid room number");
                return;
            }
        }

        private static void BrowseRoomPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png",
                Title = "Select Room Photo"
            };
            dlg.ShowDialog();
            ((Button)sender).Tag = dlg.FileName.ToString();
        }

        public static void CreateAddRoomViewPopupWindow(Room room, ListBox viewsList)
        {
            double windowWidth = 400;
            Window popup = new Window
            {
                Width = windowWidth,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Add View"
            };

            StackPanel dataStackPanel = new StackPanel
            {
                Width = windowWidth
            };
            popup.Content = dataStackPanel;

            Grid dataGrid = new Grid
            {
                Width = windowWidth,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto}
                }
            };
            dataStackPanel.Children.Add(dataGrid);

            Label viewLabel = new Label
            {
                Content = "View : ",
                FontSize = 22
            };
            Grid.SetColumn(viewLabel, 0);
            dataGrid.Children.Add(viewLabel);

            TextBox viewTextBox = new TextBox
            {
                FontSize = 22,
                Width = 200
            };
            Grid.SetColumn(viewTextBox, 1);
            dataGrid.Children.Add(viewTextBox);

            Button addButton = CreateButton(80, 40, "Add");
            addButton.Margin = new Thickness(0, 20, 0, 0);
            addButton.Tag = new List<object>();
            ((List<object>)addButton.Tag).Add(room);
            ((List<object>)addButton.Tag).Add(viewsList);
            ((List<object>)addButton.Tag).Add(viewTextBox);
            ((List<object>)addButton.Tag).Add(popup);
            addButton.Click += AddButton_Click;
            dataStackPanel.Children.Add(addButton);

            popup.ShowDialog();
        }

        private static void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Button addViewButton = (Button)sender;
            List<object> objects = (List<object>)addViewButton.Tag;
            Room room = (Room)objects[0];
            ListBox listBox = (ListBox)objects[1];
            TextBox viewTextBox = (TextBox)objects[2];
            Window popup = (Window)objects[3];

            if(viewTextBox.Text == "")
            {
                MessageBox.Show("Invalid view");
                return;
            }

            RoomView roomView = new RoomView(viewTextBox.Text);

            if(DataModels.GetInstance().AddRoomView(room, roomView))
            {
                MessageBox.Show("Added");
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = "View : " + roomView.view,
                    FontSize = 22
                };
                listBox.Items.Add(listBoxItem);
                popup.Close();
            }
            else
            {
                MessageBox.Show("Invalid view");
            }
        }

        public static void CreateAddhotelPopupWindow()
        {
            double windowWidth = 500;

            Window popup = new Window
            {
                Width = windowWidth,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Add Hotel"
            };

            Grid dataGrid = new Grid
            {
                Width = windowWidth,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                Margin = new Thickness(20, 0.2 * windowWidth, 0, 0)
            };
            popup.Content = dataGrid;
           
            Label hotelNameLabel = new Label
            {
                Content = "Hotel Name : ",
                FontSize = 22
            };
            Grid.SetColumn(hotelNameLabel, 0);
            Grid.SetRow(hotelNameLabel, 0);
            dataGrid.Children.Add(hotelNameLabel);

            TextBox hotelNameTextBox = new TextBox
            {
                FontSize = 22,
                Width = 200
            };
            Grid.SetRow(hotelNameTextBox, 0);
            Grid.SetColumn(hotelNameTextBox, 1);
            dataGrid.Children.Add(hotelNameTextBox);

            Label licenseNumberLabel = new Label
            {
                Content = "License Number : ",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(licenseNumberLabel, 1);
            Grid.SetColumn(licenseNumberLabel, 0);
            dataGrid.Children.Add(licenseNumberLabel);

            TextBox licenseNumberTextBox = new TextBox
            {
                FontSize = 22,
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(licenseNumberTextBox, 1);
            Grid.SetColumn(licenseNumberTextBox, 1);
            dataGrid.Children.Add(licenseNumberTextBox);

            Label locationLabel = new Label
            {
                Content = "Location : ",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(locationLabel, 2);
            Grid.SetColumn(locationLabel, 0);
            dataGrid.Children.Add(locationLabel);

            ComboBox locationComboBox = new ComboBox
            {
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0),
                FontSize = 22
            };
            Grid.SetRow(locationComboBox, 2);
            Grid.SetColumn(locationComboBox, 1);
            List<Location> locations = DataModels.GetInstance().GetAllLocations();
            foreach(Location location in locations)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    FontSize = 22,
                    Content = location
                };
                locationComboBox.Items.Add(comboBoxItem);
            }
            if (locationComboBox.Items.Count > 0)
                locationComboBox.SelectedIndex = 0;
            dataGrid.Children.Add(locationComboBox);

            Label imageLabel = new Label
            {
                Content = "Image : ",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(imageLabel, 3);
            Grid.SetColumn(imageLabel, 0);
            dataGrid.Children.Add(imageLabel);

            Button browseButton = CreateButton(80, 40, "Browse");
            browseButton.Tag = "";
            browseButton.Click += HotelAddBrowseButton_Click;
            browseButton.Margin = new Thickness(0, 10, 0, 0);
            Grid.SetRow(browseButton, 3);
            Grid.SetColumn(browseButton, 1);
            dataGrid.Children.Add(browseButton);

            Button addButton = CreateButton(80, 40, "Add");
            addButton.Margin = new Thickness(0, 20, 0, 0);
            addButton.HorizontalAlignment = HorizontalAlignment.Center;
            addButton.Tag = new List<object>();
            ((List<object>)addButton.Tag).Add(hotelNameTextBox);
            ((List<object>)addButton.Tag).Add(licenseNumberTextBox);
            ((List<object>)addButton.Tag).Add(locationComboBox);
            ((List<object>)addButton.Tag).Add(browseButton);
            ((List<object>)addButton.Tag).Add(popup);
            addButton.Click += AddHotelButton_Click;
            Grid.SetColumnSpan(addButton, 2);
            Grid.SetRow(addButton, 4);
            dataGrid.Children.Add(addButton);

            popup.ShowDialog();
        }

        private static void AddHotelButton_Click(object sender, RoutedEventArgs e)
        {
            Button addButton = (Button)sender;
            List<object> objects = (List<object>)addButton.Tag;
            TextBox hotelNameTextBox = (TextBox)objects[0];
            TextBox licenseNumberTextBox = (TextBox)objects[1];
            ComboBox locationComboBox = (ComboBox)objects[2];
            Button browseButton = (Button)objects[3];
            Window popup = (Window)objects[4];

            int licenseNumber;
            if (!int.TryParse(licenseNumberTextBox.Text, out licenseNumber))
            {
                MessageBox.Show("Enter valid licence number");
                return;
            }

            if(hotelNameTextBox.Text == "")
            {
                MessageBox.Show("Enter Hotel Name");
                return;
            }

            if((string)browseButton.Tag == "")
            {
                MessageBox.Show("select a photo");
                return;
            }
            
            if(locationComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a location");
                return;
            }

            Hotel hotel = new Hotel(licenseNumber, hotelNameTextBox.Text, new CustomImage((string)browseButton.Tag), (Location)((ComboBoxItem)locationComboBox.SelectedItem).Content, new List<HotelFacility>(), new List<MealPlan>());

            if(DataModels.GetInstance().AddHotel(hotel))
            {
                MessageBox.Show("Added");
                Admin_window admin_Window = GetAdminWindow();
                if (admin_Window.currentCanvas != null)
                    admin_Window.currentCanvas.Hide();
                admin_Window.InitializeHotelsCanvas(DataModels.GetInstance().GetAllHotels());
                popup.Close();
            }
            else
            {
                MessageBox.Show("error 404");
            }
        }

        private static void HotelAddBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png",
                Title = "Select Hotel Photo"
            };
            dlg.ShowDialog();
            ((Button)sender).Tag = dlg.FileName;
        }

        public static void CreateAddMealPlanPopupWindow(Hotel hotel, ListBox listBox)
        {
            double windowWidth = 350;

            Window popup = new Window
            {
                Width = windowWidth,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Add Meal Plan"
            };
            
            Grid dataGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                Margin = new Thickness(20, 0.3 * windowWidth, 0, 0)
            };
            popup.Content = dataGrid;

            Label mealPlanNameLabel = new Label
            {
                Content = "Name : ",
                FontSize = 22
            };
            Grid.SetRow(mealPlanNameLabel, 0);
            Grid.SetColumn(mealPlanNameLabel, 0);
            dataGrid.Children.Add(mealPlanNameLabel);

            TextBox mealPlanNameTextBox = new TextBox
            {
                Width = 200,
                FontSize = 22
            };
            Grid.SetRow(mealPlanNameTextBox, 0);
            Grid.SetColumn(mealPlanNameTextBox, 1);
            dataGrid.Children.Add(mealPlanNameTextBox);

            Label priceLabel = new Label
            {
                Content = "Price : ",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(priceLabel, 1);
            Grid.SetColumn(priceLabel, 0);
            dataGrid.Children.Add(priceLabel);

            TextBox priceTextBox = new TextBox
            {
                Width = 200,
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(priceTextBox, 1);
            Grid.SetColumn(priceTextBox, 1);
            dataGrid.Children.Add(priceTextBox);

            Button addButton = CreateButton(80, 40, "Add");
            addButton.Margin = new Thickness(0, 20, 0, 0);
            addButton.Tag = new List<object>();
            ((List<object>)addButton.Tag).Add(hotel);
            ((List<object>)addButton.Tag).Add(listBox);
            ((List<object>)addButton.Tag).Add(mealPlanNameTextBox);
            ((List<object>)addButton.Tag).Add(priceTextBox);
            ((List<object>)addButton.Tag).Add(popup);
            addButton.Click += MealPlanAddButton_Click;
            Grid.SetColumnSpan(addButton, 2);
            Grid.SetRow(addButton, 2);
            dataGrid.Children.Add(addButton);

            popup.ShowDialog();
        }

        private static void MealPlanAddButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            List<object> objects = (List<object>)button.Tag;
            Hotel hotel = (Hotel)objects[0];
            ListBox listBox = (ListBox)objects[1];
            TextBox nameTextBox = (TextBox)objects[2];
            TextBox priceTextBox = (TextBox)objects[3];
            Window popup = (Window)objects[4];

            if(nameTextBox.Text == "")
            {
                MessageBox.Show("Please Enter MealPlan Name");
                return;
            }
            int price;
            if(!int.TryParse(priceTextBox.Text, out price))
            {
                MessageBox.Show("Enter a valid price");
                return;
            }
            List<MealPlan> mealPlans = new List<MealPlan>();
            mealPlans.Add(new MealPlan(nameTextBox.Text, price));
            if(DataModels.GetInstance().AddMealPlans(hotel.licenseNumber, mealPlans))
            {
                MessageBox.Show("added");
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = mealPlans[0],
                    FontSize = 22
                };
                listBox.Items.Add(listBoxItem);
                popup.Close();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
