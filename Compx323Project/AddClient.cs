using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compx323Project
{
    public partial class AddClient : ModifiableForm
    {
        string query = "SELECT FIRSTNAME || ' ' || LASTNAME || ', from ' || CAREGIVERCOMPANY, CAREGIVERID FROM CAREGIVER";
        List<String> caregiverIDS = new List<string>();
        string clientID;
        public AddClient()
        {
            InitializeComponent();
            SetupButtons();
        }
        public override void UpdateForm()
        {
            base.UpdateForm();
            clientID = SQLHelper.identifiers[StartScreen.displayList.SelectedIndex - 1];
            DataTable table = SQLHelper.GetDataTable("SELECT * FROM CLIENT WHERE CLIENTID = '" + clientID + "'");
            txtFirstName.Text = table.Rows[0][1].ToString();
            txtLastName.Text = table.Rows[0][2].ToString();
            txtStreetNumber.Text = table.Rows[0][3].ToString();
            txtStreetName.Text = table.Rows[0][4].ToString();
            txtCity.Text = table.Rows[0][5].ToString();
            txtPostcode.Text = table.Rows[0][6].ToString();
            txtCountry.Text = table.Rows[0][7].ToString();
            txtPhone.Text = table.Rows[0][8].ToString();
            dateTimePicker1.Value = Convert.ToDateTime(table.Rows[0][9]);
            caregiverDropDown.SelectedIndex = caregiverIDS.IndexOf(table.Rows[0][10].ToString());
        }

        public override void ShowForm()
        {
            base.ShowForm();
            if (StartScreen.radOracle.Checked) FillDropDown(query, "No Caregivers Employed", ref caregiverIDS, ref caregiverDropDown);
            else FillDropDown("Caregiver", ref caregiverIDS, ref caregiverDropDown);
        }

        public void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime date = dateTimePicker1.Value;
            string firstName = FormHelper.FormatWord(txtFirstName.Text.Trim()),
                lastName = FormHelper.FormatWord(txtLastName.Text.Trim()),
                streetNumber = FormHelper.FormatNumber(txtStreetNumber.Text.Trim()),
                streetName = FormHelper.FormatWords(txtStreetName.Text.Trim()),
                postcode = FormHelper.FormatNumber(txtPostcode.Text.Trim()),
                country = FormHelper.FormatWord(txtCountry.Text.Trim()),
                city = FormHelper.FormatWord(txtCity.Text.Trim()),
                phone = FormHelper.FormatNumber(txtPhone.Text.Trim()),
                caregiverID = caregiverIDS[caregiverDropDown.SelectedIndex];

            if (FormHelper.StringError(firstName, "Please enter valid first name.")) return;
            if (FormHelper.StringError(lastName, "Please enter valid last name.")) return;
            if (FormHelper.StringError(streetNumber, "Please enter valid street number.")) return;
            if (FormHelper.StringError(streetName, "Please enter valid street name.")) return;
            if (FormHelper.StringError(postcode, "Please enter valid postcode.")) return;
            if (FormHelper.StringError(country, "Please enter valid country.")) return;
            if (FormHelper.StringError(city, "Please enter valid city.")) return;
            if (FormHelper.StringError(phone, "Please enter valid phone number.")) return;

            if (StartScreen.radOracle.Checked)
            {
                string query;
                if (updating)
                    query = "update client set FIRSTNAME = '" + firstName + "', LASTNAME = '" + lastName + "', STREETNUMBER = " + streetNumber + ", STREETNAME = '" + streetName + "', CITY = '" + city + "', POSTCODE = " + postcode + ", COUNTRY = '" + country + "', PHONE = " + phone + ", CAREGIVERID = " + caregiverID + " WHERE CLIENTID = '" + clientID + "'";
                else
                    query = "insert into client values(clientai.nextval, '" + firstName + "', '" + lastName + "', " + streetNumber + ", '" + streetName + "', '" + city + "', " + postcode + ", '" + country + "', " + phone + ", TO_DATE('" + date + "', 'DD/MM/YYYY HH:MI:SS AM'), " + caregiverID + ")";

                if (SQLHelper.ExecuteModifyQuery(query) != -1)
                {
                    ClearInput();
                    if (updating) FormExit(null, null);
                }
            }
            else
            {
                var clientCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Client");
                var last = Builders<BsonDocument>.Filter.Empty;

                int idnum = 1;
                for (int i = 0; i < clientCollection.Find(last).ToListAsync().Result.Count; i++)
                {
                    idnum++;
                }
                BsonDocument clientDocument = new BsonDocument
                {
                    {"clientid", new BsonInt32(idnum) },
                    {"firstname", firstName},
                    {"lastname", lastName },
                    {"streeetnumber", new BsonInt32(int.Parse(streetNumber) )},
                    {"streetname", streetName },
                    {"city", city },
                    {"postcode", new BsonInt32(int.Parse(postcode)) },
                    {"country", country },
                    {"phone", new BsonInt32(int.Parse(phone)) },
                    {"dateofbirth", date.ToShortDateString() },
                    {"caregiverid", new BsonInt32(int.Parse(caregiverID)) }
                };

                clientCollection.InsertOne(clientDocument);
                MessageBox.Show("(Mongo) 1 Row Affected");
                ClearInput();
            }
        }
    }
}
