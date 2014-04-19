using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace LetsPoll
{
    public partial class BarChartResult : PhoneApplicationPage
    {
        public BarChartResult()
        {
            InitializeComponent();
            this.MyBarSeriesChart.DataContext = new Point[] { new Point(0, 2), new Point(1, 10), new Point(2, 6) };
        }
    }
}