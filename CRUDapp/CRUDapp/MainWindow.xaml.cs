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
using Microsoft.EntityFrameworkCore;
using CRUDapp.Models;
using System.IO;
using System.ComponentModel;

namespace CRUDapp
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WorldSkills2Context db;

        public MainWindow()
        {
            InitializeComponent();

            db = new WorldSkills2Context();
            db.Users.Load(); // загружаем данные
            UsersGrid.ItemsSource = db.Users.Local.ToBindingList(); // устанавливаем привязку к кэшу

            this.Closing += Window_Closing;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // clean up database connections
            db.Dispose();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < UsersGrid.SelectedItems.Count; i++)
                {
                    User user = UsersGrid.SelectedItems[i] as User;
                    if (user != null)
                    {
                        db.Users.Remove(user);
                    }
                }
            }
            db.SaveChanges();
        }

        private void AddSortColumn(DataGrid sender, string sortColumn, ListSortDirection direction)

        {

            var cView = CollectionViewSource.GetDefaultView(sender.ItemsSource);

            cView.SortDescriptions.Add(new SortDescription(sortColumn, direction));

            //Add the sort arrow on the DataGridColumn 

            foreach (var col in sender.Columns.Where(x => x.SortMemberPath == sortColumn))

            {

                col.SortDirection = direction;

            }

        }

        private void UsersGrid_Sorting(object sender, DataGridSortingEventArgs e)

        {

            var dgSender = (DataGrid)sender;

            var cView = CollectionViewSource.GetDefaultView(dgSender.ItemsSource);



            //Alternate between ascending/descending if the same column is clicked 

            ListSortDirection direction = ListSortDirection.Ascending;

            if (cView.SortDescriptions.FirstOrDefault().PropertyName == e.Column.SortMemberPath)

                direction = cView.SortDescriptions.FirstOrDefault().Direction == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;



            cView.SortDescriptions.Clear();

            AddSortColumn((DataGrid)sender, e.Column.SortMemberPath, direction);

            //To this point the default sort functionality is implemented 



            //Now check the wanted columns and add multiple sort 

            if (e.Column.SortMemberPath == "WantedColumn")

            {

                AddSortColumn((DataGrid)sender, "SecondColumn", direction);

            }

            e.Handled = true;

        }

        /*
        private void AddImageToUser(object sender, MouseButtonEventArgs e)
        {
            Stream myStream;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                if ((myStream = dlg.OpenFile()) != null)
                {
                    string strfilename = dlg.FileName;
                    string filetext = File.ReadAllText(strfilename);

                    dlg.DefaultExt = ".png";
                    dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                    dlg.Title = "Open Image";
                    dlg.InitialDirectory = "./";

                    BitmapImage image = new BitmapImage(new Uri(dlg.FileName));
                    ImageBox.Source = image;

                    try
                    {
                        string newRelativePath = $"{System.DateTime.Now.ToString("HHmmss")}_{dlg.SafeFileName}";
                        File.Copy(dlg.FileName, System.IO.Path.Combine(Environment.CurrentDirectory, $"images/{newRelativePath}"));
                        ImagePath = newRelativePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                myStream.Dispose();

            }
        }
        */
    }
}
