using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DELISAIMAGE.Class;
using DELISAIMAGE.Model;
using DELISAIMAGE.UserControl;
using DELISAIMAGE.ViewModel;
using Microsoft.Win32;

namespace DELISAIMAGE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VMMain vMMain;
        public MainWindow()
        {
            InitializeComponent();
            vMMain = (VMMain)DataContext;
        }

        private void OpenFileDiaLogClick(object sender, RoutedEventArgs e)
        {
            var openfile = new OpenFileDialog();
            openfile.Multiselect = true;
            openfile.Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openfile.ShowDialog() == true)
            {
                foreach(var file in openfile.FileNames)
                {
                    var modelImage = new ModelImage
                    {
                        Imagepath = file
                    };
                    var select = (vMMain.ModelImages.ToList()).FirstOrDefault(x => x.Imagepath == file);
                    if(select == null)
                        vMMain.ModelImages.Add(modelImage);
                }
            }
        }

        private void UCList_OnBorderTextRowMouseDown(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not TextBlock textBlock) return;
            if (textBlock.DataContext is ModelImage model)
            {
                image.Source = new BitmapImage(new Uri(model.Imagepath));
            }
        }

        private void AddBoxClick(object sender, RoutedEventArgs e)
        {
            vMMain.BoxDataAdd();
        }

        private void OpenFileAllRemoveClick(object sender, RoutedEventArgs e)
        {
            vMMain.ModelImages.Clear();
        }

        private void BoxDataAllRemoveClick(object sender, RoutedEventArgs e)
        {
            vMMain.BoxLocations.Clear();
        }

        private void BtnANALYZEClick(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                var OpenCv = new OpenCv();
                await OpenCv.Select(vMMain.ModelImages.ToList(), vMMain.BoxLocations.ToList());
                MessageBox.Show("완료");
            });
        }
    }
}