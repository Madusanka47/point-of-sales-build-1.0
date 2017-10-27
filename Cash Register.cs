using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Data.SqlClient;


namespace point_of_sales_build_1._0
{
    public partial class Cash_Register : Telerik.WinControls.UI.RadForm
    {
       
        DatabaseCore get = new DatabaseCore();

        SqlDataReader data;
        string firstColum;
        string secondColum;
        string fourthColum;
        string descript, itemQty, unitPrice;
        double totalAmount;
        double pageTotal;
        int qtyCount ;
        int totalCount;



        private void radTextBox2_KeyDown(object sender, KeyEventArgs e)
        {

            secondColum = radTextBox2.Text;
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(radTextBox2.Text))
                {   //dummy message box need to implement !! <---Reminder !!
                    MessageBox.Show("Please Enter Valied Item ", "Feld Cannot Be Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string Query = "SELECT description,item_qty,unit_price FROM item WHERE item_no = '" + secondColum + "'";
                    data = get.getData(Query);
                    if (data == null || !data.HasRows)
                    {   //dummy message box need to implement !! <---Reminder !!
                        MessageBox.Show("Item Not Available ", "Data Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        data.Close();
                    }
                    else
                    {
                        while (data.Read())
                        {
                            descript = data["description"].ToString();
                            itemQty = data["item_qty"].ToString();
                            unitPrice = data["unit_price"].ToString();
                        }
                        data.Close();
                        radTextBox3.Text = descript;
                        int count = calculateCount();
                        qtyCount = count;
                        radTextBox4.Focus();
                       
                    }

                }

            }
        }
        private void radTextBox2_TextChanged(object sender, EventArgs e)
        {
            radTextBox2.CharacterCasing = CharacterCasing.Upper;
        }

        public Cash_Register()
        {
            InitializeComponent();  // Form Start constructor !! --> Finds here <--
                radLabel5.Text = DateTime.Now.ToString();
                radTextBox3.BackColor = BackColor;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            getTotal();
        }
        public void getTotal()
        {
            for (int i = 0; i < radGridView1.Rows.Count; i++)
            {
                pageTotal = pageTotal + Convert.ToDouble(radGridView1.Rows[i].Cells[5].Value);
                //Generate total value !! 
            }
            checkOut set = new checkOut();
            
            set.getTotal(pageTotal.ToString("0.00"));
            set.ShowDialog();


        }

        private void radTextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                firstColum = radTextBox1.Text;
                fourthColum = radTextBox4.Text;
               // MessageBox.Show(qtyCount.ToString());  for testing!!
                validateQty();
               
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel order", "Cancel", MessageBoxButtons.YesNo);//dummy message box need to implement !! <---Reminder !!
            if (dialogResult == DialogResult.Yes) {
                clearFeilds();
            radGridView1.Rows.Clear();
            radGridView1.Refresh();
            radTextBox1.Focus();
            }



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            radLabel5.Text = DateTime.Now.ToString();
        }

        public void validateQty()
        {
            if (string.IsNullOrEmpty(radTextBox1.Text))
            {
                MessageBox.Show("Please Enter Salesman's id to continue");//dummy message box need to implement !! <---Reminder !!
            }
            else
            {

                if (string.IsNullOrEmpty(radTextBox4.Text))
                {
                    MessageBox.Show("Quantity cannot be black"); //dummy message box need to implement !! <---Reminder !!
                }
                else if (Convert.ToInt32(radTextBox4.Text) > Convert.ToInt32(itemQty))
                {
                    MessageBox.Show("Out of stock"); //dummy message box need to implement !! <---Reminder !!
                }
                else
                {
                    int tempCount = qtyCount + Convert.ToInt32(fourthColum);
                    if (tempCount > Convert.ToInt32( itemQty))
                    {
                        MessageBox.Show("Out of stock");//dummy message box need to implement !! <---Reminder !!
                    }
                    else { addRows(); }
               
                }
               }
            }
            public void addRows(){

            totalAmount = Convert.ToInt32(fourthColum) * Convert.ToDouble(unitPrice);
            string[] row = { firstColum, secondColum, descript, fourthColum, unitPrice, totalAmount.ToString("0.00") };
            radGridView1.Rows.Add(row);
            clearFeilds();

            radTextBox1.Text = firstColum;
            descript = null;
            radTextBox2.Focus();  

        }
        public void clearFeilds()
        {
            Action<Control.ControlCollection> func = null; //Finaly to clear the feild to next data!! Start <--
            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };
            func(Controls);//End--->

        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            clearFeilds();
            radTextBox2.Focus();
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void radTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void radTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                radTextBox2.Focus();

            }
        }

        public void updateItemTable()
        {
            for (int i = 0; i <= radGridView1.Rows.Count; i++)
            {
                try
                {
                    string itemName = radGridView1.Rows[i].Cells[1].Value.ToString();
                    string itemQty = radGridView1.Rows[i].Cells[3].Value.ToString();
                    string actualQty = get.getSingleData("SELECT item_qty FROM item WHERE item_no = '" + itemName + "' ");//test
                    int newQtyValue = Convert.ToInt32(actualQty) - Convert.ToInt32(itemQty);
                    int output = get.updateTableData("UPDATE item SET item_qty = '" + newQtyValue + "' WHERE item_no = '" + itemName + "'"); //validation !
                }
                catch (Exception e)
                {
                    MessageBox.Show("Thank you for using!"); //dummy message box need to implement !! <---Reminder !!}
                }
            }
        }
        public int calculateCount() {

            string tempname = radTextBox2.Text;
            int a = 0;
            for (int i = 0; i < radGridView1.Rows.Count; i++)
            {

                string name = radGridView1.Rows[i].Cells[1].Value.ToString();
                if (tempname.Equals(name))
                
               {
                    a = a + Convert.ToInt32(radGridView1.Rows[i].Cells[3].Value);
                }
            }

            return a;
        }
       
    }

}
