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
    public partial class AddCaregiver : ModifiableForm
    {
        string dropDownQuery = "SELECT COMPANYNAME, COMPANYNAME FROM COMPANY";
        List<string> companyNames = new List<string>();
        string caregiverID;
        public AddCaregiver()
        {
            InitializeComponent();
            SetupButtons();
        }
        public override void UpdateForm()
        {
            base.UpdateForm();
            caregiverID = SQLHelper.identifiers[StartScreen.displayList.SelectedIndex - 1];
            DataTable table = SQLHelper.GetDataTable("SELECT * FROM CAREGIVER WHERE CAREGIVERID = '" + caregiverID + "'");
            txtFirstName.Text = table.Rows[0][1].ToString();
            txtLastName.Text = table.Rows[0][2].ToString();
            datePicker.Value = Convert.ToDateTime(table.Rows[0][3]);
            txtPhone.Text = table.Rows[0][4].ToString();
            compDropdownName.SelectedIndex = companyNames.IndexOf(table.Rows[0][5].ToString());
        }

        public override void ShowForm()
        {
            base.ShowForm();
            if (StartScreen.radOracle.Checked) FillDropDown(dropDownQuery, "Something Went Wrong", ref companyNames, ref compDropdownName);
            else FillDropDown("Company", ref companyNames, ref compDropdownName);
        }

        public void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime date = datePicker.Value;
            string firstName = FormHelper.FormatWord(txtFirstName.Text.Trim()),
                lastName = FormHelper.FormatWord(txtLastName.Text.Trim()),
                phone = FormHelper.FormatNumber(txtPhone.Text.Trim());

            if (FormHelper.StringError(firstName, "Please enter valid first name.")) return;
            if (FormHelper.StringError(lastName, "Please enter valid last name.")) return;
            if (FormHelper.StringError(phone, "Please enter valid phone number.")) return;

            if (StartScreen.radOracle.Checked)
            {
                string query;
                if (updating)
                    query = "UPDATE CAREGIVER SET FIRSTNAME = '" + firstName + "', LASTNAME = '" + lastName + "', PHONE = " + phone + ", CAREGIVERCOMPANY = '" + compDropdownName.SelectedItem.ToString() + "' WHERE CAREGIVERID = " + caregiverID;
                else
                    query = "INSERT INTO CAREGIVER VALUES(CAREGIVERAI.NEXTVAL, '" + firstName + "', '" + lastName + "', TO_DATE('" + date + "', 'DD/MM/YYYY HH:MI:SS AM'), " + phone + ", '" + compDropdownName.SelectedItem.ToString() + "')";

                if (SQLHelper.ExecuteModifyQuery(query) != -1)
                {
                    ClearInput();
                    if (updating) FormExit(null, null);
                }
            }
            else
            {
                var caregiverCollection = MongoHelper.mongoDB.GetCollection<BsonDocument>("Caregiver");
                var last = Builders<BsonDocument>.Filter.Empty;

                int idnum = 1;
                for (int i = 0; i < caregiverCollection.Find(last).ToListAsync().Result.Count; i++)
                {
                    idnum++;
                }
                BsonDocument caregiverDocument = new BsonDocument
                {
                    {"caregiverid", new BsonInt32(idnum) },
                    {"firstname", firstName },
                    {"lastname", lastName },
                    {"dateofbirth", date.ToShortDateString() },
                    {"phone", new BsonInt32(int.Parse(phone)) },
                    {"companyname", compDropdownName.SelectedItem.ToString() }
                };
                caregiverCollection.InsertOne(caregiverDocument);
                MessageBox.Show("(MONGO) 1 Row Affected");
                ClearInput();
            }
        }
    }
}
