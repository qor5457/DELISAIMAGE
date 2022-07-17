using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DELISAIMAGE.UserControl
{
    public partial class UCList : System.Windows.Controls.UserControl
    {
        public UCList()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DataTemplate = DependencyProperty.Register("Template", typeof(string), typeof(UCList));
        public string Template
        {
            get => (string)GetValue(DataTemplate);
            set => SetValue(DataTemplate, value);
        }
        
        #region Button
        public static readonly RoutedEvent DRdRdBtnClick = EventManager.RegisterRoutedEvent("RdBtnClick", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(UCList));
        public static readonly RoutedEvent DGrGrBtnClick = EventManager.RegisterRoutedEvent("GrBtnClick", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(UCList));
        public static readonly DependencyProperty DGrBtnContent = DependencyProperty.Register("Title", typeof(string), typeof(UCList));
        
        public event RoutedEventHandler RdBtnClick
        {
            add { AddHandler(DRdRdBtnClick, value); }
            remove { RemoveHandler(DRdRdBtnClick, value); }
        }
        
        public event RoutedEventHandler GrBtnClick
        {
            add { AddHandler(DGrGrBtnClick, value); }
            remove { RemoveHandler(DGrGrBtnClick, value); }
        }
        private void BtnRdClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UCList.DRdRdBtnClick,this));
        }
        private void BtnGRClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UCList.DGrGrBtnClick,this));
        }
        public string GrBtnContent
        {
            get => (string)GetValue(DGrBtnContent);
            set => SetValue(DGrBtnContent, value);
        }
        #endregion
        public static readonly RoutedEvent TextRowMouseDown = EventManager.RegisterRoutedEvent("BorderTextRowMouseDown", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(UCList));
        
        public event RoutedEventHandler BorderTextRowMouseDown
        {
            add { AddHandler(TextRowMouseDown, value); }
            remove { RemoveHandler(TextRowMouseDown, value); }
        }
        
        private void RowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not TextBlock textBlock) return;
            RaiseEvent(new RoutedEventArgs(UCList.TextRowMouseDown, textBlock));
        }
    }
}