using Faregosoft.Models;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Faregosoft.Components
{
    public sealed partial class Pagination : UserControl
    {
        private readonly long _totalRecords;
        private Color _gray = new Color() { A = 255, R = 220, G = 220, B = 220 };
        private Color _yellow = new Color() { A = 255, R = 248, G = 199, B = 0 };
        private List<PagerModel> _pages;
        private int _size = 20;
        private int _totalPages = 0;
        private int _index = 0;

        public Pagination(long totalRecords)
        {
            InitializeComponent();
            _totalRecords = totalRecords;
            SetPages();
            Page = 1;
            Size = 20;
        }

        public int Page
        {
            get => (int)GetValue(PageProperty);
            set => SetValue(PageProperty, value);
        }

        public static readonly DependencyProperty PageProperty = DependencyProperty.Register("Page", typeof(int), typeof(Pagination), null);

        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(int), typeof(Pagination), null);


        public EventHandler PageChanged
        {
            get => (EventHandler)GetValue(PageChangedProperty);
            set => SetValue(PageChangedProperty, value);
        }

        public static readonly DependencyProperty PageChangedProperty = DependencyProperty.Register("PageChanged", typeof(EventHandler), typeof(Pagination), null);

        private void SetPages()
        {
            _totalPages = (int)_totalRecords / _size;
            if ((double)(_totalRecords / _size) > 0)
            {
                _totalPages++;
            }

            if (_totalPages == 0)
            {
                _totalPages = 1;
            }

            BuildPages();
        }

        private void BuildPages()
        {
            AddPages();
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            MyPager.Children.Clear();
            AddFirstButton();
            AddPrevButton();
            AddMiddleButtons();
            AddNextButton();
            AddLastButton();
        }

        private void AddPages()
        {
            _pages = new List<PagerModel>();
            if (_totalPages < 7)
            {
                for (int i = 0; i < _totalPages; i++)
                {
                    _pages.Add(new PagerModel { Page = i + 1, Color = _gray });
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    _pages.Add(new PagerModel { Page = i + 1, Color = _gray });
                }

                for (int i = _totalPages - 3; i < _totalPages; i++)
                {
                    _pages.Add(new PagerModel { Page = i + 1, Color = _gray });
                }
            }

            _pages[_index].Color = _yellow;
        }

        private void AddFirstButton()
        {
            StackPanel stackPanel = new StackPanel
            {
                Background = new SolidColorBrush(_gray),
                VerticalAlignment = VerticalAlignment.Center,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(5),
                Margin = new Thickness(0, 0, 5, 0),
            };
            stackPanel.Tapped += FirstButton_Tapped;

            Image image = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/icon-FirstPage.png")),
                Height = 18,
                VerticalAlignment = VerticalAlignment.Center,
            };

            stackPanel.Children.Add(image);
            MyPager.Children.Add(stackPanel);
        }

        private void AddPrevButton()
        {
            StackPanel stackPanel = new StackPanel
            {
                Background = new SolidColorBrush(_gray),
                VerticalAlignment = VerticalAlignment.Center,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(5),
                Margin = new Thickness(0, 0, 5, 0),
            };
            stackPanel.Tapped += PrevButton_Tapped;

            Image image = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/icon-arrow-left.png")),
                Height = 18,
                VerticalAlignment = VerticalAlignment.Center,
            };

            stackPanel.Children.Add(image);
            MyPager.Children.Add(stackPanel);
        }

        private void AddMiddleButtons()
        {
            if (_totalPages < 7)
            {
                AddMiddleButonsWithoutEllipsis();
            }
            else
            {
                AddMiddleButonsWithEllipsis();
            }
        }

        private void AddMiddleButonsWithoutEllipsis()
        {
            for (int i = 0; i < _totalPages; i++)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Background = new SolidColorBrush(_pages[i].Color),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(8, 4, 8, 4),
                    Margin = new Thickness(0, 0, 0, 0),
                    Width = 50,
                    Tag = i
                };
                stackPanel.Tapped += Button_Tapped;

                TextBlock textBlock = new TextBlock
                {
                    Text = $"{_pages[i].Page}",
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold
                };

                stackPanel.Children.Add(textBlock);
                MyPager.Children.Add(stackPanel);
            }
        }

        private void AddMiddleButonsWithEllipsis()
        {
            for (int i = 0; i < 3; i++)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Background = new SolidColorBrush(_pages[i].Color),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(8, 4, 8, 4),
                    Margin = new Thickness(0, 0, 0, 0),
                    Width = 50,
                    Tag = i
                };
                stackPanel.Tapped += Button_Tapped;

                TextBlock textBlock = new TextBlock
                {
                    Text = $"{_pages[i].Page}",
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold
                };

                stackPanel.Children.Add(textBlock);
                MyPager.Children.Add(stackPanel);
            }

            StackPanel ellipsisStackPanel = new StackPanel
            {
                Background = new SolidColorBrush(_gray),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(0, 0, 0, 0),
                Width = 50
            };

            TextBlock ellipsisTextBlock = new TextBlock
            {
                Text = "...",
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold
            };

            ellipsisStackPanel.Children.Add(ellipsisTextBlock);
            MyPager.Children.Add(ellipsisStackPanel);

            for (int i = 0; i < 3; i++)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Background = new SolidColorBrush(_pages[i + 3].Color),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(8, 4, 8, 4),
                    Margin = new Thickness(0, 0, 0, 0),
                    Width = 50,
                    Tag = i + 3
                };
                stackPanel.Tapped += Button_Tapped;

                TextBlock textBlock = new TextBlock
                {
                    Text = $"{_pages[i + 3].Page}",
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold
                };

                stackPanel.Children.Add(textBlock);
                MyPager.Children.Add(stackPanel);
            }
        }

        private void AddNextButton()
        {
            StackPanel stackPanel = new StackPanel
            {
                Background = new SolidColorBrush(_gray),
                VerticalAlignment = VerticalAlignment.Center,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(5),
                Margin = new Thickness(5, 0, 0, 0),
            };
            stackPanel.Tapped += NextButton_Tapped;

            Image image = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/icon-arrow-right.png")),
                Height = 18,
                VerticalAlignment = VerticalAlignment.Center,
            };

            stackPanel.Children.Add(image);
            MyPager.Children.Add(stackPanel);
        }

        private void AddLastButton()
        {
            StackPanel stackPanel = new StackPanel
            {
                Background = new SolidColorBrush(_gray),
                VerticalAlignment = VerticalAlignment.Center,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(5),
                Margin = new Thickness(5, 0, 0, 0),
            };
            stackPanel.Tapped += LastButton_Tapped;

            Image image = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/icon-LastPage.png")),
                Height = 18,
                VerticalAlignment = VerticalAlignment.Center,
            };

            stackPanel.Children.Add(image);
            MyPager.Children.Add(stackPanel);
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _index = int.Parse($"{(sender as StackPanel).Tag}");
            SetAllPagesGray();
            _pages[_index].Color = _yellow;
            RefreshButtons();
            UpdatePage();
        }

        private void FirstButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _index = 0;
            BuildPages();
            UpdatePage();
        }

        private void LastButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _index = _pages.Count - 1;
            BuildPages();
            UpdatePage();
        }

        private void NextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_index == 5)
            {
                return;
            }

            if (_totalPages < 7 && _index + 1 == _pages[_pages.Count - 1].Page)
            {
                return;
            }

            SetAllPagesGray();

            if (_totalPages < 7 || _index < 2)
            {
                _index++;
                _pages[_index].Color = _yellow;
                RefreshButtons();
                UpdatePage();
                return;
            }

            if (_index == 2 && _pages[_index + 1].Page - _pages[_index].Page > 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    _pages[i].Page++;
                }
                _pages[2].Color = _yellow;
                RefreshButtons();
                UpdatePage();
                return;
            }

            if (_index == 2 && _pages[_index + 1].Page - _pages[_index].Page == 1)
            {
                _index++;
                _pages[_index].Color = _yellow;
                RefreshButtons();
                UpdatePage();
                return;
            }

            _index++;
            _pages[_index].Color = _yellow;
            RefreshButtons();
            UpdatePage();
        }

        private void UpdatePage()
        {
            Page = _pages[_index].Page;
            EventHandler handler = PageChanged;
            if (handler != null)
            {
                handler(this, null);
            }
        }

        private void PrevButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_index == 0 && _pages[0].Page == 1)
            {
                return;
            }

            SetAllPagesGray();

            if (_index == 0 && _pages[0].Page != 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    _pages[i].Page--;
                }
                _pages[0].Color = _yellow;
                RefreshButtons();
                UpdatePage();
                return;
            }

            if (_totalPages < 7 || _index > 3)
            {
                _index--;
                _pages[_index].Color = _yellow;
                RefreshButtons();
                UpdatePage();
                return;
            }

            if (_index == 3 && _pages[_index].Page - _pages[_index - 1].Page > 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    _pages[i + 3].Page--;
                }
                _pages[3].Color = _yellow;
                RefreshButtons();
                UpdatePage();
                return;
            }

            if (_index == 3 && _pages[_index].Page - _pages[_index - 1].Page == 1)
            {
                _index--;
                _pages[_index].Color = _yellow;
                RefreshButtons();
                UpdatePage();
                return;
            }

            _index--;
            _pages[_index].Color = _yellow;
            RefreshButtons();
            UpdatePage();
        }

        private void SetAllPagesGray()
        {
            foreach (PagerModel page in _pages)
            {
                page.Color = _gray;
            }
        }

        private void SizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string size = e.AddedItems[0].ToString();
            _index = 0;
            _size = int.Parse(size);
            Size = _size;
            SetPages();
            UpdatePage();
        }
    }
}
