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
using ExpenseObject;
using MySql.Data.MySqlClient;

namespace Updated_Financial_Manager_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<Expense> foundExpenses { get; private set; }  

        public MainWindow()
        {
            InitializeComponent();
            initWelcomeText();
            initUpdateTip();
            populateExpenseTable();

        }

        private void initWelcomeText()
        {

            welcomeText.Text = "Welcome to your Financial Planner! With this app, you can do the following: \n • Enter Your Expenses with descriptive information \n" +
                " • Update Expenses when info changes \n" +
                " • Delete Expenses when no longer due \n" +
                " • Make plan to save money and get detailed report regarding amount needed to reach savings goal \n" +
                "\n Saving can be tough, but you got this!";

        }

        private void initUpdateTip()
        {

            multipleUpdatesTip.Text = "Dev Tip: \n" +
                "To update the status of multiple expenses to \"Paid\", you must enter in the Id's of the records you want to update by entering the Id followed by the : character. \n" +
                "• Example: 1:2:3:4, Expenses with an Id of 1, 2, 3, and 4 would be updated";

        }

        /*
        *  @author Rick Morris
        *  This method is used to retrieve an open MySqlConnection to perform database operations
        */
        private MySqlConnection getOpenConnection()
        {

            //Connect to Database....
            string dataConnection = "datasource=localhost;port=3306;database=expense;username=root;password=MightyLotus215";

            //Write query
            MySqlConnection newConnection = new MySqlConnection(dataConnection);
            newConnection.Open();

            return newConnection;

        }

        /*
        *  @author Rick Morris
        *  This method is used to close an open MySqlConnection Connection
        */ 
        private void closeConnection(MySqlConnection connectionToClose)
        {
            connectionToClose.Close();
        }

        private void populateExpenseTable()
        {

            try
            {

                this.foundExpenses = new List<Expense>();

                MySqlConnection sqlConnection = getOpenConnection();

                //Write query
                string query = "SELECT * FROM Expenses";

                MySqlCommand myCommand = new MySqlCommand(query, sqlConnection);
                MySqlDataReader reader = myCommand.ExecuteReader();


                //While reader is reading, add items found to list...
                this.foundExpenses = new List<Expense>();
                Console.WriteLine("List Size: " + foundExpenses.Count);
                while (reader.Read())
                {
                    String tempName = reader["Name"].ToString();
                    String tempOwner = reader["Owner"].ToString();
                    String tempId = reader["id"].ToString();
                    String strAmount = reader["Amount"].ToString();
                    Decimal tempAmount = Decimal.Parse(strAmount);
                    String tempDueDate = reader["DueDate"].ToString();
                    String tempIssuer = reader["Issuer"].ToString();
                    String tempStatus = reader["Status"].ToString();

                    this.foundExpenses.Add(new Expense(tempName, tempOwner, tempId, tempAmount, tempDueDate, tempIssuer, tempStatus));
                }

                //Loop through this list and add new row to table
                /*for (int i = 0; i < this.foundExpenses.Count; i++)
                {
                    if (this.foundExpenses != null && this.foundExpenses.Count != 0)
                        expenseTable.Items.Add(this.foundExpenses[i]);

                }
                */
                expenseTable.ItemsSource = this.foundExpenses;

                errorBlockHome.Text = "Successfully found expenses!";
                errorBlockHome.Foreground = Brushes.Green;

            } catch (Exception e)
            {
                errorBlockHome.Text = "Error: " + e.Message;
                errorBlockHome.Foreground = Brushes.Red; 
            }

        }

        private void addExpenseBtn(object sender, RoutedEventArgs e)
        {

            try
            {

                MySqlConnection newConnection = getOpenConnection();

                //Write query
                string query = "insert into Expenses(Owner, Name, Amount, DueDate, Issuer, Status) values(" + "'Rick'" + ", '" + newNameBox.Text + "', " + newAmountBox.Text + ", '" + newDueDateBox.Text + "', '" + newIssuerBox.Text + "', '" + newStatusBox.Text + "');";
                errorBlockAdd.Text = query + "\n";
                MySqlCommand myCommand = new MySqlCommand(query, newConnection);
                MySqlDataReader reader = myCommand.ExecuteReader();


                //While reader is reading, add items found to list...
                while (reader.Read())
                {

                }

                //Close connection after the data is received
                closeConnection(newConnection);

                populateExpenseTable();

                errorBlockAdd.Text += "Expense successfully added!";
                errorBlockAdd.Foreground = Brushes.Green;

            }
            catch (Exception excep)
            {

                errorBlockAdd.Text += "Your expense was unsuccessfully added. Error: " + excep.Message;
                errorBlockAdd.Foreground = Brushes.Red;
                Console.WriteLine(excep.Message);

            }

        }

        private void updateBtn(object sender, RoutedEventArgs e)
        {

            try
            {

                //Some fields cannot be changed so set defaulted items here
                String defaultOwner = "Rick";

                //Get Expense to use for update
                Expense exp = new Expense(updateNameBox.Text, defaultOwner, idToEditBox.Text, Decimal.Parse(updateAmountBox.Text), updateDueDateBox.Text, updateIssuerBox.Text, updateStatusBox.Text);

                //Create Update Query
                String query = "UPDATE Expenses SET id='" + exp.Id + "',Owner='" + exp.Owner + "',Name='" + exp.Name + "',Amount='" + Convert.ToString(exp.Amount) + "',DueDate='" + exp.DueDate + "',Issuer='" + exp.Issuer + "',Status='" + exp.Status + "' where id='" + exp.Id + "';";
                
                //Connect to DB
                MySqlConnection myConnection = getOpenConnection();
                MySqlCommand myCommand = new MySqlCommand(query, myConnection);
                MySqlDataReader dataReader;
                
                dataReader = myCommand.ExecuteReader();
                errorBlockUpdate.Text = "Records Updated!";
                errorBlockUpdate.Foreground = Brushes.Green;
                while (dataReader.Read())
                {

                }
                closeConnection(myConnection);

                populateExpenseTable();

            } catch (Exception error)
            {

                errorBlockUpdate.Text = "Update Failed. Error " + error.Message;
                errorBlockUpdate.Foreground = Brushes.Red;

            }

        }

        private void deleteBtn(object sender, RoutedEventArgs e)
        {

            try
            {


                String query = "delete from Expenses where id ='" + idToDelete.Text + "';";

                MySqlConnection connection = getOpenConnection();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while(reader.Read()){

                }

                closeConnection(connection);

                errorBlockDelete.Text = "Your expense has been deleted!";
                errorBlockDelete.Foreground = Brushes.Green;

                populateExpenseTable();


            }
            catch(Exception error)
            {

                errorBlockDelete.Text = "There was an error: " + error.Message;
                errorBlockDelete.Foreground = Brushes.Red;

            }

        }

        private void getExpenseInfo(object sender, RoutedEventArgs e)
        {

            if(this.foundExpenses != null)
            {

                try
                {

                    //If the user entered the Id of a record then get the record and populate the fields in the UI               
                    Expense expenseToEdit = getExpenseToEdit(idToEditBox.Text);

                    if (expenseToEdit == null)
                        throw new Exception("You do not have an expense with that Id, please enter a valId");

                    errorBlockUpdate.Text = "We found the expense you want to edit!";

                    updateNameBox.Text = expenseToEdit.Name;
                    updateAmountBox.Text = expenseToEdit.Amount.ToString();
                    updateDueDateBox.Text = expenseToEdit.DueDate;
                    updateIssuerBox.Text = expenseToEdit.Issuer;
                    idToEditBox.Text = expenseToEdit.Id;
                    updateStatusBox.Text = expenseToEdit.Status;

                }
                catch (Exception error)
                {
                    errorBlockUpdate.Text = error.ToString();
                    errorBlockUpdate.Foreground = Brushes.Red;
                }

            }

        }

        private Expense getExpenseToEdit(String idToSearch)
        {
            if (foundExpenses != null)
            {

                foreach (Expense e in foundExpenses)
                {

                    if (e.Id == idToSearch)
                        return e;

                }

            }

            return null;
        }
    }
}
