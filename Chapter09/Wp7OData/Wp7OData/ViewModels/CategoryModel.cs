using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace Wp7OData.ViewModels {
    public class CategoryModel {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }        
    }
}
