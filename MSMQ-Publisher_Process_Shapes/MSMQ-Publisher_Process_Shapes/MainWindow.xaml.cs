using MSMQ_Publisher_Process_Shapes.ViewModel;
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

namespace MSMQ_Publisher_Process_Shapes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ShapeViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm = new ShapeViewModel();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var maxposition = e.GetPosition(canvas);
            var selectedShape = ToolBox.SelectedItem as Shape;

            // Check if the selected shape is null or the type is supported   
            if (selectedShape == null || !selectedShape.GetType().IsSubclassOf(typeof(Shape))) return;

            //foreach (var child in canvas.Children.OfType<Shape>().ToList())
            //{
            //    canvas.Children.Remove(child);
            //}

            var shape = Activator.CreateInstance(selectedShape.GetType()) as Shape;
            if (shape == null) return;

            // Set the default properties of the new shape            
            shape.Width = selectedShape.Width;
            shape.Height = selectedShape.Height;
            shape.Stroke = selectedShape.Stroke;
            shape.Fill = selectedShape.Fill;
            var size = new Size(selectedShape.Width, selectedShape.Height);
            shape.Measure(size);
            shape.Arrange(new Rect(size));
            shape.UpdateLayout();

            // Set the position of the new shape on the canvas
            Canvas.SetLeft(shape, maxposition.X);
            Canvas.SetTop(shape, maxposition.Y);

            // Add the new shape to the canvas
            canvas.Children.Add(shape);

            vm.Publish(shape, maxposition.X, maxposition.Y);
        }
    }
}
